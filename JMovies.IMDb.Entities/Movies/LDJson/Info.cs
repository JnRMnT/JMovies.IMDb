using System.Text.Json.Serialization;

namespace JMovies.IMDb.Entities.Movies.LDJson
{
    /// <summary>
    /// Class representing a basic info linked to the production
    /// </summary>
    public class Info
    {
        /// <summary>
        /// Property representing the info type
        /// </summary>
        [JsonPropertyName("@type")]
        public string InfoType { get; set; }

        /// <summary>
        /// Property representing name related to the info
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Property representing URL related to the info
        /// </summary>
        [JsonPropertyName("url")]
        public string URL { get; set; }
    }
}
