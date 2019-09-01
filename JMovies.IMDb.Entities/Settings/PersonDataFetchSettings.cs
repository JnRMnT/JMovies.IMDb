﻿using System;
using System.Collections.Generic;
using System.Text;

namespace JMovies.IMDb.Entities.Settings
{
    /// <summary>
    /// Class that is responsible for holding settings for Person data fetching
    /// </summary>
    public class PersonDataFetchSettings
    {
        /// <summary>
        /// Should the Bio Page details be also fetched? This effects the response time
        /// </summary>
        public bool FetchBioPage { get; set; }

        /// <summary>
        /// Should the byte array content of the images be fetched along with URLs
        /// </summary>
        public bool FetchImageContents { get; set; }
    }
}
