using System.Text.Json.Serialization;

namespace JMovies.IMDb.Entities.Movies.LDJson
{
    /// <summary>
    /// Class representing the metadata linked to the page
    /// </summary>
    public class Metadata
    {
        /// <summary>
        /// Property representing the production's type
        /// </summary>
        [JsonPropertyName("@type")]
        public string ProductionType { get; set; }

        /// <summary>
        /// Property representing the production's description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }


        /// <summary>
        /// Property representing the production's URL
        /// </summary>
        [JsonPropertyName("url")]
        public string URL { get; set; }


        /// <summary>
        /// Property representing the actor list of the production
        /// </summary>
        [JsonPropertyName("actor")]
        public Info[] Actors { get; set; }

        /// <summary>
        /// Property representing the creator list of the production
        /// </summary>
        [JsonPropertyName("creator")]
        public Info[] Creators { get; set; }

        /// <summary>
        /// Property representing the release date of the production
        /// </summary>
        [JsonPropertyName("datePublished")]
        public string ReleaseDate { get; set; }

        /// <summary>
        /// Property representing the genre list of the production
        /// </summary>
        [JsonPropertyName("genre")]
        public string[] Genres { get; set; }


        /// <summary>
        /// Property representing the name of the production
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Property representing the keywords related to the production
        /// </summary>
        [JsonPropertyName("keywords")]
        public string Keywords { get; set; }

        /// <summary>
        /// Property representing the main image related to the production
        /// </summary>
        [JsonPropertyName("image")]
        public string Image { get; set; }

    }
}
