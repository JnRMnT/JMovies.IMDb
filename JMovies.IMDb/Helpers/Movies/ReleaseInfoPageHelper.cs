using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using JMovies.IMDb.Common.Constants;
using JMovies.IMDb.Entities.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace JMovies.IMDb.Helpers.Movies
{
    /// <summary>
    /// Helper Class responsible for parsing the Release Info Page
    /// </summary>
    public static class ReleaseInfoPageHelper
    {
        /// <summary>
        /// Parses the Release Info Page and persists the data on Movie instance
        /// </summary>
        /// <param name="movie">Movie object to be populated</param>
        /// <param name="releaseInfoPageDocument">HTML Document of the Release Info Page</param>
        public static void Parse(Movie movie, HtmlDocument releaseInfoPageDocument)
        {
            #region Release Dates
            List<ReleaseDate> releaseDates = new List<ReleaseDate>();
            HtmlNode releaseDatesNode = releaseInfoPageDocument.DocumentNode.QuerySelector("#releases");


#pragma warning disable CS1570 // XML comment has badly formed XML

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
            ///I just fixed null error
            ///New way of getting akas and release dates (fully) looks hard.I guess these services should use but i dont have much experience about that
            ///https://www.imdb.com/tr/?pt=title&spt=releaseinfo&const=tt0038969&ht=actionOnly&pageAction=ttrel-releases-seemore&rrid=TRGZ7Y3XGZNFZ6K585SB
            ///https://caching.graphql.imdb.com/?operationName=TitleReleaseDatesPaginated&variables=%7B%22after%22%3A%22NQ%3D%3D%22%2C%22const%22%3A%22tt0038969%22%2C%22first%22%3A50%2C%22locale%22%3A%22en-US%22%2C%22originalTitleText%22%3Afalse%7D&extensions=%7B%22persistedQuery%22%3A%7B%22sha256Hash%22%3A%225e101e432b7a0d78da43a53e6be2b7cc5309f34929fb67b7762a001eace8edb1%22%2C%22version%22%3A1%7D%7D
            ///
#pragma warning restore CS1570 // XML comment has badly formed XML
#pragma warning restore CS1587 // XML comment is not placed on a valid language element

            if (releaseDatesNode != null)
            {
                HtmlNode releaseDatesTableNode = releaseDatesNode.NodesAfterSelf().FirstOrDefault(e => e.Name == "table");
                if (releaseDatesTableNode != null)
                {
                    foreach (HtmlNode releaseDateRow in releaseDatesTableNode.QuerySelectorAll("tr"))
                    {
                        ReleaseDate releaseDate = new ReleaseDate();
                        releaseDate.Country = new Country();
                        HtmlNode[] releaseDateColumns = releaseDateRow.QuerySelectorAll("td").ToArray();
                        HtmlNode releaseDateCountryLink = releaseDateColumns[0].QuerySelector("a");
                        Match countryMatch = IMDbConstants.ReleaseDateCountryIdentifierRegex.Match(releaseDateCountryLink.GetAttributeValue("href", string.Empty));
                        releaseDate.Country.Name = releaseDateCountryLink.InnerText.Prepare();
                        if (countryMatch.Success)
                        {
                            releaseDate.Country.Identifier = countryMatch.Groups[1].Value;
                        }

                        string releaseDateString = releaseDateColumns[1].InnerText.Prepare();
                        Match allNumericReleaseDateMatch = GeneralRegexConstants.AllNumericRegex.Match(releaseDateString);
                        if (allNumericReleaseDateMatch.Success)
                        {
                            releaseDate.Date = new DateTime(allNumericReleaseDateMatch.Groups[1].Value.ToInteger(), 1, 1);
                        }
                        else
                        {
                            releaseDate.Date = DateTime.Parse(releaseDateString);
                        }


                        if (releaseDateColumns.Length > 2)
                        {
                            releaseDate.Description = releaseDateColumns[2].InnerText.Prepare();
                            if (releaseDate.Description.Count(e => e == '(') == 1)
                            {
                                releaseDate.Description = releaseDate.Description.TrimStart('(').TrimEnd(')');
                            }
                        }
                        releaseDates.Add(releaseDate);
                    }
                }
            }


            movie.ReleaseDates = releaseDates;
            #endregion
            #region AKAs
            HtmlNode akasNode = releaseInfoPageDocument.DocumentNode.QuerySelector("#akas");
            if (akasNode != null)
            {
                List<AKA> akas = new List<AKA>();
                HtmlNode akasTableNode = akasNode.NodesAfterSelf().FirstOrDefault(e => e.Name == "table");
                if (akasTableNode != null)
                {
                    foreach (HtmlNode akaDateRow in akasTableNode.QuerySelectorAll("tr"))
                    {
                        AKA aka = new AKA();
                        HtmlNode[] akaColumns = akaDateRow.QuerySelectorAll("td").ToArray();
                        aka.Description = akaColumns[0].InnerText.Prepare();
                        aka.Name = akaColumns[1].InnerText.Prepare();
                        akas.Add(aka);
                    }
                }

                movie.AKAs = akas.ToArray();
            }
            #endregion
        }
    }
}
