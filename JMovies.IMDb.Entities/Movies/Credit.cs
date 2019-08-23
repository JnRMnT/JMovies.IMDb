using JMovies.IMDb.Entities.People;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition of a credit in a title
    /// </summary>
    [Table("Credit")]
    public class Credit
    {

        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        public long ID { get; set; }

        /// <summary>
        /// Person related to the credit
        /// </summary>
        public Person Person { get; set; }

        /// <summary>
        /// Role of the person in the title
        /// </summary>
        [MaxLength(2)]
        public CreditRoleType RoleType { get; set; }
    }
}
