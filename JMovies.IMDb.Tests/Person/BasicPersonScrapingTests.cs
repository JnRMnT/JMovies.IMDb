using JMovies.IMDb.Entities.Interfaces;
using JMovies.IMDb.Entities.Settings.Presets;
using JMovies.IMDb.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JMovies.IMDb.Tests.Person
{
    /// <summary>
    /// Test class that contains the tests for basic scraping scenarios for persons
    /// </summary>
    [TestClass]
    public class BasicPersonScrapingTests
    {
        /// <summary>
        /// Static list of IMDb IDs of some persons to be tested
        /// </summary>
        private static readonly long[] personIDsToTest = new long[] { 18652, 3614913, 5253, 1297015, 3614913, 1877 };

        /// <summary>
        /// Method that tests scraping of person pages
        /// </summary>
        [TestMethod]
        public void PersonScraping()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            foreach (long personID in personIDsToTest)
            {
                Entities.People.Person person = imdbDataProvider.GetPerson(personID, new BasicPersonDataFetchSettings());
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
                Entities.People.Person person = imdbDataProvider.GetPerson(personID, new FullPersonDataFetchSettings());
                Assert.IsNotNull(person);
                Assert.IsFalse(string.IsNullOrEmpty(person.FullName));
                Assert.AreEqual(personID, person.IMDbID);
            }
        }
    }
}
