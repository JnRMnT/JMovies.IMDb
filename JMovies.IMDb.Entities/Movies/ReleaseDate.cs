using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition of release dates
    /// </summary>
    [Table("ReleaseDate")]
    public class ReleaseDate
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key]
        public long ID { get; set; }

        /// <summary>
        /// Date of the release
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Country of release date
        /// </summary>
        public Country Country { get; set; }
    }
}
