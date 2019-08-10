using HtmlAgilityPack;
using JMovies.IMDb.Common.Constants;
using JMovies.IMDb.Entities.Movies;
using JMovies.IMDb.Entities.People;
using JMovies.IMDb.Entities.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Fizzler.Systems.HtmlAgilityPack;
using System.Text.RegularExpressions;
using JMovies.IMDb.Helpers;
using System.Text;
using System.Threading.Tasks;
using JMovies.Entities.Framework;

namespace JMovies.IMDb.Providers
{
    public class IMDbScraperDataProvider : IIMDbDataProvider
    {
        public Movie GetMovie(long id, bool fetchDetailedCast)
        {
            if (id == default(long))
            {
                throw new JMException("IMDbIDEmpty");
            }

            Movie movie = new Movie();
            string url = IMDbConstants.BaseURL + IMDbConstants.MoviesPath + IMDbConstants.MovieIDPrefix + id.ToString().PadLeft(IMDbConstants.IMDbIDLength, '0');
            HtmlDocument htmlDocument = GetNewHtmlDocument();
            WebRequest webRequest = HttpHelper.InitializeWebRequest(url);
            using (Stream stream = webRequest.GetResponse().GetResponseStream())
            {
                htmlDocument.Load(stream, Encoding.UTF8);
            }
            HtmlNode documentNode = htmlDocument.DocumentNode;

            //Parse and verify IMDb ID Meta Tag
            HtmlNode idMetaTag = documentNode.QuerySelector("meta[property='pageId']");
            if (idMetaTag != null)
            {
                movie.IMDbID = Regex.Replace(idMetaTag.Attributes["content"].Value, IMDbConstants.MovieIDPrefix, string.Empty).ToLong();
            }
            else
            {
                return null;
            }
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
                return null;
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
                return null;
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
                MoviePageDetailsHelper.Parse(movie, detailsSection);
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
                string fullCreditsUrl = url + "/" + IMDbConstants.FullCreditsPath;
                WebRequest fullCreditsPageRequest = HttpHelper.InitializeWebRequest(fullCreditsUrl);
                HtmlDocument creditsPageDocument = GetNewHtmlDocument();
                using (Stream stream = fullCreditsPageRequest.GetResponse().GetResponseStream())
                {
                    creditsPageDocument.Load(stream, Encoding.UTF8);
                }
                HtmlNode fullCreditsPageDocumentNode = creditsPageDocument.DocumentNode;
                HtmlNode fullCreditsPageCastListNode = fullCreditsPageDocumentNode.QuerySelector(".cast_list");
                ParseCastList(movie, credits, fullCreditsPageCastListNode);
                movie.Credits = credits.ToArray();
            }


            Parallel.ForEach(movie.Credits, (Credit credit) =>
            {
                credit.Person = GetPerson(credit.Person.IMDbID, credit.Person);
            });

            return movie;
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

        private static HtmlDocument GetNewHtmlDocument()
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            HtmlNode.ElementsFlags["br"] = HtmlElementFlag.Empty;
            htmlDocument.OptionWriteEmptyNodes = true;
            return htmlDocument;
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

        public Person GetPerson(long id)
        {
            Person person = new Person();
            return GetPerson(id, person);
        }

        public Person GetPerson(long id, Person person)
        {
            if (id == default(long))
            {
                throw new JMException("IMDbIDEmpty");
            }

            string url = IMDbConstants.BaseURL + IMDbConstants.PersonsPath + IMDbConstants.PersonIDPrefix + id.ToString().PadLeft(IMDbConstants.IMDbIDLength, '0');
            HtmlDocument htmlDocument = GetNewHtmlDocument();

            WebRequest webRequest = HttpHelper.InitializeWebRequest(url);
            using (Stream stream = webRequest.GetResponse().GetResponseStream())
            {
                htmlDocument.Load(stream, Encoding.UTF8);
            }
            HtmlNode documentNode = htmlDocument.DocumentNode;

            //Parse and verify IMDb ID Meta Tag
            HtmlNode idMetaTag = documentNode.QuerySelector("meta[property='pageId']");
            if (idMetaTag != null)
            {
                person.IMDbID = Regex.Replace(idMetaTag.Attributes["content"].Value, IMDbConstants.PersonIDPrefix, string.Empty).ToLong();
            }
            else
            {
                return null;
            }

            HtmlNode mainDetailsElement = documentNode.QuerySelector(".maindetails_center");
            if (mainDetailsElement != null)
            {
                HtmlNode nameOverviewWidget = mainDetailsElement.QuerySelector(".name-overview-widget");
                if (nameOverviewWidget != null)
                {
                    HtmlNode nameContainer = mainDetailsElement.QuerySelector("h1.header .itemprop");
                    if (nameContainer != null)
                    {
                        person.FullName = nameContainer.InnerText;
                    }

                    HtmlNode primaryImageElement = mainDetailsElement.QuerySelector("#img_primary .image a img");
                    if (primaryImageElement != null)
                    {
                        person.PrimaryImage = IMDBImageHelper.NormalizeImageUrl(primaryImageElement.Attributes["src"].Value);
                    }
                }
            }

            return person;
        }
    }
}