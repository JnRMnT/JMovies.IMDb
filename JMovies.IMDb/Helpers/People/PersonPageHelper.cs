﻿using Fizzler.Systems.HtmlAgilityPack;
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
            HtmlNode mainDetailsElement = documentNode.QuerySelector(".maindetails_center");
            if (mainDetailsElement != null)
            {
                HtmlNode nameOverviewWidget = mainDetailsElement.QuerySelector(".name-overview-widget");
                if (nameOverviewWidget != null)
                {
                    HtmlNode nameContainer = nameOverviewWidget.QuerySelector("h1.header .itemprop");
                    if (nameContainer != null)
                    {
                        person.FullName = nameContainer.InnerText;
                    }

                    HtmlNode primaryImageElement = nameOverviewWidget.QuerySelector("#img_primary .image a img");
                    if (primaryImageElement != null)
                    {
                        Image image = new Image
                        {
                            Title = primaryImageElement.Attributes["title"].Value.Prepare(),
                            URL = IMDBImageHelper.NormalizeImageUrl(primaryImageElement.Attributes["src"].Value)
                        };
                        if (settings.FetchImageContents)
                        {
                            image.Content = IMDBImageHelper.GetImageContent(image.URL);
                        }
                        person.PrimaryImage = image;
                    }

                    HtmlNode jobCategoriesContainer = nameOverviewWidget.QuerySelector("div#name-job-categories");
                    if (jobCategoriesContainer != null)
                    {
                        List<CreditRoleType> roles = new List<CreditRoleType>();
                        foreach (HtmlNode jobCategoryLink in jobCategoriesContainer.QuerySelectorAll("a"))
                        {
                            CreditRoleType role = CreditRoleType.Undefined;
                            string roleText = jobCategoryLink.InnerText.Prepare();
                            Enum.TryParse(roleText, out role);
                            roles.Add(role);
                        }

                        person.Roles = roles;
                    }
                }
                else
                {
                    HtmlNode nameHeader = documentNode.QuerySelector(".header");
                    if (nameHeader != null)
                    {
                        person.FullName = nameHeader.InnerText.Prepare();
                    }
                }
            }
            else
            {
                //Read from meta 

                //full name og:title
                HtmlNode titleTag = documentNode.QuerySelector("meta[property='og:title']");
                if (titleTag != null)
                {
                    person.FullName = Regex.Replace(titleTag.Attributes["content"].Value, IMDbConstants.PersonTitleSuffix, string.Empty).ToString();
                }

                //image from og:image
                HtmlNode imageTag = documentNode.QuerySelector("meta[property='og:image']");
                if (imageTag != null)
                {
                    Image image = new Image
                    {
                        Title =person.FullName ?? "Profile",
                        URL = IMDBImageHelper.NormalizeImageUrl(imageTag.Attributes["content"].Value)
                    };
                    if (settings.FetchImageContents)
                    {
                        image.Content = IMDBImageHelper.GetImageContent(image.URL);
                    }
                    person.PrimaryImage = image;
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
            HtmlNode filmographyElement = documentNode.QuerySelector("#filmography");
            HtmlNode[] filmogpaphyCategories = documentNode.QuerySelectorAll(".filmo-category-section").ToArray();
            DetectGender(person, filmogpaphyCategories);

            foreach (HtmlNode filmographyCategorySection in filmogpaphyCategories)
            {
                string categoryName = filmographyCategorySection.NodesBeforeSelf().FirstOrDefault(e => e.Name == "div").Attributes["data-category"].Value;
                categoryName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(categoryName.Replace("_", " "));
                string categoryTypeString = categoryName.Replace(" ", string.Empty);
                CreditRoleType creditRoleType = CreditRoleType.Undefined;
                Enum.TryParse(categoryTypeString, out creditRoleType);
            }
            #endregion
            #region Known For Parsing
            HtmlNode knownForElement = documentNode.QuerySelector("#knownfor");
            if (knownForElement != null)
            {
                List<ProductionCredit> knowForCredits = new List<ProductionCredit>();
                foreach (HtmlNode knownForTitleNode in knownForElement.QuerySelectorAll(".knownfor-title"))
                {
                    HtmlNode titleYearElement = knownForTitleNode.QuerySelector(".knownfor-year");
                    Match titleYearMatch = GeneralRegexConstants.PharantesisRegex.Match(titleYearElement.InnerText);
                    int titleYear = default(int);
                    int? titleEndYear = null;
                    if (titleYearMatch.Success)
                    {
                        string titleYearString = titleYearMatch.Groups[1].Value;
                        titleYearMatch = IMDbConstants.CreditYearRegex.Match(titleYearString);
                        if (titleYearMatch.Success)
                        {
                            titleYear = titleYearMatch.Groups[1].Value.ToInteger();
                            if (titleYearMatch.Groups.Count >= 4)
                            {
                                titleEndYear = titleYearMatch.Groups[3].Value.ToInteger();
                            }
                        }
                    }

                    HtmlNode roleElement = knownForTitleNode.QuerySelector(".knownfor-title-role");
                    HtmlNode movieLink = roleElement.Element("a");
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

                    string role = roleElement.Element("span").InnerText.Prepare();
                    CreditRoleType roleType = CreditRoleType.Undefined;
                    if (!Enum.TryParse<CreditRoleType>(role, out roleType))
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
                string categoryName = categoryWrapper.NodesBeforeSelf().FirstOrDefault(e => e.Name == "div").Attributes["data-category"].Value;
                return categoryName == IMDbConstants.ActorCategoryName || categoryName == IMDbConstants.ActressCategoryName;
            });
            if (actingCategory != null)
            {
                string categoryName = actingCategory.NodesBeforeSelf().FirstOrDefault(e => e.Name == "div").Attributes["data-category"].Value;
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
