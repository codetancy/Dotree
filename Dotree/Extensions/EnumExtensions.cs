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

    public static string Color(this FileType fileType) =>
        fileType switch
        {
            FileType.Dir => "yellow",
            FileType.File => "white",
            FileType.SymLink => "green",
            _ => throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null)
        };
}