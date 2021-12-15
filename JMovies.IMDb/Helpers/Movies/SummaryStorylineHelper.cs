using HtmlAgilityPack;
using JMovies.IMDb.Entities.Movies;
using JMovies.IMDb.Entities.People;
using Fizzler.Systems.HtmlAgilityPack;
using JMovies.IMDb.Common.Constants;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace JMovies.IMDb.Helpers.Movies
{
    /// <summary>
    /// Class responsible for parsing the summary-story line section 
    /// </summary>
    public class SummaryStorylineHelper
    {
        /// <summary>
        /// Method responsible for parsing the summary-story line section  
        /// </summary>
        /// <param name="movie">Movie instance to be populated</param>
        /// <param name="storyLineSection">HTML Node containing Story Line section</param>
        public static void Parse(Movie movie, HtmlNode storyLineSection)
        {
            HtmlNode storyLine = storyLineSection.QuerySelector("[data-testid=storyline-plot-summary]");
            if (storyLine != null)
            {
                movie.StoryLine = storyLine.InnerText.Prepare();

                //Parse Story Line related info
                foreach (HtmlNode storyLineSectionHeader in storyLine.ParentNode.ChildNodes.Where(e => e.Name == "ul").LastOrDefault()?.ChildNodes.Where(e => e.Name == "li"))
                {
                    string headerContent = storyLineSectionHeader.FirstChild.InnerText.Prepare();
                    if (IMDbConstants.TaglinesSummaryRegex.IsMatch(headerContent))
                    {
                        movie.TagLines = new List<TagLine>();
                        for (int i = 1; i < storyLineSectionHeader.ChildNodes.Count; i++)
                        {
                            movie.TagLines.Add(new TagLine { Content = storyLineSectionHeader.ChildNodes[i].InnerText.Prepare() });
                        }
                    }
                    else if (IMDbConstants.GenresSummaryRegex.IsMatch(headerContent))
                    {
                        List<Genre> genres = new List<Genre>();
                        foreach (HtmlNode genreLink in storyLineSectionHeader.QuerySelectorAll("a"))
                        {
                            Match genreMatch = IMDbConstants.GenreLinkRegex.Match(genreLink.OuterHtml);
                            if (genreMatch.Success)
                            {
                                genres.Add(new Genre
                                {
                                    Identifier = genreMatch.Groups[1].Value,
                                    Value = genreLink.InnerText.Prepare()
                                });
                            }
                        }
                        movie.Genres = genres;
                    }
                }
            }

            HtmlNode keywordsNode = storyLineSection.QuerySelector("[data-testid=storyline-plot-keywords]");
            if (keywordsNode != null)
            {
                List<Keyword> keywords = new List<Keyword>();
                foreach (HtmlNode keywordLink in keywordsNode.QuerySelectorAll("a"))
                {
                    Match keywordMatch = IMDbConstants.KeywordLinkRegex.Match(keywordLink.OuterHtml);
                    if (keywordMatch.Success)
                    {
                        keywords.Add(new Keyword
                        {
                            Identifier = keywordMatch.Groups[1].Value,
                            Value = keywordLink.InnerText.Prepare()
                        });
                    }
                }
                movie.Keywords = keywords;
            }
        }
    }
}
