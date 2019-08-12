using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMovies.IMDb.Entities.Common
{
    /// <summary>
    /// Class definition of Money related Amounts
    /// </summary>
    public class Amount
    {
        /// <summary>
        /// Decimal value of the amount
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Currency of the amount
        /// </summary>
        public string Currency { get; set; }
    }
}
