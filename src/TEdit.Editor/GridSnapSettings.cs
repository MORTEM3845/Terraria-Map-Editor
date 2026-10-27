using System;
using TEdit.Geometry;

namespace TEdit.Editor;

/// <summary>Snapping configuration for painting tools. A 1x1 grid leaves input unchanged.</summary>
public sealed class GridSnapSettings
{
    public int Width { get; set; } = 1;
    public int Height { get; set; } = 1;
    public int OffsetX { get; set; }
    public int OffsetY { get; set; }

    public Vector2Int32 Snap(Vector2Int32 point)
    {
        return new Vector2Int32(SnapAxis(point.X, OffsetX, Width), SnapAxis(point.Y, OffsetY, Height));
    }

    public static int SnapAxis(int value, int offset, int size)
    {
        size = Math.Max(1, size);
        int remainder = (value - offset) % size;
        if (remainder < 0) remainder += size;
        return value - remainder;
    }
}
