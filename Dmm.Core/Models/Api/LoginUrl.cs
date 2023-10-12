using System.Text.Json.Serialization;

namespace Dmm.Core.Models.Api;

public class LoginUrl
{
    [JsonPropertyName("url")]
    public Uri? Url { get; set; }
}