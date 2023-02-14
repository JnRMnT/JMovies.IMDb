using JMovies.IMDb.Common.Constants;
using JMovies.IMDb.Entities.Settings;

namespace JMovies.IMDb.Common.Extensions
{
    /// <summary>
    /// Provides extension methods for the BasicDataFetchSettings type of objects
    /// </summary>
    public static class BaseDataFetchSettingsExtensions
    {
        /// <summary>
        /// Gets the active culture if one preferred or the default
        /// </summary>
        /// <returns>The active culture (preferred or default)</returns>
        public static string GetActiveCulture(this BaseDataFetchSettings settings)
        {
            return !string.IsNullOrEmpty(settings.PreferredCulture) ? settings.PreferredCulture : IMDbConstants.DefaultScrapingCulture;
        }
    }
}
