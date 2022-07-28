using JMovies.IMDb.Entities.Interfaces;
using JMovies.IMDb.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;

namespace JMovies.IMDb.Tests.Production
{
    /// <summary>
    /// Test class that contains the tests for basic async scraping scenarios for productions
    /// </summary>
    [TestClass]
    public class ProductionAsyncTests
    {
        /// <summary>
        /// Static list of IMDb IDs of some productions to be tested
        /// </summary>
        private static readonly long[] productionIDsToTest = new long[] { 30988, 3636, 1477834 };

        /// <summary>
        /// Test that checks if the code is running correctly in async mode
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task BasicFetchingTest()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            foreach (long productionID in productionIDsToTest)
            {
                Entities.Movies.Production production = await imdbDataProvider.GetProductionAsync(productionID);
                Assert.IsNotNull(production);
                Assert.IsFalse(string.IsNullOrEmpty(production.Title));
                Assert.AreEqual(productionID, production.IMDbID);
            }
        }

        /// <summary>
        /// Test that checks if the code is running without blocking
        /// </summary>
        [TestMethod]
        public void NonBlockingTest()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            Stopwatch stopwatch = Stopwatch.StartNew();
            Task<Entities.Movies.Production> productionFetcher = imdbDataProvider.GetProductionAsync(productionIDsToTest[0]);
            stopwatch.Stop();
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 1000);
            Assert.IsTrue(!productionFetcher.IsCompleted);
            Entities.Movies.Production production = productionFetcher.Result;
            Assert.IsNotNull(production);
            Assert.IsFalse(string.IsNullOrEmpty(production.Title));
            Assert.AreEqual(productionIDsToTest[0], production.IMDbID);
        }
    }
}
