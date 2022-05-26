using JMovies.IMDb.Entities.Interfaces;
using JMovies.IMDb.Entities.Settings.Presets;
using JMovies.IMDb.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JMovies.IMDb.Tests.Production
{
    /// <summary>
    /// Test class that contains the tests which are targeted for scraping culture related tests
    /// </summary>
    [TestClass]
    public class ScrapingCultureTests
    {
        /// <summary>
        /// Test Method that tests scraping with the default culture
        /// </summary>
        [TestMethod]
        public void TestDefaultCulture()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            Entities.Movies.Movie movie = imdbDataProvider.GetMovie(232500); // https://www.imdb.com/title/tt0232500/
            Assert.IsNotNull(movie);
            Assert.AreEqual("The Fast and the Furious", movie.Title);
        }

        /// <summary>
        /// Test Method that tests scraping with the Turkish culture
        /// </summary>
        [TestMethod]
        public void TestTurkishCulture()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            Entities.Movies.Movie movie = imdbDataProvider.GetMovie(232500, new BasicProductionDataFetchSettings
            {
                PreferredCulture = "tr-TR"
            }); // https://www.imdb.com/title/tt0232500/
            Assert.IsNotNull(movie);
            Assert.AreEqual("Hızlı ve Öfkeli", movie.Title);
        }
    }
}
