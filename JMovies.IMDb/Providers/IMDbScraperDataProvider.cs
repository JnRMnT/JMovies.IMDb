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
        /// <param name="fetchDetailedCast">Should the detailed cast info be fetched? This effects the response time.</param>
        /// <returns>Production instance containing retreived information</returns>
        public Production GetProduction(long id, bool fetchDetailedCast)
        {
            if (id == default(long))
            {
                throw new JMException("IMDbIDEmpty");
            }

            Movie movie = new Movie();
            string url = IMDbConstants.BaseURL + IMDbConstants.MoviesPath + IMDbConstants.MovieIDPrefix + id.ToString().PadLeft(IMDbConstants.IMDbIDLength, '0');
            HtmlDocument htmlDocument = HtmlHelper.GetNewHtmlDocument();
            WebRequest webRequest = HttpHelper.InitializeWebRequest(url);
            using (Stream stream = HttpHelper.GetResponseStream(webRequest))
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

            MoviePageDetailsHelper.Parse(this, movie, documentNode, url, fetchDetailedCast);

            return movie;
        }

        /// <summary>
        /// Gets Person information
        /// </summary>
        /// <param name="id">ID of the Person</param>
        /// <param name="fetchAdditionalDetails">Should the additional details be also fetched? This effects the response time.</param>
        /// <returns>Person instance containing retreived information</returns>
        public Person GetPerson(long id, bool fetchAdditionalDetails)
        {
            Person person = new Person();
            return GetPerson(id, person, fetchAdditionalDetails);
        }

        /// <summary>
        /// Gets Person information
        /// </summary>
        /// <param name="id">ID of the Person</param>
        /// <param name="person">Person instance to be populated</param>
        /// <param name="fetchAdditionalDetails">Should the additional details be also fetched? This effects the response time.</param>
        /// <returns>Person instance containing retreived information</returns>
        public Person GetPerson(long id, Person person, bool fetchAdditionalDetails)
        {
            if (id == default(long))
            {
                throw new JMException("IMDbIDEmpty");
            }

            string url = IMDbConstants.BaseURL + IMDbConstants.PersonsPath + IMDbConstants.PersonIDPrefix + id.ToString().PadLeft(IMDbConstants.IMDbIDLength, '0');
            HtmlDocument htmlDocument = HtmlHelper.GetNewHtmlDocument();

            WebRequest webRequest = HttpHelper.InitializeWebRequest(url);
            using (Stream stream = HttpHelper.GetResponseStream(webRequest))
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

            PersonPageHelper.Parse(person, documentNode, fetchAdditionalDetails);

            return person;
        }

        /// <summary>
        /// Gets Movie information
        /// </summary>
        /// <param name="id">ID of the movie</param>
        /// <param name="fetchDetailedCast">Should the detailed cast info be fetched? This effects the response time.</param>
        /// <returns>Movie instance containing retreived information</returns>
        public Movie GetMovie(long id, bool fetchDetailedCast)
        {
            return GetProduction(id, fetchDetailedCast) as Movie;
        }

        /// <summary>
        /// Gets TV Series information
        /// </summary>
        /// <param name="id">ID of the TV Series</param>
        /// <param name="fetchDetailedCast">Should the detailed cast info be fetched? This effects the response time.</param>
        /// <returns>TV Series instance containing retreived information</returns>
        public TVSeries GetTvSeries(long id, bool fetchDetailedCast)
        {
            return GetProduction(id, fetchDetailedCast) as TVSeries;
        }
    }
}