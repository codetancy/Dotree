﻿using System.Text.Json.Serialization;

namespace Dotree;

public struct TreeConfig
{
    public TreeConfig()
    {
    }

    [JsonPropertyName("directoryColor")]
    public string DirectoryColor { get; init; } = "yellow";

    [JsonPropertyName("fileColor")]
    public string FileColor { get; init; } = "white";

    [JsonPropertyName("symLinkColor")]
    public string SymLinkColor { get; init; } = "green";

    [JsonPropertyName("memorySizeColor")]
    public string MemorySizeColor { get; init; } = "red";

    [JsonPropertyName("boxBorderColor")]
    public string BoxBorderColor { get; init; } = "aqua";

    [JsonPropertyName("spacingCharacter")]
    public string Spacing { get; set; } = "  ";

    [JsonPropertyName("sidewayTCharacter")]
    public string SidewayT { get; set; } = "\u251C";

    [JsonPropertyName("horizontalCharacter")]
    public string Horizontal { get; set; } = "\u2500";

    [JsonPropertyName("verticalCharacter")]
    public string Vertical { get; set; } = "\u2502";

    [JsonPropertyName("bottomLCharacter")]
    public string BottomL { get; set; } = "\u2514";
}