using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using JMovies.IMDb.Entities.Movies;
using JMovies.IMDb.Entities.People;
using System.Collections.Generic;
using System;
using System.Linq;
using JMovies.IMDb.Entities.Common;
using System.Globalization;
using JMovies.IMDb.Common.Constants;
using JMovies.IMDb.Factories;
using System.Text.RegularExpressions;

namespace JMovies.IMDb.Helpers.People
{
    public class PersonPageHelper
    {
        public static void Parse(Person person, HtmlNode documentNode, bool fetchAdditionalDetails)
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
                        person.PrimaryImage = IMDBImageHelper.NormalizeImageUrl(primaryImageElement.Attributes["src"].Value);
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

                        person.Roles = roles.ToArray();
                    }

                    List<Image> photos = new List<Image>();
                    HtmlNode mediaStripContainer = nameOverviewWidget.QuerySelector(".mediastrip_container");
                    if (mediaStripContainer != null)
                    {
                        foreach (HtmlNode imageLink in mediaStripContainer.QuerySelectorAll(".mediastrip a"))
                        {
                            HtmlNode imageNode = imageLink.Element("img");
                            photos.Add(new Image
                            {
                                Title = imageNode.Attributes["title"].Value.Prepare(),
                                URL = IMDBImageHelper.NormalizeImageUrl(imageNode.Attributes["loadlate"].Value)
                            });
                        }
                    }
                    person.Photos = photos.ToArray();
                }

            }
            #endregion
            #region Bio Page Parsing
            if (fetchAdditionalDetails)
            {
                BioPageHelper.ParseBioPage(person);
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
                person.KnownFor = knowForCredits.ToArray();
            }
            #endregion
        }

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
