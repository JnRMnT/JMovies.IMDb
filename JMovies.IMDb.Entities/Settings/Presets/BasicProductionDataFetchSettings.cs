namespace JMovies.IMDb.Entities.Settings.Presets
{
    /// <summary>
    /// A preset of settings to fetch only basic production data
    /// </summary>
    public class BasicProductionDataFetchSettings : ProductionDataFetchSettings
    {
        /// <summary>
        /// Do not fetch detailed cast
        /// </summary>
        public override bool FetchDetailedCast { get => false; }

        /// <summary>
        /// Do not fetch image contents
        /// </summary>
        public override bool FetchImageContents { get => false; }

        /// <summary>
        /// Do not fetch media images
        /// </summary>
        public override int MediaImagesFetchCount { get => 0; }

        /// <summary>
        /// Do not fetch the cast
        /// </summary>
        public override int CastFetchCount { get => 0; }
    }
}
