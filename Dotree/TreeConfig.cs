using System.Text.Json.Serialization;

namespace Dotree;

public record struct TreeConfig
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

    [JsonPropertyName("sidewayTCharacter")]
    public char SidewayT { get; set; } = '\u251C';

    [JsonPropertyName("horizontalCharacter")]
    public char Horizontal { get; set; } = '\u2500';

    [JsonPropertyName("verticalCharacter")]
    public char Vertical { get; set; } = '\u2502';

    [JsonPropertyName("bottomLCharacter")]
    public char BottomL { get; set; } = '\u2514';

    [JsonPropertyName("maxDepth")]
    public int MaxDepth { get; set; } = int.MaxValue;

    [JsonPropertyName("searchPattern")]
    public string SearchPattern { get; set; } = ".*";
}