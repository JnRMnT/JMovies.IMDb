using JMovies.IMDb.Entities.Movies;
using JMovies.IMDb.Entities.People;
using JMovies.IMDb.Entities.Settings;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.Interfaces
{
    /// <summary>
    /// Data provider interface that Scraper implements. This interface can be safely used for Dependency Injection.
    /// </summary>
    public interface IIMDbDataProvider
    {
        /// <summary>
        /// Gets Production Information iresspective of type using the default settings
        /// </summary>
        /// <param name="id">ID of the production</param>
        /// <returns>Production instance containing retreived information</returns>
        Production GetProduction(long id);

        /// <summary>
        /// Gets Production Information iresspective of type using the default settings
        /// </summary>
        /// <param name="id">ID of the production</param>
        /// <returns>Task that fetches Production instance containing retreived information</returns>
        Task<Production> GetProductionAsync(long id);

        /// <summary>
        /// Gets Movie information using the default settings
        /// </summary>
        /// <param name="id">ID of the movie</param>
        /// <returns>Movie instance containing retreived information</returns>
        Movie GetMovie(long id);

        /// <summary>
        /// Gets Movie information using the default settings
        /// </summary>
        /// <param name="id">ID of the movie</param>
        /// <returns>Task that fetches Movie instance containing retreived information</returns>
        Task<Movie> GetMovieAsync(long id);

        /// <summary>
        /// Gets TV Series information using the default settings
        /// </summary>
        /// <param name="id">ID of the TV Series</param>
        /// <returns>TV Series instance containing retreived information</returns>
        TVSeries GetTvSeries(long id);

        /// <summary>
        /// Gets TV Series information using the default settings
        /// </summary>
        /// <param name="id">ID of the TV Series</param>
        /// <returns>Task that fetches TV Series instance containing retreived information</returns>
        Task<TVSeries> GetTvSeriesAsync(long id);

        /// <summary>
        /// Gets Production Information iresspective of type
        /// </summary>
        /// <param name="id">ID of the production</param>
        /// <param name="settings">Object containing Data Fetch settings</param>
        /// <returns>Production instance containing retreived information</returns>
        Production GetProduction(long id, ProductionDataFetchSettings settings);

        /// <summary>
        /// Gets Production Information iresspective of type
        /// </summary>
        /// <param name="id">ID of the production</param>
        /// <param name="settings">Object containing Data Fetch settings</param>
        /// <returns>Task that fetches Production instance containing retreived information</returns>
        Task<Production> GetProductionAsync(long id, ProductionDataFetchSettings settings);

        /// <summary>
        /// Gets Movie information
        /// </summary>
        /// <param name="id">ID of the movie</param>
        /// <param name="settings">Object containing Data Fetch settings</param>
        /// <returns>Movie instance containing retreived information</returns>
        Movie GetMovie(long id, ProductionDataFetchSettings settings);

        /// <summary>
        /// Gets Movie information
        /// </summary>
        /// <param name="id">ID of the movie</param>
        /// <param name="settings">Object containing Data Fetch settings</param>
        /// <returns>Taks that fetches Movie instance containing retreived information</returns>
        Task<Movie> GetMovieAsync(long id, ProductionDataFetchSettings settings);

        /// <summary>
        /// Gets TV Series information
        /// </summary>
        /// <param name="id">ID of the TV Series</param>
        /// <param name="settings">Object containing Data Fetch settings</param>
        /// <returns>TV Series instance containing retreived information</returns>
        TVSeries GetTvSeries(long id, ProductionDataFetchSettings settings);

        /// <summary>
        /// Gets TV Series information
        /// </summary>
        /// <param name="id">ID of the TV Series</param>
        /// <param name="settings">Object containing Data Fetch settings</param>
        /// <returns>Task that fetches TV Series instance containing retreived information</returns>
        Task<TVSeries> GetTvSeriesAsync(long id, ProductionDataFetchSettings settings);

        /// <summary>
        /// Gets Person information
        /// </summary>
        /// <param name="id">ID of the Person</param>
        /// <param name="settings">Object containing Data Fetch settings</param>
        /// <returns>Person instance containing retreived information</returns>
        Person GetPerson(long id, PersonDataFetchSettings settings);

        /// <summary>
        /// Gets Person information
        /// </summary>
        /// <param name="id">ID of the Person</param>
        /// <param name="settings">Object containing Data Fetch settings</param>
        /// <returns>Task that fetches Person instance containing retreived information</returns>
        Task<Person> GetPersonAsync(long id, PersonDataFetchSettings settings);

        /// <summary>
        /// Gets Person information using the default settings
        /// </summary>
        /// <param name="id">ID of the Person</param>
        /// <returns>Person instance containing retreived information</returns>
        Person GetPerson(long id);

        /// <summary>
        /// Gets Person information using the default settings
        /// </summary>
        /// <param name="id">ID of the Person</param>
        /// <returns>Task that fetches the Person instance containing retreived information</returns>
        Task<Person> GetPersonAsync(long id);
    }
}