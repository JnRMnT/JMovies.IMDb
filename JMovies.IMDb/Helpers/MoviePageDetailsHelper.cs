using HtmlAgilityPack;
using JMovies.IMDb.Entities.IMDB;
using Fizzler.Systems.HtmlAgilityPack;
using JMovies.IMDb.Common.Constants;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
using JMovies.IMDb.Entities.Common;

namespace JMovies.IMDb.Helpers
{
    public class MoviePageDetailsHelper
    {
        public static void Parse(Movie movie, HtmlNode detailsSection)
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