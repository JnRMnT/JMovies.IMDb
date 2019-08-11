﻿using HtmlAgilityPack;
using JMovies.IMDb.Entities.Movies;
using JMovies.IMDb.Entities.People;
using Fizzler.Systems.HtmlAgilityPack;
using JMovies.IMDb.Common.Constants;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Text;
using JMovies.IMDb.Providers;
using System.Linq;

namespace JMovies.IMDb.Helpers.Movies
{
    public class MoviePageDetailsHelper
    {
        public static bool Parse(IMDbScraperDataProvider providerInstance, Movie movie, HtmlNode documentNode, string moviePageUrl, bool fetchDetailedCast)
        {
            HtmlNode titleTypeTag = documentNode.QuerySelector("meta[property='og:type']");
            if (titleTypeTag != null && titleTypeTag.Attributes["content"].Value == IMDbConstants.TVSeriesOgType)
            {
                //Initialize movie as TV Series
                movie = new TVSeries
                {
                    IMDbID = movie.IMDbID
                };
            }

            //Parse Title
            HtmlNode titleWrapper = documentNode.QuerySelector(".title_wrapper");
            if (titleWrapper != null)
            {
                movie.Title = titleWrapper.QuerySelector("h1").InnerText.Prepare();
                if (IMDbConstants.MovieYearRegex.IsMatch(movie.Title))
                {
                    Match yearMatch = IMDbConstants.MovieYearRegex.Match(movie.Title);
                    movie.Year = yearMatch.Groups[2].Value.Trim().ToInteger();
                    movie.Title = yearMatch.Groups[1].Value.Trim();
                }
                HtmlNode originalTitleNode = titleWrapper.QuerySelector(".originalTitle");
                if (originalTitleNode != null)
                {
                    movie.OriginalTitle = originalTitleNode.InnerText.Prepare();
                }

                foreach (HtmlNode titleLink in titleWrapper.QuerySelectorAll("a"))
                {
                    if (titleLink.OuterHtml.Contains("/releaseinfo"))
                    {
                        Match yearMatch = IMDbConstants.MovieYearRegex.Match(titleLink.InnerText.Prepare());
                        if (yearMatch.Success)
                        {
                            movie.Year = yearMatch.Groups[2].Value.Trim().ToInteger();
                            if (yearMatch.Groups.Count > 3)
                            {
                                (movie as TVSeries).EndYear = yearMatch.Groups[3].Value.Trim().ToInteger();
                            }
                        }
                    }
                }
            }
            else
            {
                return false;
            }

            //Parse Summary
            HtmlNode summaryWrapper = documentNode.QuerySelector(".plot_summary_wrapper");
            List<Credit> credits = new List<Credit>();
            if (summaryWrapper != null)
            {
                HtmlNode summaryText = summaryWrapper.QuerySelector(".summary_text");
                if (summaryText != null)
                {
                    movie.PlotSummary = summaryText.InnerText.Prepare();
                }

                foreach (HtmlNode creditSummaryNode in summaryWrapper.QuerySelectorAll(".credit_summary_item"))
                {
                    Credit[] summaryCredits = SummaryCastHelper.GetCreditInfo(creditSummaryNode);
                    if (summaryCredits != null && summaryCredits.Length > 0)
                    {
                        credits.AddRange(summaryCredits);
                    }
                }
            }
            else
            {
                return false;
            }

            //Parse Story Line
            HtmlNode storyLineSection = documentNode.QuerySelector("#titleStoryLine");
            if (storyLineSection != null)
            {
                SummaryStorylineHelper.Parse(movie, storyLineSection);
            }

            //Parse Details Section
            HtmlNode detailsSection = documentNode.QuerySelector("#titleDetails");
            if (detailsSection != null)
            {
                MoviePageDetailsHelper.ParseDetailsSection(movie, detailsSection);
            }

            if (!fetchDetailedCast)
            {
                //Parse Cast Table
                HtmlNode castListNode = documentNode.QuerySelector(".cast_list");
                ParseCastList(movie, credits, castListNode);
            }
            else
            {
                //Fetch credits through full credits page
                string fullCreditsUrl = moviePageUrl + "/" + IMDbConstants.FullCreditsPath;
                WebRequest fullCreditsPageRequest = HttpHelper.InitializeWebRequest(fullCreditsUrl);
                HtmlDocument creditsPageDocument = HtmlHelper.GetNewHtmlDocument();
                using (Stream stream = HttpHelper.GetResponseStream(fullCreditsPageRequest))
                {
                    creditsPageDocument.Load(stream, Encoding.UTF8);
                }
                HtmlNode fullCreditsPageDocumentNode = creditsPageDocument.DocumentNode;
                HtmlNode fullCreditsPageCastListNode = fullCreditsPageDocumentNode.QuerySelector(".cast_list");
                ParseCastList(movie, credits, fullCreditsPageCastListNode);
                movie.Credits = credits.ToArray();
            }

            #region Parse Ratings
            HtmlNode ratingsWrapper = documentNode.QuerySelector(".imdbRating");
            HtmlNode ratingNode = ratingsWrapper.QuerySelector("span[itemprop='ratingValue']");
            HtmlNode ratingCountNode = ratingsWrapper.QuerySelector("span[itemprop='ratingCount']");
            movie.Rating = new Rating();
            movie.Rating.Value = double.Parse(ratingNode.InnerText.Prepare().Replace('.',','));
            movie.Rating.RateCount = ratingCountNode.InnerText.Prepare().Replace(",", string.Empty).ToLong();
            #endregion
            return true;
        }

