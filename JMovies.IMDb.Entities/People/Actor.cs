using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.People
{
    /// <summary>
    /// Class definition of an actor
    /// </summary>
    public class Actor: Person
    {
        /// <summary>
        /// Overriden property of PersonType that returns Actor always
        /// </summary>
        public override PersonTypeEnum PersonType { get => PersonTypeEnum.Actor; set => base.PersonType = value; }
    }
}
