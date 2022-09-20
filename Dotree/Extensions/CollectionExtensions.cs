using System.Collections.Generic;
using System.Linq;

namespace Dotree.Extensions;

public static class CollectionExtensions
{
    public static IEnumerable<(int index, T value)> Enumerate<T>(this IEnumerable<T> enumerable)
        => enumerable.Select((val, i) => (i, val));
}