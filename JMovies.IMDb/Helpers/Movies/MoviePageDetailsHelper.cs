using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using JMovies.IMDb.Common.Constants;
using JMovies.IMDb.Entities.Common;
using JMovies.IMDb.Entities.Misc;
using JMovies.IMDb.Entities.Movies;
using JMovies.IMDb.Entities.Movies.LDJson;
using JMovies.IMDb.Entities.People;
using JMovies.IMDb.Entities.PrivateAPI;
using JMovies.IMDb.Entities.Settings;
using JMovies.IMDb.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace JMovies.IMDb.Helpers.Movies
{
    /// <summary>
    /// Class responsible for parsing the Movie Page Details
    /// </summary>
    public static class MoviePageDetailsHelper
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
            HtmlNode blockTitleElement = documentNode.QuerySelector("[data-testid=hero-title-block__title]");
            HtmlNode simpleTitleElement = documentNode.QuerySelector("[data-testid=hero__pageTitle]");
            if (blockTitleElement != null || simpleTitleElement != null)
            {
                HtmlNode titleWrapper = null;
                if (blockTitleElement != null)
                {
                    titleWrapper = blockTitleElement.ParentNode.ParentNode;
                }
                else
                {
                    titleWrapper = simpleTitleElement.ParentNode.ParentNode;
                }
                HtmlNode documentTitle = documentNode.QuerySelector("title");
                string documentTitleText = documentTitle.InnerText.Prepare();
                movie.Title = titleWrapper.QuerySelector("h1").InnerText.Prepare();
                if (IMDbConstants.MovieYearRegex.IsMatch(documentTitleText))
                {
                    Match yearMatch = IMDbConstants.MovieYearRegex.Match(documentTitleText);
                    movie.Year = yearMatch.Groups[1].Value.Trim().ToInteger();
                }
                HtmlNode originalTitleNode = titleWrapper.QuerySelector("[data-testid=hero-title-block__original-title]");
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
                            movie.Year = yearMatch.Groups[1].Value.Trim().ToInteger();
                            if (yearMatch.Groups.Count > 2)
                            {
                                string endYearString = yearMatch.Groups[2].Value.Trim();
                                if (!string.IsNullOrEmpty(endYearString))
                                {
                                    (movie as TVSeries).EndYear = yearMatch.Groups[2].Value.Trim().ToInteger();
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

            string alternativePosterURL = null;
            //parse metadata
            HtmlNode ldJsonMetadataNode = documentNode.QuerySelector("script[type='application/ld+json']");
            if (ldJsonMetadataNode != null)
            {
                Metadata metadata = JsonSerializer.Deserialize<Metadata>(ldJsonMetadataNode.InnerText);
                movie.StoryLine = metadata.Description;
                movie.Genres = metadata.Genres?.Select(e => new Genre { Identifier = e, Value = e }).ToArray();
                movie.Keywords = metadata.Keywords?.Split(',').Select(e => new Keyword { Identifier = e, Value = e }).ToArray();
                if (!string.IsNullOrEmpty(metadata.Image))
                {
                    alternativePosterURL = metadata.Image;
                }
            }

            HtmlNode posterWrapperNode = documentNode.QuerySelector("[data-testid=hero-media__poster]");
            if (posterWrapperNode != null)
            {
                HtmlNode posterNode = posterWrapperNode.QuerySelector("img");
                if (posterNode != null)
                {
                    movie.Poster = new Image
                    {
                        Title = posterNode.GetAttributeValue("alt", string.Empty).Prepare(),
                        URL = IMDBImageHelper.NormalizeImageUrl(posterNode.GetAttributeValue("src", string.Empty))
                    };
                    if (settings.FetchImageContents)
                    {
                        movie.Poster.Content = IMDBImageHelper.GetImageContent(movie.Poster.URL);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(alternativePosterURL))
            {
                //Use poster from metadata
                movie.Poster = new Image { URL = alternativePosterURL };
                if (settings.FetchImageContents)
                {
                    movie.Poster.Content = IMDBImageHelper.GetImageContent(movie.Poster.URL);
                }
            }

            //Parse Summary
            HtmlNode summaryWrapper = documentNode.QuerySelector("[data-testid=plot]");
            if (summaryWrapper != null)
            {
                HtmlNode summaryText = summaryWrapper.Children().LastOrDefault(e => !string.IsNullOrEmpty(e.InnerText));
                if (summaryText != null)
                {
                    movie.PlotSummary = summaryText.InnerText.Prepare();
                    if (movie.PlotSummary.StartsWith(IMDbConstants.EmptyPlotText))
                    {
                        movie.PlotSummary = string.Empty;
                    }
                }
            }
            else
            {
                return false;
            }
            HtmlNode summaryCreditsWrapper = documentNode.QuerySelector("[data-testid=title-pc-wide-screen]");
            List<Credit> credits = new List<Credit>();
            if (summaryCreditsWrapper != null)
            {
                foreach (HtmlNode creditSummaryNode in summaryCreditsWrapper.QuerySelectorAll("[data-testid=title-pc-principal-credit]"))
                {
                    List<Credit> summaryCredits = SummaryCastHelper.GetCreditInfo(creditSummaryNode);
                    if (summaryCredits != null && summaryCredits.Count > 0)
                    {
                        credits.AddRange(summaryCredits);
                    }
                }
            }

            //Parse Story Line
            HtmlNode storyLineSection = documentNode.QuerySelector("[data-testid=Storyline]");
            if (storyLineSection != null)
            {
                SummaryStorylineHelper.Parse(movie, storyLineSection);
            }

            //Parse Details Section
            HtmlNode detailsSection = documentNode.QuerySelector("[data-testid=title-details-section] > ul");
            if (detailsSection != null)
            {
                ParseDetailsSection(movie, detailsSection);
            }

            //Parse Technical Specs Section
            HtmlNode technicalSpecsSection = documentNode.QuerySelector("[data-testid=title-techspecs-section] > ul");
            if (technicalSpecsSection != null)
            {
                ParseDetailsSection(movie, technicalSpecsSection);
            }

            if (!settings.FetchDetailedCast)
            {
                //Parse Cast Table
                HtmlNode castListNode = documentNode.QuerySelector("[data-testid=title-cast]");
                SummaryCreditsHelper.ParseCastList(movie, credits, castListNode);
            }
            else
            {
                //Fetch credits through full credits page
                string fullCreditsUrl = moviePageUrl + "/" + IMDbConstants.FullCreditsPath;
                WebRequest fullCreditsPageRequest = HttpHelper.InitializeWebRequest(fullCreditsUrl, settings);
                HtmlDocument creditsPageDocument = HtmlHelper.GetNewHtmlDocument();
                using (Stream stream = HttpHelper.GetResponseStream(fullCreditsPageRequest, settings))
                {
                    creditsPageDocument.Load(stream, Encoding.UTF8);
                }
                HtmlNode fullCreditsPageDocumentNode = creditsPageDocument.DocumentNode;
                HtmlNode fullCreditsPageCastListNode = fullCreditsPageDocumentNode.QuerySelector(".cast_list");
                ParseCastListAsLegacyTable(movie, credits, fullCreditsPageCastListNode);
                movie.Credits = credits;
            }

            #region  Parse Relase Info Page
            string releaseInfoURL = moviePageUrl + "/" + IMDbConstants.ReleaseInfoPath;
            WebRequest releaseInfoPageRequest = HttpHelper.InitializeWebRequest(releaseInfoURL, settings);
            HtmlDocument releaseInfoPageDocument = HtmlHelper.GetNewHtmlDocument();
            using (Stream stream = HttpHelper.GetResponseStream(releaseInfoPageRequest, settings))
            {
                releaseInfoPageDocument.Load(stream, Encoding.UTF8);
            }
            ReleaseInfoPageHelper.Parse(movie, releaseInfoPageDocument, settings);
            #endregion
            #region Parse Ratings
            Match ratingMatch = IMDbConstants.RatingJSONLDMatcher.Match(documentNode.InnerHtml);
            if (ratingMatch.Success)
            {
                string ratingContextJSON = ratingMatch.Value;
                string ratingValue = IMDbConstants.RatingValueMatcher.Match(ratingContextJSON).Groups?[1].Value.Prepare();
                string ratingCount = IMDbConstants.RatingCountMatcher.Match(ratingContextJSON).Groups?[1].Value.Prepare();
                movie.Rating = new Rating(DataSourceTypeEnum.IMDb, movie);
                movie.Rating.Value = ParseRatingValue(ratingValue);
                movie.Rating.RateCount = ratingCount.Replace(",", string.Empty).ToLong();
            }
            #endregion

            #region Parse Photo Gallery Page
            if (settings.MediaImagesFetchCount > 0)
            {
                PhotoGalleryPageHelper.Parse(movie, moviePageUrl, settings);
            }
            #endregion
            return true;
        }

        /// <summary>
        /// Method responsible for parsing the rating value from string to a double
        /// </summary>
        /// <param name="ratingString">Rating value as a string</param>
        private static double ParseRatingValue(string ratingString)
        {
            string decSep = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
            if (decSep == ",") ratingString = ratingString.Replace('.', ',');
            double value = double.Parse(ratingString);
            return value;
        }

        /// <summary>
        /// Method responsible for parsing the cast list of the movie
        /// </summary>
        /// <param name="movie">Movie instance to be populated</param>
        /// <param name="credits">Credits list to be filled</param>
        /// <param name="castListNode">Html node that holds the cast list</param>
        private static void ParseCastListAsLegacyTable(Movie movie, List<Credit> credits, HtmlNode castListNode)
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
            foreach (HtmlNode detailBox in detailsSection.ChildNodes.Where(e => e.Name == "li"))
            {
                HtmlNode headerNode = detailBox.FirstChild;
                if (headerNode != null)
                {
                    string headerContent = headerNode.InnerText.Prepare();
                    if (IMDbConstants.OfficialSitesHeaderRegex.IsMatch(headerContent))
                    {
                        List<OfficialSite> officialSites = new List<OfficialSite>();
                        foreach (HtmlNode officialSiteLink in detailBox.QuerySelectorAll("a"))
                        {
                            string url = officialSiteLink.Attributes["href"].Value;
                            officialSites.Add(new OfficialSite
                            {
                                Title = officialSiteLink.InnerText.Prepare(),
                                URL = url
                            });
                        }
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
                        HtmlNode timeNode = headerNode.ParentNode.QuerySelector("time");
                        if (timeNode != null)
                        {
                            movie.Runtime = timeNode.Attributes["datetime"].Value.ToHtmlTimeSpan();
                        }
                        else
                        {
                            movie.Runtime = detailBox.LastChild.InnerText.Prepare().HumanReadableToTimeSpan();
                        }
                    }
                }
            }
        }

    }
}