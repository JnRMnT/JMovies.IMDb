using System;
using JMovies.IMDb.Entities.Movies;
using JMovies.IMDb.Entities.People;
using JMovies.IMDb.Entities.Interfaces;
using JMovies.IMDb.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JMovies.IMDb.Entities.Settings;
using JMovies.IMDb.Entities.Settings.Presets;

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
        private static readonly long[] productionIDsTotest = new long[] { 1477834, 173, 212, 397442, 6412452, 0944947, 2139881 };

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
    }
}