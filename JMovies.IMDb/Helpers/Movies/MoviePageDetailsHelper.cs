using HtmlAgilityPack;
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
using JMovies.IMDb.Entities.Misc;
using JMovies.IMDb.Entities.Common;
using JMovies.IMDb.Entities.Settings;

namespace JMovies.IMDb.Helpers.Movies
{
    /// <summary>
    /// Class responsible for parsing the Movie Page Details
    /// </summary>
    public class MoviePageDetailsHelper
    {
        /// <summary>
        /// Main Parse method of the Movie Page
        /// </summary>
        /// <param name="providerInstance">Instance reference of the IMDbScraperDataProvider</param>
        /// <param name="movie">Movie instance that is populated</param>
        /// <param name="documentNode">Document Node of the movie page</param>
        /// <param name="moviePageUrl">URL of the movie page</param>
        /// <param name="settings">Object containing Data Fetch settings</param>
        /// <returns>If scraping was successful or not</returns>
        public static bool Parse(IMDbScraperDataProvider providerInstance, ref Movie movie, HtmlNode documentNode, string moviePageUrl, ProductionDataFetchSettings settings)
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
                                string endYearString = yearMatch.Groups[3].Value.Trim();
                                if (!string.IsNullOrEmpty(endYearString))
                                {
                                    (movie as TVSeries).EndYear = yearMatch.Groups[3].Value.Trim().ToInteger();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                return false;
            }

            HtmlNode posterNode = documentNode.QuerySelector(".poster img");
            if (posterNode != null)
            {
                movie.Poster = new Image
                {
                    Title = posterNode.GetAttributeValue("title", string.Empty),
                    URL = IMDBImageHelper.NormalizeImageUrl(posterNode.GetAttributeValue("src", string.Empty))
                };
                if (settings.FetchImageContents)
                {
                    movie.Poster.Content = IMDBImageHelper.GetImageContent(movie.Poster.URL);
                }
            }

            //Parse Summary
            HtmlNode summaryWrapper = documentNode.QuerySelector(".plot_summary_wrapper");
            List<Credit> credits = new List<Credit>();
            if (summaryWrapper != null)
            {
                HtmlNode summaryText = summaryWrapper.QuerySelector(".summary_text");
                if (summaryText != null)
                {
                    movie.PlotSummary = summaryText.FirstChild.InnerText.Prepare();
                    if (movie.PlotSummary.StartsWith(IMDbConstants.EmptyPlotText))
                    {
                        movie.PlotSummary = string.Empty;
                    }
                }

                foreach (HtmlNode creditSummaryNode in summaryWrapper.QuerySelectorAll(".credit_summary_item"))
                {
                    List<Credit> summaryCredits = SummaryCastHelper.GetCreditInfo(creditSummaryNode);
                    if (summaryCredits != null && summaryCredits.Count > 0)
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

            if (!settings.FetchDetailedCast)
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
                movie.Credits = credits;
            }

            #region  Parse Relase Info Page
            string releaseInfoURL = moviePageUrl + "/" + IMDbConstants.ReleaseInfoPath;
            WebRequest releaseInfoPageRequest = HttpHelper.InitializeWebRequest(releaseInfoURL);
            HtmlDocument releaseInfoPageDocument = HtmlHelper.GetNewHtmlDocument();
            using (Stream stream = HttpHelper.GetResponseStream(releaseInfoPageRequest))
            {
                releaseInfoPageDocument.Load(stream, Encoding.UTF8);
            }
            ReleaseInfoPageHelper.Parse(movie, releaseInfoPageDocument);
            #endregion
            #region Parse Ratings
            HtmlNode ratingsWrapper = documentNode.QuerySelector(".imdbRating");
            if (ratingsWrapper != null)
            {
                HtmlNode ratingNode = ratingsWrapper.QuerySelector("span[itemprop='ratingValue']");
                HtmlNode ratingCountNode = ratingsWrapper.QuerySelector("span[itemprop='ratingCount']");
                movie.Rating = new Rating(DataSourceTypeEnum.IMDb, movie);
                movie.Rating.Value = double.Parse(ratingNode.InnerText.Prepare().Replace('.', ','));
                movie.Rating.RateCount = ratingCountNode.InnerText.Prepare().Replace(",", string.Empty).ToLong();
            }
            #endregion

            #region Parse Photo Gallery Page
            if (settings.MediaImagesFetchCount > 0)
            {
                string photoGalleryURL = moviePageUrl + "/" + IMDbConstants.PhotoGalleryPath;
                WebRequest photoGalleryPageRequest = HttpHelper.InitializeWebRequest(photoGalleryURL);
                HtmlDocument photoGalleryPageDocument = HtmlHelper.GetNewHtmlDocument();
                using (Stream stream = HttpHelper.GetResponseStream(photoGalleryPageRequest))
                {
                    photoGalleryPageDocument.Load(stream, Encoding.UTF8);
                }
                PhotoGalleryPageHelper.Parse(movie, photoGalleryPageDocument?.DocumentNode, settings);
            }
            #endregion
            return true;
        }

