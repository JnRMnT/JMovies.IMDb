using HtmlAgilityPack;
using JMovies.IMDb.Common.Constants;
using JMovies.IMDb.Entities.Movies;
using JMovies.IMDb.Entities.People;
using JMovies.IMDb.Entities.Interfaces;
using System.IO;
using System.Net;
using Fizzler.Systems.HtmlAgilityPack;
using System.Text.RegularExpressions;
using JMovies.IMDb.Helpers;
using JMovies.IMDb.Helpers.Movies;
using JMovies.IMDb.Helpers.People;
using System.Text;
using JM.Entities.Framework;
using JMovies.IMDb.Entities.Settings;

namespace JMovies.IMDb.Providers
{
    /// <summary>
    /// Data provider that fetches the data real-time on IMDb website using Screen Scraping and returns it as per defined in IIMDbDataProvider.
    /// </summary>
    public class IMDbScraperDataProvider : IIMDbDataProvider
    {
        /// <summary>
        /// Gets Production Information iresspective of type
        /// </summary>
        /// <param name="id">ID of the production</param>
        /// <param name="settings">Object containing Data Fetch settings</param>
        /// <returns>Production instance containing retreived information</returns>
        public Production GetProduction(long id, ProductionDataFetchSettings settings)
        {
            if (id == default(long))
            {
                throw new JMException("IMDbIDEmpty");
            }
            else if (settings == null)
            {
                throw new JMException("SettingsEmpty");
            }

            Movie movie = new Movie();
            string url = IMDbConstants.BaseURL + IMDbConstants.MoviesPath + IMDbConstants.MovieIDPrefix + IMDBIDHelper.GetPaddedIMDBId(id);
            HtmlDocument htmlDocument = HtmlHelper.GetNewHtmlDocument();
            WebRequest webRequest = HttpHelper.InitializeWebRequest(url + "/", settings);
            using (Stream stream = HttpHelper.GetResponseStream(webRequest, settings))
            {
                htmlDocument.Load(stream, Encoding.UTF8);
            }
            HtmlNode documentNode = htmlDocument.DocumentNode;

            //Parse and verify IMDb ID Meta Tag
            string foundID = IMDbConstants.ProductionTitleMatcherRegex.Match(documentNode.InnerHtml)?.Groups?[1]?.Value;
            if (!string.IsNullOrEmpty(foundID))
            {
                movie.IMDbID = foundID.ToLong();
            }
            else
            {
                return null;
            }

            MoviePageDetailsHelper.Parse(this, ref movie, documentNode, url, settings);

            return movie;
        }

        /// <summary>
        /// Gets Person information
        /// </summary>
        /// <param name="id">ID of the Person</param>
        /// <param name="settings">Object containing Data Fetch settings</param>
        /// <returns>Person instance containing retreived information</returns>
        public Person GetPerson(long id, PersonDataFetchSettings settings)
        {
            Person person = new Person();
            return GetPerson(id, person, settings);
        }

        /// <summary>
        /// Gets Person information
        /// </summary>
        /// <param name="id">ID of the Person</param>
        /// <param name="person">Person instance to be populated</param>
        /// <param name="settings">Object containing Data Fetch settings</param>
        /// <returns>Person instance containing retreived information</returns>
        public Person GetPerson(long id, Person person, PersonDataFetchSettings settings)
        {
            if (id == default(long))
            {
                throw new JMException("IMDbIDEmpty");
            }

            string url = IMDbConstants.BaseURL + IMDbConstants.PersonsPath + IMDbConstants.PersonIDPrefix + IMDBIDHelper.GetPaddedIMDBId(id);
            HtmlDocument htmlDocument = HtmlHelper.GetNewHtmlDocument();

            WebRequest webRequest = HttpHelper.InitializeWebRequest(url, settings);
            using (Stream stream = HttpHelper.GetResponseStream(webRequest, settings))
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

            PersonPageHelper.Parse(person, documentNode, settings);

            return person;
        }

        /// <summary>
        /// Gets Movie information
        /// </summary>
        /// <param name="id">ID of the movie</param>
        /// <param name="settings">Object containing Data Fetch settings</param>
        /// <returns>Movie instance containing retreived information</returns>
        public Movie GetMovie(long id, ProductionDataFetchSettings settings)
        {
            return GetProduction(id, settings) as Movie;
        }

        /// <summary>
        /// Gets TV Series information
        /// </summary>
        /// <param name="id">ID of the TV Series</param>
        /// <param name="settings">Object containing Data Fetch settings</param>
        /// <returns>TV Series instance containing retreived information</returns>
        public TVSeries GetTvSeries(long id, ProductionDataFetchSettings settings)
        {
            return GetProduction(id, settings) as TVSeries;
        }

        /// <summary>
        /// Gets Production Information iresspective of type using the default settings
        /// </summary>
        /// <param name="id">ID of the production</param>
        /// <returns>Production instance containing retreived information</returns>
        public Production GetProduction(long id)
        {
            return GetProduction(id, new ProductionDataFetchSettings());
        }

        /// <summary>
        /// Gets Movie information using the default settings
        /// </summary>
        /// <param name="id">ID of the movie</param>
        /// <returns>Movie instance containing retreived information</returns>
        public Movie GetMovie(long id)
        {
            return GetMovie(id, new ProductionDataFetchSettings());
        }

        /// <summary>
        /// Gets TV Series information using the default settings
        /// </summary>
        /// <param name="id">ID of the TV Series</param>
        /// <returns>TV Series instance containing retreived information</returns>
        public TVSeries GetTvSeries(long id)
        {
            return GetTvSeries(id, new ProductionDataFetchSettings());
        }

        /// <summary>
        /// Gets Person information using the default settings
        /// </summary>
        /// <param name="id">ID of the Person</param>
        /// <returns>Person instance containing retreived information</returns>
        public Person GetPerson(long id)
        {
            return GetPerson(id, new PersonDataFetchSettings());
        }
    }
}