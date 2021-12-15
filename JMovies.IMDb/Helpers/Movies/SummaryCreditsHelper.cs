using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using JMovies.IMDb.Common.Constants;
using JMovies.IMDb.Entities.Movies;
using JMovies.IMDb.Entities.People;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace JMovies.IMDb.Helpers.Movies
{
    /// <summary>
    /// Helper class responsible for parsing the summary credits
    /// </summary>
    public class SummaryCreditsHelper
    {
        /// <summary>
        /// Method responsible for parsing the cast list of the movie
        /// </summary>
        /// <param name="movie">Movie instance to be populated</param>
        /// <param name="credits">Credits list to be filled</param>
        /// <param name="castListNode">Html node that holds the cast list</param>
        public static void ParseCastList(Movie movie, List<Credit> credits, HtmlNode castListNode)
        {
            if(castListNode != null)
            {
                foreach(HtmlNode castItem in castListNode.QuerySelectorAll("[data-testid=title-cast-item]"))
                {
                    HtmlNode personNode = castItem.QuerySelector("[data-testid=title-cast-item__actor]");
                    HtmlNode charactersNode = castItem.QuerySelector("[data-testid=cast-item-characters-list]");
                    ActingCredit actingCredit = new ActingCredit();
                    actingCredit.Person = new Actor();
                    Match personIDMatch = IMDbConstants.PersonIDURLMatcher.Match(personNode.Attributes["href"].Value);
                    if (personIDMatch.Success && personIDMatch.Groups.Count > 1)
                    {
                        actingCredit.Person.IMDbID = personIDMatch.Groups[1].Value.ToLong();
                        actingCredit.Person.FullName = personNode.InnerText.Prepare();
                    }

                    List<Character> characters = new List<Character>();
                    if (charactersNode != null)
                    {
                        foreach (HtmlNode characterNode in charactersNode.QuerySelectorAll("a"))
                        {
                            Character character = GetCharacter(characterNode, movie);
                            if (character != null)
                            {
                                characters.Add(character);
                            }
                        }
                        if (characters.Count == 0)
                        {
                            Character character = GetCharacter(charactersNode.FirstChild, movie);
                            if (character != null && (!string.IsNullOrEmpty(character.Name) || character.IMDbID != null))
                            {
                                characters.Add(character);
                            }
                        }
                    }
                    actingCredit.Characters = characters;
                    credits.Add(actingCredit);
                }
                movie.Credits = credits;
            }
        }


        /// <summary>
        /// Method responsible for parsing a single character information
        /// </summary>
        /// <param name="characterNode">HTML Node that holds the character information</param>
        /// <param name="movie">Movie instance that is populated</param>
        /// <returns>Parsed character instance</returns>
        private static Character GetCharacter(HtmlNode characterNode, Movie movie)
        {
            if (!characterNode.GetAttributes().Any(e => e.Name == "data-testid" && e.Value == "title-cast-item__eps-toggle"))
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

                HtmlNode episodeInformationNode = characterNode.ParentNode.ParentNode.ParentNode.ParentNode.QuerySelector("[data-testid=title-cast-item__eps-toggle]");
                if (episodeInformationNode == null)
                {
                    character.Name = Regex.Replace(Regex.Replace(characterNode.FirstChild.InnerText.Prepare(), @"\n", string.Empty), @"\s+", @" ").Trim();
                }
                else
                {
                    character.Name = Regex.Replace(Regex.Replace(characterNode.FirstChild.InnerText.Prepare(), @"\n", string.Empty), @"\s+", @" ").Trim();
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
    }
}
