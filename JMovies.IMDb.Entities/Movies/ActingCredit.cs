using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.Movies
{
    public class ActingCredit: Credit
    {
        public Character[] Characters { get; set; }

        public ActingCredit() : base()
        {
            this.RoleType = CreditRoleType.Acting;
        }
    }
}
