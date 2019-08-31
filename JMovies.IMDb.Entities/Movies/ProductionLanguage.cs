using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition that handles language to production mapping
    /// </summary>
    public class ProductionLanguage
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key]
        public long ID { get; set; }

        /// <summary>
        /// Language of the production
        /// </summary>
        [Required]
        public virtual Language Language { get; set; }

        /// <summary>
        /// Related Production
        /// </summary>
        [Required]
        public virtual Production Production { get; set; }

        /// <summary>
        /// Reference id of the production
        /// </summary>
        public long ProductionID { get; set; }

        /// <summary>
        /// Reference id of the language
        /// </summary>
        public long LanguageID { get; set; }
    }
}
