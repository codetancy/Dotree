using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dotree.Extensions;
using Spectre.Console;

namespace Dotree;

public class FileTree
{
    public TreeNode Root { get; init; }

    public static FileTree Create(string path)
    {
        var fileInfo = new FileInfo(path);
        var root = fileInfo.IsDirectory()
            ? CreateDirNode(path, depth: 0)
            : CreateFileNode(path, depth: 0);

        return new FileTree
        {
            Root = root
        };
    }

    private static TreeNode CreateFileNode(string filename, int depth)
    {
        var fileInfo = new FileInfo(filename);
        return new TreeNode {
            FileName = fileInfo.Name,
            FileType = FileType.File,
            Length = fileInfo.Length,
            Size = MemorySize.FromBytes(fileInfo.Length),
            Children = new List<TreeNode>(),
            Depth = depth,
        };
    }

    private static TreeNode CreateDirNode(string root, int depth)
    {
        var children = new List<TreeNode>();
        children.AddRange(Directory.EnumerateFiles(root).Select(node => CreateFileNode(node, depth + 1)));
        children.AddRange(Directory.EnumerateDirectories(root).Select(node => CreateDirNode(node, depth + 1)));
        children.Sort((x, y) => string.Compare(x.FileName, y.FileName, StringComparison.OrdinalIgnoreCase));
        var length = children.Sum(c => c.Length);

        var fileInfo = new FileInfo(root);
        return new TreeNode()
        {
            FileName = fileInfo.Name,
            FileType = FileType.Dir,
            Length = length,
            Size = MemorySize.FromBytes(length),
            Children = children,
            Depth = depth,
        };
    }

    public void Display(TreeConfig config)
    {
        var sb = new StringBuilder();
        ConstructRow(Root, sb, basePrefix: "", config: config);
        ConstructBranch(Root, sb, basePrefix: "", config: config);
        AnsiConsole.MarkupLine(sb.ToString());
    }

    private void ConstructRow(TreeNode node, StringBuilder sb, string basePrefix, TreeConfig config)
    {
        sb.AppendLine($"{basePrefix}[{node.FileType.Color(config)}]{node.FileName}[/] ([{config.MemorySizeColor}]{node.Size}[/])");
    }

    private void ConstructBranch(TreeNode node, StringBuilder sb, string basePrefix, TreeConfig config)
    {
        for (int i = 0; i < node.Children.Count; i++)
        {
            bool isLast = (i == node.Children.Count - 1);

            var child = node.Children[i];
            var filePrefix = isLast
                ? $"{basePrefix}[{config.BoxBorderColor}]{(config.BottomL)}{config.Horizontal}[/]{config.Spacing}"
                : $"{basePrefix}[{config.BoxBorderColor}]{(config.SidewayT)}{config.Horizontal}[/]{config.Spacing}";

            ConstructRow(child, sb, basePrefix: filePrefix, config: config);

            if (child.FileType == FileType.Dir)
            {
                var dirPrefix = isLast
                    ? $"{basePrefix}{config.Spacing}{config.Spacing}"
                    : $"{basePrefix}[{config.BoxBorderColor}]{config.Vertical}[/] {config.Spacing}";
                ConstructBranch(child, sb, basePrefix: dirPrefix, config: config);
            }
        }
    }
}