        private static void ParseCastList(Movie movie, List<Credit> credits, HtmlNode castListNode)
        {
            if (castListNode != null)
            {
                foreach (HtmlNode castNode in castListNode.QuerySelectorAll("tr"))
                {
                    IEnumerable<HtmlNode> castColumns = castNode.QuerySelectorAll("td");
                    if (castColumns != null && castColumns.Count() == 4)
                    {
                        HtmlNode personNode = castColumns.ElementAt(1);
                        HtmlNode charactersNode = castColumns.ElementAt(3);

                        ActingCredit actingCredit = new ActingCredit();
                        actingCredit.Person = new Actor();
                        Match personIDMatch = IMDbConstants.PersonIDURLMatcher.Match(personNode.QuerySelector("a").Attributes["href"].Value);
                        if (personIDMatch.Success && personIDMatch.Groups.Count > 1)
                        {
                            actingCredit.Person.IMDbID = personIDMatch.Groups[1].Value.ToLong();
                            actingCredit.Person.FullName = personNode.InnerText.Prepare();
                        }

                        List<Character> characters = new List<Character>();
                        foreach (HtmlNode characterNode in charactersNode.QuerySelectorAll("a"))
                        {
                            Character character = GetCharacter(characterNode, characters, movie);
                            if (character != null)
                            {
                                characters.Add(character);
                            }
                        }
                        if (characters.Count == 0)
                        {
                            Character character = GetCharacter(charactersNode.FirstChild, characters, movie);
                            if (character != null && (!string.IsNullOrEmpty(character.Name) || character.IMDbID != null))
                            {
                                characters.Add(character);
                            }
                        }
                        actingCredit.Characters = characters.ToArray();
                        credits.Add(actingCredit);
                    }
                }
                movie.Credits = credits.ToArray();
            }
        }
        
