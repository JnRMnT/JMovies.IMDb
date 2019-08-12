using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition of a TV Character
    /// </summary>
    public class TVCharacter : Character
    {
        /// <summary>
        /// Count of episodes the character was in
        /// </summary>
        public int EpisodeCount { get; set; }

        /// <summary>
        /// Character first appereance year
        /// </summary>
        public int? StartYear { get; set; }

        /// <summary>
        /// Character last appereance year
        /// </summary>
        public int? EndYear { get; set; }
    }
}
