using System.Text.Json.Serialization;

namespace JMovies.IMDb.Entities.PrivateAPI.Production
{
    /// <summary>
    /// Class Definition of a private Production AKA Data
    /// </summary>
    public class AKAData
    {
        /// <summary>
        /// Edges property
        /// </summary>
        [JsonPropertyName("edges")]
        public AKAsEdge[] AKAEdges { get; set; }
    }
}
