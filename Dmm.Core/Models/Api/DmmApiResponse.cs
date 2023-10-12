using System.Text.Json.Serialization;

namespace Dmm.Core.Models.Api;

public class DmmApiResponse<T>
{
    [JsonPropertyName("result_code")]
    public long ResultCode { get; set; }

    [JsonPropertyName("data")]
    public T? Data { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }
}