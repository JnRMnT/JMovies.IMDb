using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using JMovies.IMDb.Common.Constants;
using JMovies.IMDb.Entities.Common;
using JMovies.IMDb.Entities.People;
using JMovies.IMDb.Entities.Settings;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace JMovies.IMDb.Helpers.People
{
    /// <summary>
    /// Class responsible for parsing the Bio Page
    /// </summary>
    public static class BioPageHelper
    {
        /// <summary>
        /// Method responsible for parsing the Bio Page
        /// </summary>
        /// <param name="person">Person to be populated</param>
        /// <param name="settings">Object containing Data Fetch settings</param>
        public static void ParseBioPage(Person person, PersonDataFetchSettings settings)
        {
            string url = IMDbConstants.BaseURL + IMDbConstants.PersonsPath + IMDbConstants.PersonIDPrefix + IMDBIDHelper.GetPaddedIMDBId(person.IMDbID) + "/" + IMDbConstants.BioPagePath;
            HtmlDocument htmlDocument = HtmlHelper.GetNewHtmlDocument();
            WebRequest webRequest = HttpHelper.InitializeWebRequest(url, settings);
            using (Stream stream = HttpHelper.GetResponseStream(webRequest, settings))
            {
                htmlDocument.Load(stream, Encoding.UTF8);
            }
            HtmlNode documentNode = htmlDocument.DocumentNode;

            HtmlNode overviewTable = documentNode.QuerySelector("#overviewTable");
            if (overviewTable != null)
            {
                HtmlNode[] overviewRows = overviewTable.QuerySelectorAll("tr").ToArray();
                foreach (HtmlNode overviewRow in overviewRows)
                {
                    HtmlNode titleNode = overviewRow.QuerySelector("td");
                    string overviewTitle = titleNode.InnerText.Prepare();
                    switch (overviewTitle)
                    {
                        case IMDbConstants.BioOverviewBirthInfo:
                            //Get Birth date and place
                            HtmlNode birthDateColumn = overviewRow.QuerySelectorAll("td").Skip(1).Take(1).First();
                            string birthDateString = birthDateColumn.QuerySelector("time").Attributes["datetime"].Value;
                            DateTime birthDate;
                            if (DateTime.TryParse(birthDateString, out birthDate))
                            {
                                person.BirthDate = birthDate;
                            }
                            person.BirthPlace = birthDateColumn.QuerySelectorAll("a").Last().InnerText.Prepare();
                            break;
                        case IMDbConstants.BioOverviewBirthName:
                            //Get Birth Name
                            person.BirthName = overviewRow.QuerySelectorAll("td").Skip(1).Take(1).First().InnerText.Prepare();
                            break;
                        case IMDbConstants.BioOverviewHeight:
                            //Get Height
                            Match heightMatch = IMDbConstants.BioHeightRegex.Match(overviewRow.QuerySelectorAll("td").Skip(1).Take(1).First().InnerText.Prepare());
                            if (heightMatch.Success)
                            {
                                string heightString = heightMatch.Groups[1].Value.Replace(".", ",");
                                double metricHeight;
                                if (double.TryParse(heightString, out metricHeight))
                                {
                                    person.Height = new Length((int)(metricHeight * 100));
                                }
                            }
                            break;
                        case IMDbConstants.BioOverviewNickName:
                            //Get NickName
                            person.NickName = overviewRow.QuerySelectorAll("td").Skip(1).Take(1).First().InnerText.Prepare();
                            break;
                    }
                }
            }

            HtmlNode miniBioLink = documentNode.QuerySelector("a[name='mini_bio']");
            if (miniBioLink != null)
            {
                HtmlNode miniBioDiv = miniBioLink.NodesAfterSelf().FirstOrDefault(e => e.Name == "div");
                HtmlNode miniBioElement = miniBioDiv.Element("p");
                person.MiniBiography = miniBioElement.InnerText.Prepare();
            }
        }
    }
}
