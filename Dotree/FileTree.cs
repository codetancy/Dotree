using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dotree.Extensions;
using static Dotree.Constants;

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

    public void Display()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{Root.FileName} ({Root.Size})");
        ConstructBranch(Root, sb, basePrefix: "");
        Console.Write(sb);
    }

    private void ConstructBranch(TreeNode node, StringBuilder sb, string basePrefix)
    {
        for (int i = 0; i < node.Children.Count; i++)
        {
            bool isLast = (i == node.Children.Count - 1);
            
            var child = node.Children[i];
            var filePrefix = !isLast 
                ? $"{basePrefix}{SidewayT}{Horizontal}{Spacing}"
                : $"{basePrefix}{BottomL}{Horizontal}{Spacing}";
            
            sb.AppendLine($"{filePrefix}{child.FileName} ({child.Size})");
        
            if (child.FileType == FileType.Dir)
            {
                var dirPrefix = !isLast
                    ? $"{basePrefix}{Vertical} {Spacing}"
                    : $"{basePrefix}{Spacing}{Spacing}";
                ConstructBranch(child, sb, basePrefix: dirPrefix);
            }
        }
    }
}