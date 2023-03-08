using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using JMovies.IMDb.Common.Constants;
using JMovies.IMDb.Entities.Common;
using JMovies.IMDb.Entities.Movies;
using JMovies.IMDb.Entities.People;
using JMovies.IMDb.Entities.Settings;
using JMovies.IMDb.Factories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace JMovies.IMDb.Helpers.People
{
    /// <summary>
    /// Class responsible for parsing the person page
    /// </summary>
    public static class PersonPageHelper
    {
        /// <summary>
        /// Method responsible for parsing the person page
        /// </summary>
        /// <param name="person">Person to be populated</param>
        /// <param name="documentNode">HTML Node containing the person page</param>
        /// <param name="settings">Object containing Data Fetch settings</param>
        public static void Parse(Person person, HtmlNode documentNode, PersonDataFetchSettings settings)
        {
            #region Main Details Parsing
            HtmlNode nameContainer = documentNode.QuerySelector("[data-testid=hero__pageTitle]");
            if (nameContainer != null)
            {
                person.FullName = nameContainer.InnerText.Prepare();
                if (nameContainer.NextSibling != null)
                {
                    List<CreditRoleType> roles = new List<CreditRoleType>();
                    foreach (HtmlNode jobCategoryLink in nameContainer.NextSibling.ChildNodes)
                    {
                        CreditRoleType role = CreditRoleType.Undefined;
                        string roleText = jobCategoryLink.InnerText.Prepare();
                        Enum.TryParse(roleText, out role);
                        if (role != CreditRoleType.Undefined)
                        {
                            roles.Add(role);
                        }
                    }
                    person.Roles = roles;
                }
            }

            HtmlNode imageMetadata = documentNode.QuerySelector("meta[property='og:image']");
            if (imageMetadata != null)
            {
                if (imageMetadata != null)
                {
                    string url = imageMetadata.Attributes["content"].Value;
                    if (url.Contains("._V1_"))
                    {
                        url = Regex.Split(url, @"\._V1_")[0];
                        Image image = new Image
                        {
                            Title = person.FullName,
                            URL = IMDBImageHelper.NormalizeImageUrl(url)
                        };
                        if (settings.FetchImageContents)
                        {
                            image.Content = IMDBImageHelper.GetImageContent(image.URL);
                        }
                        person.PrimaryImage = image;
                    }
                }
            }

            #endregion
            #region Bio Page Parsing
            if (settings.FetchBioPage)
            {
                BioPageHelper.ParseBioPage(person, settings);
            }
            #endregion
            #region Filmography Parsing
            List<ProductionCredit> filmographyCredits = new List<ProductionCredit>();
            HtmlNode filmographyElement = documentNode.QuerySelector("[data-testid=nm_flmg_sec]");
            HtmlNode[] filmogpaphyCategories = filmographyElement.NextSibling.QuerySelectorAll("button.ipc-chip").ToArray();
            DetectGender(person, filmogpaphyCategories);

            #endregion
            #region Known For Parsing
            HtmlNode knownForElement = documentNode.QuerySelector("[data-testid=nm_flmg_kwn_for]").QuerySelector("[data-testid=shoveler-items-container]");
            if (knownForElement != null)
            {
                List<ProductionCredit> knowForCredits = new List<ProductionCredit>();
                foreach (HtmlNode knownForTitleNode in knownForElement.Children())
                {
                    HtmlNode titleYearElement = knownForTitleNode.QuerySelectorAll("span[data-testid],button[data-testid]")
                        .FirstOrDefault(e =>
                        {
                            string testIDAttribute = e.Attributes["data-testid"].Value;
                            return testIDAttribute.StartsWith("nm-flmg-title-metadata-")
                            || testIDAttribute.StartsWith("nm-flmg-title-year-");
                        });

                    int titleYear = default(int);
                    int? titleEndYear = null;
                    if (titleYearElement != null)
                    {
                        string titleYearString = titleYearElement.InnerText;
                        Match titleYearMatch = IMDbConstants.CreditYearRegex.Match(titleYearString);
                        if (titleYearMatch.Success)
                        {
                            titleYear = titleYearMatch.Groups[1].Value.ToInteger();
                            if (titleYearMatch.Groups.Count >= 4 && titleYearMatch.Groups[3].Success)
                            {
                                titleEndYear = titleYearMatch.Groups[3].Value.ToInteger();
                            }
                        }
                    }

                    HtmlNode roleElement = knownForTitleNode.QuerySelector(".ipc-primary-image-list-card__content-mid-bottom");
                    HtmlNode movieLink = knownForTitleNode.QuerySelector(".ipc-primary-image-list-card__content-top a");
                    ProductionCredit knownFor = new ProductionCredit();
                    if (titleEndYear != null)
                    {
                        knownFor.Production = new TVSeries { EndYear = (int)titleEndYear };
                    }
                    else
                    {
                        knownFor.Production = new Movie();
                    }

                    knownFor.Production.IMDbID = (long)IMDBIDHelper.GetIDFromUrl(movieLink.Attributes["href"].Value);
                    knownFor.Production.Title = movieLink.InnerText.Prepare();
                    knownFor.Production.Year = titleYear;

                    string role = roleElement.QuerySelector("span").InnerText.Prepare();
                    if (!Enum.TryParse(role, out CreditRoleType roleType))
                    {
                        roleType = CreditRoleType.Acting;
                        if (person.Gender == GenderEnum.Male)
                        {
                            roleType = CreditRoleType.Actor;
                        }
                        else if (person.Gender == GenderEnum.Female)
                        {
                            roleType = CreditRoleType.Actress;
                        }
                    }

                    knownFor.Credit = new CreditFactory().Build(roleType);
                    knownFor.Credit.RoleType = roleType;
                    knownFor.Credit.Person = person;
                    if (roleType == CreditRoleType.Actor || roleType == CreditRoleType.Actress || roleType == CreditRoleType.Acting)
                    {
                        ActingCredit actingCredit = (ActingCredit)knownFor.Credit;
                        actingCredit.Characters = new Character[]
                        {
                        new Character
                        {
                            Name = role
                        }
                        };
                    }

                    knowForCredits.Add(knownFor);
                }
                person.KnownFor = knowForCredits;
            }
            #endregion
        }

        /// <summary>
        /// Method responsible for detecting gender based on filmography categories
        /// </summary>
        /// <param name="person">The person which the gender will be detected</param>
        /// <param name="filmogpaphyCategories">HTML Nodes containing filmography categories</param>
        private static void DetectGender(Person person, HtmlNode[] filmogpaphyCategories)
        {
            HtmlNode actingCategory = filmogpaphyCategories.FirstOrDefault((categoryWrapper) =>
            {
                string categoryName = categoryWrapper.InnerText;
                return categoryName == IMDbConstants.ActorCategoryName || categoryName == IMDbConstants.ActressCategoryName;
            });
            if (actingCategory != null)
            {
                string categoryName = actingCategory.InnerText;
                if (categoryName == IMDbConstants.ActorCategoryName)
                {
                    person.Gender = GenderEnum.Male;
                }
                else if (categoryName == IMDbConstants.ActressCategoryName)
                {
                    person.Gender = GenderEnum.Female;
                }
            }
        }
    }
}
