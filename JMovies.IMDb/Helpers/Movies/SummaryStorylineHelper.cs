using HtmlAgilityPack;
using JMovies.IMDb.Entities.Movies;
using JMovies.IMDb.Entities.People;
using Fizzler.Systems.HtmlAgilityPack;
using JMovies.IMDb.Common.Constants;
using System.Text.RegularExpressions;
using System.Collections.Generic;

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
            HtmlNode storyLine = storyLineSection.QuerySelector("p span");
            if (storyLine != null)
            {
                movie.StoryLine = storyLine.InnerText.Prepare();
            }

            //Parse Taglines
            foreach (HtmlNode storyLineSectionHeader in storyLineSection.QuerySelectorAll("h4"))
            {
                string headerContent = storyLineSectionHeader.InnerText.Prepare();
                if (IMDbConstants.TaglinesSummaryRegex.IsMatch(headerContent))
                {
                    movie.TagLines = new string[] { storyLineSectionHeader.NextSibling.InnerText.Prepare() };
                }
                else if (IMDbConstants.PlotKeywordsSummaryRegex.IsMatch(headerContent))
                {
                    List<Keyword> keywords = new List<Keyword>();
                    foreach (HtmlNode keywordLink in storyLineSectionHeader.ParentNode.QuerySelectorAll("a"))
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
                else if (IMDbConstants.GenresSummaryRegex.IsMatch(headerContent))
                {
                    List<Genre> genres = new List<Genre>();
                    foreach (HtmlNode genreLink in storyLineSectionHeader.ParentNode.QuerySelectorAll("a"))
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
    }
}
