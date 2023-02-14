namespace JMovies.IMDb.Entities.Settings
{
    /// <summary>
    /// Class that is responsible for holding settings for Production data fetching
    /// </summary>
    public class ProductionFetchSettingsBuilder
    {
        private ProductionDataFetchSettings _settings;
        private ProductionDataFetchSettings settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = new ProductionDataFetchSettings();
                }
                return _settings;
            }
        }

        /// <summary>
        /// Enables fetching detailed cast on the settings
        /// </summary>
        /// <returns>Builder for more chaining</returns>
        public ProductionFetchSettingsBuilder WithDetailedCast()
        {
            settings.FetchDetailedCast = true;
            return this;
        }

        /// <summary>
        /// Enables limiting fetching the cast details
        /// </summary>
        /// <returns>Builder for more chaining</returns>
        public ProductionFetchSettingsBuilder WithLimitedCastDetails(int limit)
        {
            settings.CastFetchCount = limit;
            return this;
        }

        /// <summary>
        /// Enables fetching image contents fetching on the settings
        /// </summary>
        /// <returns>Builder for more chaining</returns>
        public ProductionFetchSettingsBuilder WithFetchingImageContents()
        {
            settings.FetchImageContents = true;
            return this;
        }

        /// <summary>
        /// Sets image fetching limit
        /// </summary>
        /// <returns>Builder for more chaining</returns>
        public ProductionFetchSettingsBuilder WithLimitedImageFetching(int limit)
        {
            settings.MediaImagesFetchCount = limit;
            return this;
        }


        /// <summary>
        /// Enables fetching with a preferred culture
        /// </summary>
        /// <returns>Builder for more chaining</returns>
        public ProductionFetchSettingsBuilder WithPreferredCulture(string culture)
        {
            settings.PreferredCulture = culture;
            return this;
        }

        /// <summary>
        /// Enables fetching private data
        /// </summary>
        /// <returns>Builder for more chaining</returns>
        public ProductionFetchSettingsBuilder WithPrivateData()
        {
            settings.FetchPrivateData = true;
            return this;
        }

        /// <summary>
        /// Builds a production data fetch settings based on the chained properties
        /// </summary>
        /// <returns>A built and customized production data fetch settings</returns>
        public ProductionDataFetchSettings Build()
        {
            return settings;
        }
    }
}
