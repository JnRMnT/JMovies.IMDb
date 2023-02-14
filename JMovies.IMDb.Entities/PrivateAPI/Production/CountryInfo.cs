using System.Text.Json.Serialization;

namespace JMovies.IMDb.Entities.PrivateAPI.Production
{
    /// <summary>
    /// Class Definition of a private API Country Info
    /// </summary>
    public class CountryInfo
    {
        /// <summary>
        /// Identifier of the country
        /// </summary>
        [JsonPropertyName("id")]
        public string ID { get; set; }

        /// <summary>
        /// Description/Title of the country
        /// </summary>
        [JsonPropertyName("text")]
        public string Description { get; set; }
    }
}
