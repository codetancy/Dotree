using System;

namespace Dotree.Extensions;

public static class EnumExtensions
{
    public static string ToName(this MemoryUnit unit) =>
        unit switch
        {
            MemoryUnit.B => "B",
            MemoryUnit.KB => "KB",
            MemoryUnit.MB => "MB",
            MemoryUnit.GB => "GB",
            _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
        };

    public static string Color(this FileType fileType, TreeConfig config) =>
        fileType switch
        {
            FileType.Dir => config.DirectoryColor,
            FileType.File => config.FileColor,
            FileType.SymLink => config.SymLinkColor,
            _ => throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null)
        };
}