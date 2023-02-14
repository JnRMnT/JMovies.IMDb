using System.Text.Json.Serialization;

namespace JMovies.IMDb.Entities.PrivateAPI.Production
{
    /// <summary>
    /// Class Definition of a private Production AKA Node
    /// </summary>
    public class AKANode
    {
        /// <summary>
        /// Country of the AKA
        /// </summary>
        [JsonPropertyName("country")]
        public CountryInfo Country { get; set; }

        /// <summary>
        /// Displayable Property of the AKA
        /// </summary>
        [JsonPropertyName("displayableProperty")]
        public DisplayableProperty DisplayableProperty { get; set; }
    }
}
