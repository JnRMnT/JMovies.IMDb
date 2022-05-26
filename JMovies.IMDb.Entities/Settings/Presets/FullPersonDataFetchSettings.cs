namespace JMovies.IMDb.Entities.Settings.Presets
{
    /// <summary>
    /// A preset of data fetch settings that fetches all the data available for persons
    /// </summary>
    public class FullPersonDataFetchSettings : PersonDataFetchSettings
    {
        /// <summary>
        /// Fetch the bio page
        /// </summary>
        public override bool FetchBioPage { get => true; }

        /// <summary>
        /// Fetch the image contents
        /// </summary>
        public override bool FetchImageContents { get => true; }

        /// <summary>
        /// Fetch 25 media images
        /// </summary>
        public override int MediaImagesFetchCount { get => 25; }
    }
}
