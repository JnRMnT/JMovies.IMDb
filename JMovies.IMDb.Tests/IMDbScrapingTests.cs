using System;
using JMovies.IMDb.Entities.IMDB;
using JMovies.IMDb.Entities.Interfaces;
using JMovies.IMDb.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JMovies.IMDb.Tests
{
    [TestClass]
    public class IMDbScrapingTests
    {
        [TestMethod]
        public void MovieScraping()
        {
            long movieID = 1477834;//397442;//6412452;
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            Movie movie = imdbDataProvider.GetMovie(movieID, false);
            Assert.IsNotNull(movie);
            Assert.AreEqual(movieID, movie.IMDbID);
        }

        [TestMethod]
        public void DetailedMovieScraping()
        {
            long movieID = 1477834;//1477834;//397442;//6412452;
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            Movie movie = imdbDataProvider.GetMovie(movieID, true);
            Assert.IsNotNull(movie);
            Assert.AreEqual(movieID, movie.IMDbID);
        }

        [TestMethod]
        public void PersonScraping()
        {
            long personID = 1297015;
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            Person person = imdbDataProvider.GetPerson(personID);
            Assert.IsNotNull(person);
            Assert.AreEqual(personID, person.IMDbID);
        }
    }
}