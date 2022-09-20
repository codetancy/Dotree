using System;
using Dotree.Extensions;

namespace Dotree;

public readonly struct MemorySize
{
    public decimal Size { get; init; }
    public MemoryUnit Unit { get; init; }

    public override string ToString() =>
        Unit switch
        {
            MemoryUnit.B => $"{Size:F0}{Unit.ToName()}",
            MemoryUnit.KB or MemoryUnit.MB or MemoryUnit.GB => $"{Size:F2}{Unit.ToName()}",
            _ => throw new ArgumentOutOfRangeException()
        };

    public static MemorySize FromBytes(long length)
    {
        var unit = length switch
        {
            < 1 << 10 => MemoryUnit.B,
            < 1 << 20 => MemoryUnit.KB,
            < 1 << 30 => MemoryUnit.MB,
            _ => MemoryUnit.GB,
        };

        return new MemorySize
        {
            Size = (decimal)length / (1 << (int)unit * 10),
            Unit = unit,
        };
    }
}