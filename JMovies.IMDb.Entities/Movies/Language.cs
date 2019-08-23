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
    /// Class definition of a language
    /// </summary>
    [Table("Language")]
    public class Language
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key]
        [Column(Order = 0)]
        public long ID { get; set; }


        /// <summary>
        /// Name of the language
        /// </summary>
        [MaxLength(64)]
        [Column(Order = 2)]
        public string Name { get; set; }

        /// <summary>
        /// Unique Identifier of the language
        /// </summary>
        [MaxLength(36)]
        [Column(Order = 1)]
        public string Identifier { get; set; }
    }
}
