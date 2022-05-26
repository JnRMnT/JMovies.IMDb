namespace JMovies.IMDb.Entities.Settings
{
    /// <summary>
    /// Class that is responsible for holding settings for Person data fetching
    /// </summary>
    public class PersonDataFetchSettings : BaseDataFetchSettings
    {
        /// <summary>
        /// Should the Bio Page details be also fetched? This effects the response time
        /// </summary>
        public virtual bool FetchBioPage { get; set; }

        /// <summary>
        /// Should the byte array content of the images be fetched along with URLs
        /// </summary>
        public virtual bool FetchImageContents { get; set; }

        /// <summary>
        /// Number of media images to be fetched along with the person
        /// </summary>
        public virtual int MediaImagesFetchCount { get; set; }
    }
}
