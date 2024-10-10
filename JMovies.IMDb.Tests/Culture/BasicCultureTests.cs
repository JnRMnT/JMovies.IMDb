using JMovies.IMDb.Entities.Interfaces;
using JMovies.IMDb.Entities.Settings.Presets;
using JMovies.IMDb.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JMovies.IMDb.Tests.Culture
{
    /// <summary>
    /// Test class responsible for checking the locale/culture related issues that might occur
    /// </summary>
    [TestClass]
    public class BasicCultureTests
    {
        /// <summary>
        /// Joker as the test id
        /// </summary>
        private const long testID = 11315808;

        /// <summary>
        /// The very basic test that uses en-UK as preferred culture
        /// </summary>
        [TestMethod]
        public void EnglishBasicTest()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            Entities.Movies.Movie movie = imdbDataProvider.GetMovie(testID, new BasicProductionDataFetchSettings
            {
                PreferredCulture = "en-UK"
            });
            Assert.IsNotNull(movie);
            Assert.AreEqual(testID, movie.IMDbID);
        }

        /// <summary>
        /// The very basic test that uses en-UK as preferred culture to check datetime parsing issues
        /// </summary>
        [TestMethod]
        public void BrazilianBasicTest()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            Entities.Movies.Movie movie = imdbDataProvider.GetMovie(testID, new BasicProductionDataFetchSettings
            {
                PreferredCulture = "pt-BR"
            });
            Assert.IsNotNull(movie);
            Assert.AreEqual(testID, movie.IMDbID);
        }

        /// <summary>
        /// The very basic test that uses tr-TR as preferred culture to check non-latin chars
        /// </summary>
        [TestMethod]
        public void TurkishBasicTest()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            Entities.Movies.Movie movie = imdbDataProvider.GetMovie(testID, new BasicProductionDataFetchSettings
            {
                PreferredCulture = "tr-TR"
            });
            Assert.IsNotNull(movie);
            Assert.AreEqual(testID, movie.IMDbID);
        }
    }
}