        /// <summary>
        /// Method responsible for parsing the cast list of the movie
        /// </summary>
        /// <param name="movie">Movie instance to be populated</param>
        /// <param name="credits">Credits list to be filled</param>
        /// <param name="castListNode">Html node that holds the cast list</param>
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
                            Character character = GetCharacter(characterNode, movie);
                            if (character != null)
                            {
                                characters.Add(character);
                            }
                        }
                        if (characters.Count == 0)
                        {
                            Character character = GetCharacter(charactersNode.FirstChild, movie);
                            if (character != null && (!string.IsNullOrEmpty(character.Name) || character.IMDbID != null))
                            {
                                characters.Add(character);
                            }
                        }
                        actingCredit.Characters = characters;
                        credits.Add(actingCredit);
                    }
                }
                movie.Credits = credits;
            }
        }

        /// <summary>
        /// Method responsible for parsing a single character information
        /// </summary>
        /// <param name="characterNode">HTML Node that holds the character information</param>
        /// <param name="movie">Movie instance that is populated</param>
        /// <returns>Parsed character instance</returns>
        private static Character GetCharacter(HtmlNode characterNode, Movie movie)
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

        /// <summary>
        /// Method responsible for parsing the details section of the movie page
        /// </summary>
        /// <param name="movie">Movie instance to be populated</param>
        /// <param name="detailsSection">HTML Node containing the Details section</param>
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
                        movie.OfficialSites = officialSites;
                    }
                    else if (IMDbConstants.CountriesHeaderRegex.IsMatch(headerContent))
                    {
                        List<ProductionCountry> countries = new List<ProductionCountry>();
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
                                countries.Add(new ProductionCountry
                                {
                                    Country = country,
                                    Production = movie
                                });
                            }
                        }
                        movie.Countries = countries;
                    }
                    else if (IMDbConstants.LanguagesHeaderRegex.IsMatch(headerContent))
                    {
                        List<ProductionLanguage> languages = new List<ProductionLanguage>();
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
                                languages.Add(new ProductionLanguage()
                                {
                                    Language = language,
                                    Production = movie
                                });
                            }
                        }
                        movie.Languages = languages;
                    }
                    else if (IMDbConstants.ReleaseDateHeaderRegex.IsMatch(headerContent))
                    {
                        //Release Dates are fetched from release info page seperately
                    }
                    else if (IMDbConstants.AKAHeaderRegex.IsMatch(headerContent))
                    {
                        AKA aka = new AKA
                        {
                            Name = headerNode.NextSibling.InnerText.Prepare()
                        };
                        movie.AKAs = new List<AKA>() { aka };
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
                        movie.FilmingLocations = filmingLocations;
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
                        movie.ProductionCompanies = productionCompanies;
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