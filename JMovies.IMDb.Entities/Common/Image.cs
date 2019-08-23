using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JMovies.IMDb.Entities.Common
{
    /// <summary>
    /// Class definition for basic online image
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Title of the image
        /// </summary>
        [MaxLength(255)]
        public string Title { get; set; }
        /// <summary>
        /// URL of the image
        /// </summary>
        [MaxLength(255)]
        public string URL { get; set; }
    }
}
