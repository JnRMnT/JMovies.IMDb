using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition that handles country to production mapping
    /// </summary>
    public class ProductionCountry
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key]
        public long ID { get; set; }

        /// <summary>
        /// Country of the production
        /// </summary>
        [Required]
        [ForeignKey("CountryID")]
        public virtual Country Country { get; set; }

        /// <summary>
        /// Related Production
        /// </summary>
        [Required]
        [ForeignKey("ProductionID")]
        public virtual Production Production { get; set; }

        /// <summary>
        /// Reference id of the Production
        /// </summary>
        public long ProductionID { get; set; }


        /// <summary>
        /// Reference id of the Country
        /// </summary>
        public long CountryID { get; set; }
    }
}
