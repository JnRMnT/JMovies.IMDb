namespace JMovies.IMDb.Entities.Settings
{
    /// <summary>
    /// Builder for person fetching settings
    /// </summary>
    public class PersonFetchSettingsBuilder
    {
        private PersonDataFetchSettings _settings;
        private PersonDataFetchSettings settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = new PersonDataFetchSettings();
                }
                return _settings;
            }
        }

        /// <summary>
        /// Enables fetching bio page on the settings
        /// </summary>
        /// <returns>Builder for more chaining</returns>
        public PersonFetchSettingsBuilder WithFetchingBioPage()
        {
            settings.FetchBioPage = true;
            return this;
        }

        /// <summary>
        /// Enables fetching image contents fetching on the settings
        /// </summary>
        /// <returns>Builder for more chaining</returns>
        public PersonFetchSettingsBuilder WithFetchingImageContents()
        {
            settings.FetchImageContents = true;
            return this;
        }

        /// <summary>
        /// Sets image fetching limit
        /// </summary>
        /// <returns>Builder for more chaining</returns>
        public PersonFetchSettingsBuilder WithLimitedImageFetching(int limit)
        {
            settings.MediaImagesFetchCount = limit;
            return this;
        }


        /// <summary>
        /// Enables fetching with a preferred culture
        /// </summary>
        /// <returns>Builder for more chaining</returns>
        public PersonFetchSettingsBuilder WithPreferredCulture(string culture)
        {
            settings.PreferredCulture = culture;
            return this;
        }

        /// <summary>
        /// Builds a person data fetch settings based on the chained properties
        /// </summary>
        /// <returns>A built and customized person data fetch settings</returns>
        public PersonDataFetchSettings Build()
        {
            return settings;
        }
    }
}
