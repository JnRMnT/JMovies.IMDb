using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition of an Acting Credit
    /// </summary>
    public class ActingCredit : Credit
    {
        /// <summary>
        /// Characters played by the actor/actress
        /// </summary>
        [ForeignKey("CreditID")]
        public virtual ICollection<Character> Characters { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ActingCredit() : base()
        {
            this.RoleType = CreditRoleType.Acting;
        }
    }
}
