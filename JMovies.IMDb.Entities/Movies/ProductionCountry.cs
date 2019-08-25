using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public Country Country { get; set; }

        /// <summary>
        /// Related Production
        /// </summary>
        [Required]
        public Production Production { get; set; }
    }
}
