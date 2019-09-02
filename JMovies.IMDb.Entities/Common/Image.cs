using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JMovies.IMDb.Entities.Common
{
    /// <summary>
    /// Class definition for basic online image
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key]
        public long ID { get; set; }

        /// <summary>
        /// Title of the image
        /// </summary>
        [MaxLength(255)]
        public string Title { get; set; }
        /// <summary>
        /// URL of the image
        /// </summary>
        [MaxLength(255)]
        [Column("SourceURL")]
        public string URL { get; set; }

        /// <summary>
        /// Content of the image
        /// </summary>
        public byte[] Content { get; set; }
    }
}
