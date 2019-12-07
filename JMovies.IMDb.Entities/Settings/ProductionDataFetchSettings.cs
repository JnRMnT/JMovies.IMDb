using System;
using System.Collections.Generic;
using System.Text;

namespace JMovies.IMDb.Entities.Settings
{
    /// <summary>
    /// Class that is responsible for holding settings for Production data fetching
    /// </summary>
    public class ProductionDataFetchSettings
    {
        /// <summary>
        /// Should the detailed cast info be fetched? This effects the response time.
        /// </summary>
        public virtual bool FetchDetailedCast { get; set; }

        /// <summary>
        /// Should the byte array content of the images be fetched along with URLs
        /// </summary>
        public virtual bool FetchImageContents { get; set; }

        /// <summary>
        /// Number of media images to be fetched along with the production
        /// </summary>
        public virtual int MediaImagesFetchCount { get; set; }


        /// <summary>
        /// Number of cast to be fetched along with the production
        /// </summary>
        public virtual int CastFetchCount { get; set; }
    }
}