        private static Character GetCharacter(HtmlNode characterNode, List<Character> characters, Movie movie)
        {
            if (!characterNode.GetClasses().Any(e => e == "toggle-episodes"))
            {
                Character character = null;
                if (movie is TVSeries)
                {
                    character = new TVCharacter();
                }
                else
                {
                    character = new Character();
                }

                HtmlNode episodeInformationNode = characterNode.ParentNode.QuerySelector(".toggle-episodes");
                if (episodeInformationNode == null)
                {
                    character.Name = Regex.Replace(Regex.Replace(characterNode.InnerText.Prepare(), @"\n", string.Empty), @"\s+", @" ").Trim();
                }
                else
                {
                    character.Name = Regex.Replace(Regex.Replace(characterNode.InnerText.Prepare(), @"\n", string.Empty), @"\s+", @" ").Trim();
                    Match characterEpisodeInfoMatch = IMDbConstants.CharacterEpisodeInfoRegex.Match(episodeInformationNode.InnerText.Prepare());
                    if (characterEpisodeInfoMatch.Success)
                    {
                        TVCharacter tvCharacter = character as TVCharacter;
                        tvCharacter.EpisodeCount = characterEpisodeInfoMatch.Groups[1].Value.ToInteger();
                        tvCharacter.StartYear = characterEpisodeInfoMatch.Groups[2].Value.ToInteger();
                        if (characterEpisodeInfoMatch.Groups.Count > 3)
                        {
                            tvCharacter.EndYear = characterEpisodeInfoMatch.Groups[3].Value.ToInteger();
                        }
                    }
                }
                if (IMDbConstants.CharacterRegex.IsMatch(characterNode.OuterHtml))
                {
                    character.IMDbID = IMDbConstants.CharacterRegex.Match(characterNode.OuterHtml).Groups[1].Value.ToLong();
                }
                return character;
            }
            else
            {
                return null;
            }
        }

