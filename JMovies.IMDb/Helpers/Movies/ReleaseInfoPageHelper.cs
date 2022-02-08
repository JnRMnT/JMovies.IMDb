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
            List<ReleaseDate> releaseDates = new List<ReleaseDate>();
            HtmlNode releaseDatesNode = releaseInfoPageDocument.DocumentNode.QuerySelector("#releases");
            if (releaseDatesNode != null)
            {
                HtmlNode releaseDatesTableNode = releaseDatesNode.NodesAfterSelf().FirstOrDefault(e => e.Name == "table");
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
            movie.ReleaseDates = releaseDates;
        }
    }
}
