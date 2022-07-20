using JMovies.IMDb.Entities.Interfaces;
using JMovies.IMDb.Entities.Settings;
using JMovies.IMDb.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JMovies.IMDb.Tests.Production
{
    /// <summary>
    /// Test suite for tests related to fetching of the gallery page of the productions
    /// </summary>
    [TestClass]
    public class ProductionGalleryTests
    {
        /// <summary>
        /// IMDb ID of Justice League
        /// </summary>
        private static readonly long justiceLeagueID = 12361974;


        /// <summary>
        /// Fetches the production data with 5 images
        /// </summary>
        [TestMethod]
        public void FetchingLimitedImages()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            ProductionDataFetchSettings settings = new ProductionDataFetchSettings
            {
                FetchDetailedCast = false,
                FetchImageContents = true,
                //5 is a value that is lower than a full page, so we are testing if we are able to stop at the limit
                MediaImagesFetchCount = 5
            };
            Entities.Movies.Production production = imdbDataProvider.GetProduction(justiceLeagueID, settings);
            Assert.IsNotNull(production);
            Assert.AreEqual(justiceLeagueID, production.IMDbID);
            Assert.IsNotNull(production.MediaImages);
            Assert.AreEqual(settings.MediaImagesFetchCount, production.MediaImages.Count);
        }

        /// <summary>
        /// Fetches the production data with 55 images
        /// </summary>
        [TestMethod]
        public void FetchingWithPagination()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            ProductionDataFetchSettings settings = new ProductionDataFetchSettings
            {
                FetchDetailedCast = false,
                FetchImageContents = true,
                //55 is a value that fetches at least 2 pages but still relatively low
                MediaImagesFetchCount = 55
            };
            Entities.Movies.Production production = imdbDataProvider.GetProduction(justiceLeagueID, settings);
            Assert.IsNotNull(production);
            Assert.AreEqual(justiceLeagueID, production.IMDbID);
            Assert.IsNotNull(production.MediaImages);
            Assert.AreEqual(settings.MediaImagesFetchCount, production.MediaImages.Count);
        }
    }
}
