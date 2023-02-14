namespace JMovies.IMDb.Entities.Settings
{
    /// <summary>
    /// Base settings class common to both productions and persons
    /// </summary>
    public class BaseDataFetchSettings
    {
        /// <summary>
        /// Preferred culture for fetching data (where applicable)
        /// </summary>
        public virtual string PreferredCulture { get; set; }

        /// <summary>
        /// Toggles fetching private data available through normally user-click on the website.
        /// DISCLAIMER: Use this only on private projects. Usage of these APIs are not allowed for non-private purposes.
        /// </summary>
        public virtual bool FetchPrivateData { get; set; }
    }
}
