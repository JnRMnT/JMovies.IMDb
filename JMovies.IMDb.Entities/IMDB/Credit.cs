using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.IMDB
{
    public class Credit
    {
        public Person Person { get; set; }
        public CreditRoleType RoleType { get; set; }
    }
}
