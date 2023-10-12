using System.Text.Json.Serialization;

namespace Dmm.Core.Models.Api;

public class LaunchCommand
{
    [JsonPropertyName("product_id")]
    public string? ProductId { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("exec_file_name")]
    public string? ExecFileName { get; set; }

    [JsonPropertyName("install_dir")]
    public string? InstallDir { get; set; }

    [JsonPropertyName("file_list_url")]
    public string? FileListUrl { get; set; }

    [JsonPropertyName("is_administrator")]
    public bool IsAdministrator { get; set; }

    [JsonPropertyName("file_check_type")]
    public string? FileCheckType { get; set; }

    [JsonPropertyName("total_size")]
    public long TotalSize { get; set; }

    [JsonPropertyName("latest_version")]
    public string? LatestVersion { get; set; }

    [JsonPropertyName("execute_args")]
    public string? ExecuteArgs { get; set; }

    [JsonPropertyName("conversion_url")]
    public object? ConversionUrl { get; set; }
}