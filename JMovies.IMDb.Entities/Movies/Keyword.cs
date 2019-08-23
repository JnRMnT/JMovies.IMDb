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
    /// Class definition of a keyword related to a title
    /// </summary>
    [Table("Keyword")]
    public class Keyword
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key]
        [Column(Order = 0)]
        public long ID { get; set; }

        /// <summary>
        /// Keyword name
        /// </summary>
        [MaxLength(64)]
        [Column("Name", Order = 2)]
        public string Value { get; set; }

        /// <summary>
        /// Unique Identifier of a keyword
        /// </summary>
        [MaxLength(36)]
        [Column(Order = 1)]
        public string Identifier { get; set; }
    }
}
