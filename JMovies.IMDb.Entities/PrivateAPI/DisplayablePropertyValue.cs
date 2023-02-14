using System.Text.Json.Serialization;

namespace JMovies.IMDb.Entities.PrivateAPI
{
    /// <summary>
    /// Class Definition of a private Displayable Property Value
    /// </summary>
    public class DisplayablePropertyValue
    {
        /// <summary>
        /// Plain Text Value of a displayable property
        /// </summary>
        [JsonPropertyName("plainText")]
        public string PlainText { get; set; }
    }
}
