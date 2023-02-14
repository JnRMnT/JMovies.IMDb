using System.Text.Json.Serialization;

namespace JMovies.IMDb.Entities.PrivateAPI
{
    /// <summary>
    /// Class Definition of a private API Response
    /// </summary>
    public class APIResponse
    {
        /// <summary>
        /// Main Data property
        /// </summary>
        [JsonPropertyName("data")]
        public APIData Data { get; set; }
    }
}
