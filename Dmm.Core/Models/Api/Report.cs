using System.Text.Json.Serialization;

namespace Dmm.Core.Models.Api;

public class Report
{
    [JsonPropertyName("result")]
    public bool Result { get; set; }
}