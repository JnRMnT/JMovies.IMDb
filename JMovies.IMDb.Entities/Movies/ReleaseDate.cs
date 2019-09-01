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
        [ForeignKey("CountryID")]
        public virtual Country Country { get; set; }

        /// <summary>
        /// Reference field for country
        /// </summary>
        public long CountryID { get; set; }

        /// <summary>
        /// Description related to the release date
        /// </summary>
        [MaxLength(64)]
        public string Description { get; set; }

        /// <summary>
        /// Reference field for related Production
        /// </summary>
        public long ProductionID { get; set; }
    }
}
