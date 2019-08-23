using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition of an Official Site
    /// </summary>
    public class OfficialSite
    {
        /// <summary>
        /// Title of the official website
        /// </summary>
        [MaxLength(128)]
        public string Title { get; set; }

        /// <summary>
        /// URL of the official website
        /// </summary>
        [MaxLength(128)]
        public string URL { get; set; }
    }
}
