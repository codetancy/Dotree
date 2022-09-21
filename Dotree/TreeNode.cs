using System.Collections.Generic;

namespace Dotree;

public class TreeNode
{
    public string FileName { get; init; }
    public FileType FileType { get; init; }
    public long Length { get; init; }
    public MemorySize Size { get; init; }
    public int Depth { get; init; }
    public IReadOnlyList<TreeNode> Children { get; init; } = new List<TreeNode>();
}