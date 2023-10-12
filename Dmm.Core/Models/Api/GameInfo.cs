using System.Text.Json.Serialization;

namespace Dmm.Core.Models.Api;

public class GameInfo
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("product_id")]
    public string? ProductId { get; set; }

    [JsonPropertyName("has_detail")]
    public bool HasDetail { get; set; }

    [JsonPropertyName("has_uninstall")]
    public bool HasUninstall { get; set; }

    [JsonPropertyName("allow_shortcut")]
    public bool AllowShortcut { get; set; }

    [JsonPropertyName("allow_visible_setting")]
    public bool AllowVisibleSetting { get; set; }

    [JsonPropertyName("registered_time")]
    public string? RegisteredTime { get; set; }

    [JsonPropertyName("last_played")]
    public string? LastPlayed { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("user_state")]
    public string? UserState { get; set; }

    [JsonPropertyName("actions")]
    public string[]? Actions { get; set; }

    [JsonPropertyName("is_show_latest_version")]
    public bool IsShowLatestVersion { get; set; }

    [JsonPropertyName("latest_version")]
    public string? LatestVersion { get; set; }

    [JsonPropertyName("is_show_file_size")]
    public bool IsShowFileSize { get; set; }

    [JsonPropertyName("file_size")]
    public long FileSize { get; set; }

    [JsonPropertyName("is_show_agreement_link")]
    public bool IsShowAgreementLink { get; set; }

    [JsonPropertyName("update_date")]
    public string? UpdateDate { get; set; }
}