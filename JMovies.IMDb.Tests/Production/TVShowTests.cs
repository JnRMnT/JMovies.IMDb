using JMovies.IMDb.Entities.Interfaces;
using JMovies.IMDb.Entities.Movies;
using JMovies.IMDb.Entities.Settings.Presets;
using JMovies.IMDb.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JMovies.IMDb.Tests.Production
{
    /// <summary>
    /// Test class that contains the tests dedicated for TV Show specific information
    /// </summary>
    [TestClass]
    public class TVShowTests
    {
        /// <summary>
        /// Test Method that tests a production with start date but no end date
        /// </summary>
        [TestMethod]
        public void TVShowWithNoEndTest()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            Entities.Movies.Production production = imdbDataProvider.GetProduction(48861, new FullProductionDataFetchSettings());
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
            Entities.Movies.Production production = imdbDataProvider.GetProduction(397442);
            Assert.IsNotNull(production);
            Assert.AreEqual(397442, production.IMDbID);
            Assert.IsTrue(production.ProductionType == ProductionTypeEnum.TVSeries);
        }
    }
}
