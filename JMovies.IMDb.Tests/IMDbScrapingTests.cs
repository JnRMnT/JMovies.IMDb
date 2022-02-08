using JMovies.IMDb.Entities.Interfaces;
using JMovies.IMDb.Entities.Movies;
using JMovies.IMDb.Entities.People;
using JMovies.IMDb.Entities.Settings.Presets;
using JMovies.IMDb.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace JMovies.IMDb.Tests
{
    /// <summary>
    /// Test class that provides different test scenarios of IMDb Screen Scraping
    /// </summary>
    [TestClass]
    public class IMDbScrapingTests
    {
        /// <summary>
        /// Static list of IMDb IDs of some productions to be tested
        /// </summary>
        private static readonly long[] productionIDsTotest = new long[] { 30988, 3636, 1477834, 173, 212, 397442, 6412452, 0944947, 2139881 };

        /// <summary>
        /// Static list of IMDb IDs of some persons to be tested
        /// </summary>
        private static readonly long[] personIDsToTest = new long[] { 18652, 3614913, 5253, 1297015, 3614913, 1877 };

        /// <summary>
        /// Method that tests scraping of different Production Details for basic details
        /// </summary>
        [TestMethod]
        public void ProductionScraping()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            foreach (long productionID in productionIDsTotest)
            {
                Production production = imdbDataProvider.GetProduction(productionID);
                Assert.IsNotNull(production);
                Assert.AreEqual(productionID, production.IMDbID);
            }
        }

        /// <summary>
        /// Method that tests scraping of different Production Details for extended details
        /// </summary>
        [TestMethod]
        public void DetailedProductionScraping()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            foreach (long productionID in productionIDsTotest)
            {
                Production production = imdbDataProvider.GetProduction(productionID, new FullProductionDataFetchSettings());
                Assert.IsNotNull(production);
                Assert.AreEqual(productionID, production.IMDbID);
            }
        }

        /// <summary>
        /// Method that tests scraping of person pages
        /// </summary>
        [TestMethod]
        public void PersonScraping()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            foreach (long personID in personIDsToTest)
            {
                Person person = imdbDataProvider.GetPerson(personID, new BasicPersonDataFetchSettings());
                Assert.IsNotNull(person);
                Assert.IsFalse(string.IsNullOrEmpty(person.FullName));
                Assert.AreEqual(personID, person.IMDbID);
            }
        }


        /// <summary>
        /// Method that tests detailed scraping of person pages
        /// </summary>
        [TestMethod]
        public void DetailedPersonScraping()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            foreach (long personID in personIDsToTest)
            {
                Person person = imdbDataProvider.GetPerson(personID, new FullPersonDataFetchSettings());
                Assert.IsNotNull(person);
                Assert.IsFalse(string.IsNullOrEmpty(person.FullName));
                Assert.AreEqual(personID, person.IMDbID);
            }
        }

        /// <summary>
        /// Test Method that tests a production with start date but no end date
        /// </summary>
        [TestMethod]
        public void TVShowWithNoEndTest()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            Production production = imdbDataProvider.GetProduction(48861, new FullProductionDataFetchSettings());
            Assert.IsNotNull(production);
            Assert.AreEqual(48861, production.IMDbID);
            Assert.IsTrue(production.ProductionType == ProductionTypeEnum.TVSeries);
            Assert.IsTrue((production as TVSeries).Year != default(int));
            Assert.IsFalse((production as TVSeries).EndYear.HasValue);
        }


        /// <summary>
        /// Test Method that tests a TV Show with the summary fetch settings
        /// </summary>
        [TestMethod]
        public void SummaryTVShowTest()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            Production production = imdbDataProvider.GetProduction(397442);
            Assert.IsNotNull(production);
            Assert.AreEqual(397442, production.IMDbID);
            Assert.IsTrue(production.ProductionType == ProductionTypeEnum.TVSeries);
        }

        /// <summary>
        /// Test Method that tests a movie: "The Power of the Dog"
        /// </summary>
        [TestMethod]
        public void TestMovieThePowerOfTheDog()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            Movie movie = imdbDataProvider.GetMovie(10293406); // https://www.imdb.com/title/tt10293406/
            Assert.IsNotNull(movie);
            Assert.AreEqual("The Power of the Dog", movie.Title);
            Assert.AreEqual(2021, movie.Year);
            Assert.AreEqual(6.9, Math.Round(movie.Rating.Value, 1));
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
            Movie movie = imdbDataProvider.GetMovie(1677720); // https://www.imdb.com/title/tt1677720/
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
            Movie movie = imdbDataProvider.GetMovie(12441478); // https://www.imdb.com/title/tt12441478/
            Assert.IsNotNull(movie);
            Assert.AreEqual("Earwig and the Witch", movie.Title);
            Assert.AreEqual(2020, movie.Year);
            Assert.AreEqual(4.8, Math.Round(movie.Rating.Value, 1));
            long RateCount = movie.Rating.RateCount;
            Assert.AreEqual(true, (RateCount > 3200));
        }
    }
}