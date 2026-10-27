using TEdit.Editor;
using TEdit.Geometry;
using TEdit.Terraria;
using Shouldly;
using Xunit;

namespace TEdit.Tests.Editor;

public class GridSnapSettingsTests
{
    [Fact]
    public void DefaultGridLeavesCoordinatesUnchanged()
    {
        var settings = new GridSnapSettings();

        settings.Snap(new Vector2Int32(7, 9)).ShouldBe(new Vector2Int32(7, 9));
    }

    [Fact]
    public void SnapsEachAxisUsingItsOwnSizeAndOffset()
    {
        var settings = new GridSnapSettings { Width = 4, Height = 3, OffsetX = 1, OffsetY = 2 };

        settings.Snap(new Vector2Int32(10, 8)).ShouldBe(new Vector2Int32(9, 8));
    }

    [Fact]
    public void NegativeCoordinatesSnapTowardThePreviousGridLine()
    {
        var settings = new GridSnapSettings { Width = 4, Height = 4, OffsetX = 1, OffsetY = 1 };

        settings.Snap(new Vector2Int32(-1, -5)).ShouldBe(new Vector2Int32(-3, -7));
    }

    [Fact]
    public void InvalidGridDimensionsAreTreatedAsOneTile()
    {
        GridSnapSettings.SnapAxis(9, 3, 0).ShouldBe(9);
    }

    [Fact]
    public void BrushGridSnappingIsDisabledByDefaultAndCanBeEnabled()
    {
        var brush = new BrushSettings { GridWidth = 4, GridHeight = 4, GridOffsetX = 1, GridOffsetY = 1 };
        var point = new Vector2Int32(6, 7);

        brush.SnapToGrid(point).ShouldBe(point);
        brush.IsGridSnapEnabled = true;
        brush.SnapToGrid(point).ShouldBe(new Vector2Int32(5, 5));
    }

    [Fact]
    public void SnappedCoordinatesOutsideWorldBoundsRemainInvalid()
    {
        var world = new World(10, 10, "Grid bounds test");
        var settings = new GridSnapSettings { Width = 4, Height = 4, OffsetX = 2, OffsetY = 2 };

        world.ValidTileLocation(settings.Snap(new Vector2Int32(0, 0))).ShouldBeFalse();
    }
}
