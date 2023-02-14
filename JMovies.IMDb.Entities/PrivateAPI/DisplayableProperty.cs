using System.Text.Json.Serialization;

namespace JMovies.IMDb.Entities.PrivateAPI
{
    /// <summary>
    /// Class Definition of a private Displayable Property
    /// </summary>
    public class DisplayableProperty
    {
        /// <summary>
        /// Value of a displayable property
        /// </summary>
        [JsonPropertyName("value")]
        public DisplayablePropertyValue Value { get; set; }
    }
}
