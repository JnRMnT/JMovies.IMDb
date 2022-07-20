using JMovies.IMDb.Entities.Interfaces;
using JMovies.IMDb.Entities.Settings;
using JMovies.IMDb.Entities.Settings.Presets;
using JMovies.IMDb.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JMovies.IMDb.Tests.Person
{
    /// <summary>
    /// Test suite for tests related to fetching of the gallery page of the persons
    /// </summary>
    [TestClass]
    public class PersonGalleryTests
    {
        /// <summary>
        /// IMDb ID of Ben Affleck
        /// </summary>
        private static readonly long benAffleckID = 255;

        /// <summary>
        /// Fetches the person data with 5 images
        /// </summary>
        [TestMethod]
        public void FetchingLimitedImages()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            PersonDataFetchSettings settings = new PersonDataFetchSettings
            {
                FetchBioPage = false,
                FetchImageContents = true,
                //5 is a value that is lower than a full page, so we are testing if we are able to stop at the limit
                MediaImagesFetchCount = 5
            };
            Entities.People.Person person = imdbDataProvider.GetPerson(benAffleckID, settings);
            Assert.IsNotNull(person);
            Assert.AreEqual(benAffleckID, person.IMDbID);
            Assert.IsNotNull(person.Photos);
            Assert.AreEqual(settings.MediaImagesFetchCount, person.Photos.Count);
        }

        /// <summary>
        /// Fetches the person data with 55 images
        /// </summary>
        [TestMethod]
        public void FetchingWithPagination()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            PersonDataFetchSettings settings = new PersonDataFetchSettings
            {
                FetchBioPage = false,
                FetchImageContents = true,
                //55 is a value that fetches at least 2 pages but still relatively low
                MediaImagesFetchCount = 55
            };
            Entities.People.Person person = imdbDataProvider.GetPerson(benAffleckID, settings);
            Assert.IsNotNull(person);
            Assert.AreEqual(benAffleckID, person.IMDbID);
            Assert.IsNotNull(person.Photos);
            Assert.AreEqual(settings.MediaImagesFetchCount, person.Photos.Count);
        }
    }
}
