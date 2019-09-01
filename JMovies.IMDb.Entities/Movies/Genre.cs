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
    /// Class definition of a genre
    /// </summary>
    [Table("Genre")]
    public class Genre
    {

        /// <summary>
        /// Primary Key
        /// </summary>
        [Key]
        [Column(Order = 0)]
        public long ID { get; set; }

        /// <summary>
        /// Name of the Genre
        /// </summary>
        [MaxLength(64)]
        [Column("Name", Order = 2)]
        public string Value { get; set; }

        /// <summary>
        /// Unique identifier of the Genre
        /// </summary>
        [MaxLength(36)]
        [Column(Order = 1)]
        public string Identifier { get; set; }

        /// <summary>
        /// Reference field for Production
        /// </summary>
        public long ProductionID { get; set; }
    }
}
