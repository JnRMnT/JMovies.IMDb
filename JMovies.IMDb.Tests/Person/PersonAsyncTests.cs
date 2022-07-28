using JMovies.IMDb.Entities.Interfaces;
using JMovies.IMDb.Entities.Settings.Presets;
using JMovies.IMDb.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;

namespace JMovies.IMDb.Tests.Person
{
    /// <summary>
    /// Test suite designed to test asnyc operations
    /// </summary>
    [TestClass]
    public class PersonAsyncTests
    {
        /// <summary>
        /// Static list of IMDb IDs of some persons to be tested
        /// </summary>
        private static readonly long[] personIDsToTest = new long[] { 18652, 3614913, 5253 };

        /// <summary>
        /// Test that checks if the code is running correctly in async mode
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task BasicFetchingTest()
        {
            IIMDbDataProvider imdbDataProvider = new IMDbScraperDataProvider();
            foreach (long personID in personIDsToTest)
            {
                Entities.People.Person person = await imdbDataProvider.GetPersonAsync(personID, new BasicPersonDataFetchSettings());
                Assert.IsNotNull(person);
                Assert.IsFalse(string.IsNullOrEmpty(person.FullName));
                Assert.AreEqual(personID, person.IMDbID);
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
            Task<Entities.People.Person> personFetcher = imdbDataProvider.GetPersonAsync(personIDsToTest[0], new BasicPersonDataFetchSettings());
            stopwatch.Stop();
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 1000);
            Assert.IsTrue(!personFetcher.IsCompleted);
            Entities.People.Person person = personFetcher.Result;
            Assert.IsNotNull(person);
            Assert.IsFalse(string.IsNullOrEmpty(person.FullName));
            Assert.AreEqual(personIDsToTest[0], person.IMDbID);
        }
    }
}
