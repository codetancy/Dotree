using System.IO;

namespace Dotree.Extensions;

public static class FileExtensions
{
    public static bool IsDirectory(this FileInfo fileInfo)
    {
        return fileInfo.Attributes.HasFlag(FileAttributes.Directory);
    }

    public static bool IsSymLink(this FileInfo fileInfo)
    {
        return fileInfo.Attributes.HasFlag(FileAttributes.ReparsePoint);
    }
}