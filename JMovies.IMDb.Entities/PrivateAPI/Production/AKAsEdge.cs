using System.Text.Json.Serialization;

namespace JMovies.IMDb.Entities.PrivateAPI.Production
{
    /// <summary>
    /// Class Definition of a private Production AKAs Edge
    /// </summary>
    public class AKAsEdge
    {
        /// <summary>
        /// Node property
        /// </summary>
        [JsonPropertyName("node")]
        public AKANode AKANode { get; set; }
    }
}
