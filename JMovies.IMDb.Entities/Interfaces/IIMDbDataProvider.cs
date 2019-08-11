using JMovies.IMDb.Entities.Movies;
using JMovies.IMDb.Entities.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.Interfaces
{
    /// <summary>
    /// Data provider interface that Scraper implements. This interface can be safely used for Dependency Injection.
    /// </summary>
    public interface IIMDbDataProvider
    {
        /// <summary>
        /// Gets Production Information iresspective of type
        /// </summary>
        /// <param name="id">ID of the production</param>
        /// <param name="fetchDetailedCast">Should the detailed cast info be fetched? This effects the response time.</param>
        /// <returns>Production instance containing retreived information</returns>
        Production GetProduction(long id, bool fetchDetailedCast);

        /// <summary>
        /// Gets Movie information
        /// </summary>
        /// <param name="id">ID of the movie</param>
        /// <param name="fetchDetailedCast">Should the detailed cast info be fetched? This effects the response time.</param>
        /// <returns>Movie instance containing retreived information</returns>
        Movie GetMovie(long id, bool fetchDetailedCast);

        /// <summary>
        /// Gets TV Series information
        /// </summary>
        /// <param name="id">ID of the TV Series</param>
        /// <param name="fetchDetailedCast">Should the detailed cast info be fetched? This effects the response time.</param>
        /// <returns>TV Series instance containing retreived information</returns>
        TVSeries GetTvSeries(long id, bool fetchDetailedCast);

        /// <summary>
        /// Gets Person information
        /// </summary>
        /// <param name="id">ID of the Person</param>
        /// <param name="fetchAdditionalDetails">Should the additional details be also fetched? This effects the response time.</param>
        /// <returns>Person instance containing retreived information</returns>
        Person GetPerson(long id, bool fetchAdditionalDetails);
    }
}