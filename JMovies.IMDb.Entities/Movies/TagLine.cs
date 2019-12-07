using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Tag line of the production
    /// </summary>
    public class TagLine
    {
        /// <summary>
        /// Primary identifier of a tag line
        /// </summary>
        [Key]
        public long ID { get; set; }

        /// <summary>
        /// Content of the tag line
        /// </summary>
        [Required]
        [MaxLength(512)]
        public string Content { get; set; }

        /// <summary>
        /// Reference field for Production
        /// </summary>
        public long ProductionID { get; set; }
    }
}
