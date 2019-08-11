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
    public class IMDbScraperDataProvider : IIMDbDataProvider
    {
        /// <summary>
        /// Gets Production Details based on ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fetchDetailedCast"></param>
        /// <returns></returns>
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

        public Person GetPerson(long id, bool fetchAdditionalDetails)
        {
            Person person = new Person();
            return GetPerson(id, person, fetchAdditionalDetails);
        }

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

        public Movie GetMovie(long id, bool fetchDetailedCast)
        {
            return GetProduction(id, fetchDetailedCast) as Movie;
        }

        public TVSeries GetTvSeries(long id, bool fetchDetailedCast)
        {
            return GetProduction(id, fetchDetailedCast) as TVSeries;
        }
    }
}