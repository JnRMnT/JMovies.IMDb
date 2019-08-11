using System;
using JMovies.IMDb.Entities.Movies;
using JMovies.IMDb.Entities.People;
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
            long[] movieIDs = new long[] { 1477834, 397442, 6412452 };
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            foreach (long movieID in movieIDs)
            {
                Movie movie = imdbDataProvider.GetMovie(movieID, false);
                Assert.IsNotNull(movie);
                Assert.AreEqual(movieID, movie.IMDbID);
            }
        }

        [TestMethod]
        public void DetailedMovieScraping()
        {
            long[] movieIDs = new long[] { 1477834, 397442, 6412452 };
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            foreach (long movieID in movieIDs)
            {
                Movie movie = imdbDataProvider.GetMovie(movieID, true);
                Assert.IsNotNull(movie);
                Assert.AreEqual(movieID, movie.IMDbID);
            }
        }

        [TestMethod]
        public void PersonScraping()
        {
            long[] personIDs = new long[] { 3614913, 5253, 1297015, 3614913, 1877 };
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            foreach (long personID in personIDs)
            {
                Person person = imdbDataProvider.GetPerson(personID, true);
                Assert.IsNotNull(person);
                Assert.AreEqual(personID, person.IMDbID);
            }
        }
    }
}