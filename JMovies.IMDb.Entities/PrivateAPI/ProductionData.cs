using JMovies.IMDb.Entities.PrivateAPI.Production;
using System.Text.Json.Serialization;

namespace JMovies.IMDb.Entities.PrivateAPI
{
    /// <summary>
    /// Class Definition of a private Production Data
    /// </summary>
    public class ProductionData
    {
        /// <summary>
        /// AKAs property
        /// </summary>
        [JsonPropertyName("akas")]
        public AKAData AKAsData { get; set; }
    }
}
