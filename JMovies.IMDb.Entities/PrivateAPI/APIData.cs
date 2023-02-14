using System.Text.Json.Serialization;

namespace JMovies.IMDb.Entities.PrivateAPI
{
    /// <summary>
    /// Class Definition of a private API Response
    /// </summary>
    public class APIData
    {
        /// <summary>
        /// Production Data property
        /// </summary>
        [JsonPropertyName("title")]
        public ProductionData ProductionData { get; set; }
    }
}
