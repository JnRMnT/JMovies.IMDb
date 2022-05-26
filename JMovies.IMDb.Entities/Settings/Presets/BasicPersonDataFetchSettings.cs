namespace JMovies.IMDb.Entities.Settings.Presets
{
    /// <summary>
    /// A preset of settings to fetch only basic person data
    /// </summary>
    public class BasicPersonDataFetchSettings : PersonDataFetchSettings
    {
        /// <summary>
        /// Do not fetch the bio page
        /// </summary>
        public override bool FetchBioPage { get => false; }

        /// <summary>
        /// Do not fetch image contents
        /// </summary>
        public override bool FetchImageContents { get => false; }
    }
}