        public static void ParseDetailsSection(Movie movie, HtmlNode detailsSection)
        {
            foreach (HtmlNode detailBox in detailsSection.QuerySelectorAll(".txt-block"))
            {
                HtmlNode headerNode = detailBox.QuerySelector("h4");
                if (headerNode != null)
                {
                    string headerContent = headerNode.InnerText.Prepare();
                    if (IMDbConstants.OfficialSitesHeaderRegex.IsMatch(headerContent))
                    {
                        List<OfficialSite> officialSites = new List<OfficialSite>();
                        Parallel.ForEach(detailBox.QuerySelectorAll("a"), (HtmlNode officialSiteLink) =>
                        {
                            try
                            {
                                string url = IMDbConstants.BaseURL + officialSiteLink.Attributes["href"].Value;
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                                request.AllowAutoRedirect = false;
                                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                                {
                                    string redirectURL = response.Headers["Location"];
                                    officialSites.Add(new OfficialSite
                                    {
                                        Title = officialSiteLink.InnerText.Prepare(),
                                        URL = redirectURL
                                    });
                                }
                            }
                            catch
                            {
                                //simply ignore official site errors
                            }
                        });
                        movie.OfficialSites = officialSites.ToArray();
                    }
                    else if (IMDbConstants.CountriesHeaderRegex.IsMatch(headerContent))
                    {
                        List<Country> countries = new List<Country>();
                        foreach (HtmlNode countryLink in detailBox.QuerySelectorAll("a"))
                        {
                            Match countryMatch = IMDbConstants.CountryOfOriginRegex.Match(countryLink.OuterHtml);
                            if (countryMatch.Success)
                            {
                                Country country = new Country
                                {
                                    Identifier = countryMatch.Groups[1].Value,
                                    Name = countryLink.InnerText.Prepare()
                                };
                                countries.Add(country);
                            }
                        }
                        movie.Countries = countries.ToArray();
                    }
                    else if (IMDbConstants.LanguagesHeaderRegex.IsMatch(headerContent))
                    {
                        List<Language> languages = new List<Language>();
                        foreach (HtmlNode languageLink in detailBox.QuerySelectorAll("a"))
                        {
                            Match languageMatch = IMDbConstants.PrimaryLanguageRegex.Match(languageLink.OuterHtml);
                            if (languageMatch.Success)
                            {
                                Language language = new Language
                                {
                                    Identifier = languageMatch.Groups[1].Value,
                                    Name = languageLink.InnerText.Prepare()
                                };
                                languages.Add(language);
                            }
                        }
                        movie.Languages = languages.ToArray();
                    }
                    else if (IMDbConstants.ReleaseDateHeaderRegex.IsMatch(headerContent))
                    {
                        Match releaseDateMatch = IMDbConstants.ReleaseDateRegex.Match(headerNode.NextSibling.InnerText.Prepare());
                        if (releaseDateMatch.Success)
                        {
                            ReleaseDate releaseDate = new ReleaseDate();
                            releaseDate.Country = new Country();
                            releaseDate.Country.Name = releaseDateMatch.Groups[2].Value;
                            releaseDate.Date = DateTime.Parse(releaseDateMatch.Groups[1].Value);
                            movie.ReleaseDates = new ReleaseDate[] { releaseDate };
                        }
                    }
                    else if (IMDbConstants.AKAHeaderRegex.IsMatch(headerContent))
                    {
                        AKA aka = new AKA
                        {
                            Name = headerNode.NextSibling.InnerText.Prepare()
                        };
                        movie.AKAs = new AKA[] { aka };
                    }
                    else if (IMDbConstants.FilmingLocationsHeaderRegex.IsMatch(headerContent))
                    {
                        List<string> filmingLocations = new List<string>();
                        foreach (HtmlNode locationLinkNode in headerNode.ParentNode.QuerySelectorAll("a"))
                        {
                            Match locationLinkMatch = IMDbConstants.LocationsLinkRegex.Match(locationLinkNode.OuterHtml);
                            if (locationLinkMatch.Success)
                            {
                                filmingLocations.Add(locationLinkMatch.Groups[1].Value.Prepare());
                            }
                        }
                        movie.FilmingLocations = filmingLocations.ToArray();
                    }
                    else if (IMDbConstants.BudgetHeaderRegex.IsMatch(headerContent))
                    {
                        movie.Budget = new Budget();
                        movie.Budget.Amount = headerNode.NextSibling.InnerText.Prepare().ToAmount();
                        movie.Budget.Description = string.Empty;
                        foreach (HtmlNode attributeNode in headerNode.ParentNode.QuerySelectorAll(".attribute"))
                        {
                            Match attributeMatch = GeneralRegexConstants.PharantesisRegex.Match(attributeNode.InnerText.Prepare());
                            if (attributeMatch.Success)
                            {
                                if (!string.IsNullOrEmpty(movie.Budget.Description))
                                {
                                    movie.Budget.Description += " ";
                                }
                                movie.Budget.Description += attributeMatch.Groups[1].Value;
                            }
                        }
                    }
                    else if (IMDbConstants.ProductionCompanyHeaderRegex.IsMatch(headerContent))
                    {
                        List<Company> productionCompanies = new List<Company>();
                        foreach (HtmlNode productionCompanyNode in headerNode.ParentNode.QuerySelectorAll("a"))
                        {
                            Match companyIDMatch = IMDbConstants.ProductionCompanyLinkRegex.Match(productionCompanyNode.Attributes["href"].Value);
                            if (companyIDMatch.Success)
                            {
                                Company productionCompany = new Company();
                                productionCompany.Name = productionCompanyNode.InnerText.Prepare();
                                productionCompany.ID = companyIDMatch.Groups[1].Value.ToLong();
                                productionCompanies.Add(productionCompany);
                            }
                        }
                        movie.ProductionCompanies = productionCompanies.ToArray();
                    }
                    else if (IMDbConstants.RuntimeHeaderRegex.IsMatch(headerContent))
                    {
                        movie.Runtime = headerNode.ParentNode.QuerySelector("time").Attributes["datetime"].Value.ToHtmlTimeSpan();
                    }
                }
            }
        }

    }
}