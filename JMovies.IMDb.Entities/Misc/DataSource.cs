using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JMovies.IMDb.Entities.Misc
{

    /// <summary>
    /// Class Definition of a data source
    /// </summary>
    public class DataSource
    {
        /// <summary>
        /// Default constructor of data source
        /// </summary>
        public DataSource()
        {

        }

        /// <summary>
        /// Datasource constructor by type
        /// </summary>
        /// <param name="dataSourceType"></param>
        public DataSource(DataSourceTypeEnum dataSourceType)
        {
            this.DataSourceType = dataSourceType;
        }

        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Identifier
        {
            get
            {
                return (int)DataSourceType;
            }
            set
            {
                DataSourceType = (DataSourceTypeEnum)value;
            }
        }

        /// <summary>
        /// Name of the source
        /// </summary>
        [MaxLength(32)]
        public string Name
        {
            get
            {
                return DataSourceType.ToString();
            }
            set { }
        }

        /// <summary>
        /// Type of the data source
        /// </summary>
        [NotMapped]
        public DataSourceTypeEnum DataSourceType { get; set; }
    }
}
