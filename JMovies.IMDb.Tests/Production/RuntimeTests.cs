using JMovies.IMDb.Entities.Interfaces;
using JMovies.IMDb.Entities.Settings;
using JMovies.IMDb.Entities.Settings.Presets;
using JMovies.IMDb.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace JMovies.IMDb.Tests.Production
{
    /// <summary>
    /// Test class that contains the tests which are targeted for scraping runtime info related tests
    /// </summary>
    [TestClass]
    public class RuntimeTests
    {
        /// <summary>
        /// Test Method that tests runtime info of the TV Series "Hunters"
        /// </summary>
        [TestMethod]
        public void RuntimeOfHunters()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            ProductionDataFetchSettings settings = new BasicProductionDataFetchSettings();
            Entities.Movies.TVSeries tvSeries = imdbDataProvider.GetTvSeries(7456722, settings); // https://www.imdb.com/title/tt7456722/
            Assert.IsNotNull(tvSeries);
            Assert.AreNotEqual(default, tvSeries.Runtime);
            Assert.AreEqual(TimeSpan.FromMinutes(60), tvSeries.Runtime);
        }
    }
}
