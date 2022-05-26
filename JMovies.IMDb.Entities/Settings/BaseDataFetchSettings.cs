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
    }
}
