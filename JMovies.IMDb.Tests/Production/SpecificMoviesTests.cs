using JMovies.IMDb.Entities.Interfaces;
using JMovies.IMDb.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace JMovies.IMDb.Tests.Production
{
    /// <summary>
    /// Test class that contains the tests which are targeted for specific movies
    /// </summary>
    [TestClass]
    public class SpecificMoviesTests
    {

        /// <summary>
        /// Test Method that tests a movie: "The Power of the Dog"
        /// </summary>
        [TestMethod]
        public void TestMovieThePowerOfTheDog()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            Entities.Movies.Movie movie = imdbDataProvider.GetMovie(10293406); // https://www.imdb.com/title/tt10293406/
            Assert.IsNotNull(movie);
            Assert.AreEqual("The Power of the Dog", movie.Title);
            Assert.AreEqual(2021, movie.Year);
            Assert.AreEqual(6.8, Math.Round(movie.Rating.Value, 1));
            long RateCount = movie.Rating.RateCount;
            Assert.AreEqual(true, (RateCount > 30000));
        }

        /// <summary>
        /// Test Method that tests a movie: "Ready Player One"
        /// </summary>
        [TestMethod]
        public void TestMovieReadyPlayerOne()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            Entities.Movies.Movie movie = imdbDataProvider.GetMovie(1677720); // https://www.imdb.com/title/tt1677720/
            Assert.IsNotNull(movie);
            Assert.AreEqual("Ready Player One", movie.Title);
            Assert.AreEqual(2018, movie.Year);
            Assert.AreEqual(7.4, Math.Round(movie.Rating.Value, 1));
            long RateCount = movie.Rating.RateCount;
            Assert.AreEqual(true, (RateCount > 400000));
        }

        /// <summary>
        /// Test Method that tests a movie: "Earwig and the Witch"
        /// </summary>
        [TestMethod]
        public void TestMovieEarwigAndTheWitch()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            Entities.Movies.Movie movie = imdbDataProvider.GetMovie(12441478); // https://www.imdb.com/title/tt12441478/
            Assert.IsNotNull(movie);
            Assert.AreEqual("Earwig and the Witch", movie.Title);
            Assert.AreEqual(2020, movie.Year);
            Assert.AreEqual(4.7, Math.Round(movie.Rating.Value, 1));
            long RateCount = movie.Rating.RateCount;
            Assert.AreEqual(true, (RateCount > 3200));
        }
    }
}
