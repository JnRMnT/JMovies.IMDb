using JMovies.IMDb.Entities.Interfaces;
using JMovies.IMDb.Entities.Settings;
using JMovies.IMDb.Entities.Settings.Presets;
using JMovies.IMDb.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace JMovies.IMDb.Tests.Production
{
    /// <summary>
    /// Test class that contains the tests which are targeted for scraping runtime info related tests
    /// </summary>
    [TestClass]
    public class GenreTests
    {
        /// <summary>
        /// Test Method that tests genre info of the TV Series "Hunters"
        /// </summary>
        [TestMethod]
        public void TestGenreOfHunters()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            ProductionDataFetchSettings settings = new BasicProductionDataFetchSettings();
            Entities.Movies.TVSeries tvSeries = imdbDataProvider.GetTvSeries(7456722, settings); // https://www.imdb.com/title/tt7456722/
            Assert.IsNotNull(tvSeries);
            Assert.IsNotNull(tvSeries.Genres);
            Assert.AreNotEqual(0, tvSeries.Genres.Count);
            Assert.IsTrue(Enumerable.SequenceEqual(new string[] { "Crime", "Drama", "Mystery" }, tvSeries.Genres.Select(e => e.Value).ToArray()));
        }
    }
}
