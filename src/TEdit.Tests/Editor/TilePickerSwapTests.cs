using TEdit.Editor;
using TEdit.Geometry;
using TEdit.Terraria;
using Xunit;

namespace TEdit.Tests.Editor;

public class TilePickerSwapTests
{
    [Fact]
    public void SwapTargetAndMask_SwapsTileValuesAndPreservesMaskModeAndBrushSettings()
    {
        var picker = new TilePicker { PaintMode = PaintMode.TileAndWall, Tile = 42 };
        var masks = new TileMaskSettings
        {
            TileMaskMode = MaskMode.Match,
            TileMaskValue = 7
        };
        var brush = new BrushSettings
        {
            IsLocked = false,
            Width = 9,
            Height = 5,
            Shape = BrushShape.Cross,
            Rotation = 45,
            FlipHorizontal = true,
            IsSpray = true,
            SprayDensity = 28
        };

        picker.Swap(masks);

        Assert.Equal(7, picker.Tile);
        Assert.Equal(42, masks.TileMaskValue);
        Assert.Equal(MaskMode.Match, masks.TileMaskMode);
        Assert.Equal(9, brush.Width);
        Assert.Equal(5, brush.Height);
        Assert.Equal(BrushShape.Cross, brush.Shape);
        Assert.Equal(45, brush.Rotation);
        Assert.True(brush.FlipHorizontal);
        Assert.True(brush.IsSpray);
        Assert.Equal(28, brush.SprayDensity);
    }

    [Fact]
    public void SwapTargetAndMask_WallSwapsOnlyWallValues()
    {
        var picker = new TilePicker { PaintMode = PaintMode.TileAndWall, Tile = 42, Wall = 18 };
        var masks = new TileMaskSettings
        {
            TileMaskMode = MaskMode.Match,
            TileMaskValue = 7,
            WallMaskMode = MaskMode.NotMatching,
            WallMaskValue = 4
        };

        picker.Swap(masks, wall: true);

        Assert.Equal(42, picker.Tile);
        Assert.Equal(7, masks.TileMaskValue);
        Assert.Equal(4, picker.Wall);
        Assert.Equal(18, masks.WallMaskValue);
        Assert.Equal(MaskMode.NotMatching, masks.WallMaskMode);
    }

    [Fact]
    public void SwapInLiquidMode_CyclesLiquidWithoutChangingMasks()
    {
        var picker = new TilePicker { PaintMode = PaintMode.Liquid, LiquidType = LiquidType.Water };
        var masks = new TileMaskSettings
        {
            TileMaskMode = MaskMode.Match,
            TileMaskValue = 7
        };

        picker.Swap(masks);

        Assert.Equal(LiquidType.Lava, picker.LiquidType);
        Assert.Equal(7, masks.TileMaskValue);
    }
}
