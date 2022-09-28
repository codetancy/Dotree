using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Dotree.Extensions;

namespace Dotree;

public class FileTree
{
    private readonly TreeConfig _config;
    private readonly Regex _searchPattern;

    public TreeNode Root { get; init; }

    public FileTree(string root): this(root, new TreeConfig())
    {
    }

    public FileTree(string root, TreeConfig config)
    {
        _config = config;
        _searchPattern = new Regex(_config.SearchPattern, RegexOptions.Multiline);

        var fileInfo = new FileInfo(root);
        if (fileInfo.IsDirectory())
        {
            Root = CreateDirNode(fileInfo.FullName, fileInfo.Name, depth: 0);
        }
        else if (fileInfo.IsSymLink())
        {
            Root = CreateFileNode(fileInfo.Name, FileType.SymLink, length: 0, depth: 0);
        }
        else
        {
            Root = CreateFileNode(fileInfo.Name, FileType.File, fileInfo.Length, depth: 0);
        }
    }

    private TreeNode CreateFileNode(string filename, FileType fileType, long length, int depth)
        => new TreeNode {
            FileName = filename,
            FileType = fileType,
            Length = length,
            Depth = depth,
        };

    private TreeNode CreateDirNode(string path, string name, int depth)
    {
        var children = new List<TreeNode>();
        if (depth < _config.MaxDepth)
        {
            children.AddRange(
                Directory.EnumerateFileSystemEntries(path)
                    .Where(entry => _searchPattern.IsMatch(entry))
                    .Select(entry => new FileInfo(entry))
                    .Select(fileInfo =>
                    {
                        if (fileInfo.IsSymLink())
                            return CreateFileNode(fileInfo.Name, FileType.SymLink, 0, depth + 1);
                        if (fileInfo.IsDirectory())
                            return CreateDirNode(fileInfo.FullName, fileInfo.Name, depth + 1);
                        return CreateFileNode(fileInfo.Name, FileType.File, fileInfo.Length, depth + 1);
                    }));
            children.Sort((x, y) => string.Compare(x.FileName, y.FileName, StringComparison.OrdinalIgnoreCase));
        }

        return new TreeNode()
        {
            FileName = name,
            FileType = FileType.Dir,
            Length = children.Sum(c => c.Length),
            Children = children,
            Depth = depth,
        };
    }

    public override string ToString()
    {
        var sb = new StringBuilder(1024);
        ConstructRow(Root, sb);
        ConstructBranch(Root, sb);
        return sb.ToString();
    }

    private void ConstructRow(TreeNode node, StringBuilder sb, string basePrefix = "")
    {
        sb.AppendLine($"{basePrefix}[{node.FileType.Color(_config)}]{node.FileName}[/] ([{_config.MemorySizeColor}]{node.Size}[/])");
    }

    private void ConstructBranch(TreeNode node, StringBuilder sb, string basePrefix = "")
    {
        for (int i = 0; i < node.Children.Count; i++)
        {
            bool isLast = (i == node.Children.Count - 1);

            var child = node.Children[i];
            var filePrefix = isLast
                ? $"{basePrefix}[{_config.BoxBorderColor}]{_config.BottomL}{_config.Horizontal}[/] "
                : $"{basePrefix}[{_config.BoxBorderColor}]{_config.SidewayT}{_config.Horizontal}[/] ";

            ConstructRow(child, sb, basePrefix: filePrefix);

            if (child.FileType == FileType.Dir)
            {
                var dirPrefix = isLast
                    ? $"{basePrefix}  "
                    : $"{basePrefix}[{_config.BoxBorderColor}]{_config.Vertical}[/]  ";
                ConstructBranch(child, sb, basePrefix: dirPrefix);
            }
        }
    }
}