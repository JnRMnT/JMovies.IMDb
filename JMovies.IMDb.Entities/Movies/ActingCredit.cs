using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Class definition of an Acting Credit
    /// </summary>
    public class ActingCredit: Credit
    {
        /// <summary>
        /// Characters played by the actor/actress
        /// </summary>
        public Character[] Characters { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ActingCredit() : base()
        {
            this.RoleType = CreditRoleType.Acting;
        }
    }
}
