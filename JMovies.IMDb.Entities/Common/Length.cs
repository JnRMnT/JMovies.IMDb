using System;
using System.Collections.Generic;
using System.Text;

namespace JMovies.IMDb.Entities.Common
{
    public class Length
    {
        public Length(int metricLength)
        {
            this.Metric = metricLength;
        }

        public int Metric { get; set; }
        public double Imperial
        {
            get
            {
                return (double)Metric / 2.54;
            }
        }
    }
}
