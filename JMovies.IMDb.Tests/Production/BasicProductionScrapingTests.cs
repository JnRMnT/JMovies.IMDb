using JMovies.IMDb.Entities.Interfaces;
using JMovies.IMDb.Entities.Settings.Presets;
using JMovies.IMDb.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JMovies.IMDb.Tests.Production
{
    /// <summary>
    /// Test class that contains the tests for basic scraping scenarios for productions
    /// </summary>
    [TestClass]
    public class BasicProductionScrapingTests
    {
        /// <summary>
        /// Static list of IMDb IDs of some productions to be tested
        /// </summary>
        private static readonly long[] productionIDsTotest = new long[] { 30988, 3636, 1477834, 173, 212, 397442, 6412452, 0944947, 2139881 };


        /// <summary>
        /// Method that tests scraping of different Production Details for basic details
        /// </summary>
        [TestMethod]
        public void ProductionScraping()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            foreach (long productionID in productionIDsTotest)
            {
                Entities.Movies.Production production = imdbDataProvider.GetProduction(productionID);
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
                Entities.Movies.Production production = imdbDataProvider.GetProduction(productionID, new FullProductionDataFetchSettings());
                Assert.IsNotNull(production);
                Assert.AreEqual(productionID, production.IMDbID);
            }
        }

    }
}
