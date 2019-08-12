using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.Movies
{
    /// <summary>
    /// Enumeration of Role Types in a credit
    /// </summary>
    public enum CreditRoleType
    {
        /// <summary>
        /// Default Value of Credit Role
        /// </summary>
        Undefined,
        /// <summary>
        /// Director role in a title
        /// </summary>
        Director,
        /// <summary>
        /// Acting role in a title
        /// </summary>
        Acting,
        /// <summary>
        /// Writer role in a title
        /// </summary>
        Writer,
        /// <summary>
        /// Creator role in a title
        /// </summary>
        Creator,
        /// <summary>
        /// Producer role in a title
        /// </summary>
        Producer,
        /// <summary>
        /// Composer role in a title
        /// </summary>
        Composer,
        /// <summary>
        /// Part of a Music Department
        /// </summary>
        MusicDepartment,
        /// <summary>
        /// Actor role in a title
        /// </summary>
        Actor,
        /// <summary>
        /// Actress role in a title
        /// </summary>
        Actress
    }
}
