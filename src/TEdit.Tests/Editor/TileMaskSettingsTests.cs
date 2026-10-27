using Shouldly;
using TEdit.Editor;
using TEdit.Terraria;
using Xunit;

namespace TEdit.Tests.Editor;

public class TileMaskSettingsTests
{
    [Fact]
    public void Passes_SingleTileMask_MatchesOnlySelectedType()
    {
        var mask = new TileMaskSettings { TileMaskMode = MaskMode.Match, TileMaskValue = 0 };

        mask.Passes(ActiveTile(0)).ShouldBeTrue();
        mask.Passes(ActiveTile(1)).ShouldBeFalse();
        mask.Passes(new Tile()).ShouldBeFalse();
    }

    [Fact]
    public void Passes_MultipleTileMask_MatchesAnySelectedType()
    {
        var mask = new TileMaskSettings { TileMaskMode = MaskMode.Match, TileMaskValue = 0 };
        mask.TileMaskValues.Add(0);
        mask.TileMaskValues.Add(1);

        mask.Passes(ActiveTile(0)).ShouldBeTrue();
        mask.Passes(ActiveTile(1)).ShouldBeTrue();
        mask.Passes(ActiveTile(2)).ShouldBeFalse();
        mask.Passes(new Tile()).ShouldBeFalse();
    }

    [Fact]
    public void Passes_MultipleNotMatchingMask_RejectsEverySelectedType()
    {
        var mask = new TileMaskSettings { TileMaskMode = MaskMode.NotMatching };
        mask.TileMaskValues.Add(0);
        mask.TileMaskValues.Add(1);

        mask.Passes(ActiveTile(0)).ShouldBeFalse();
        mask.Passes(ActiveTile(1)).ShouldBeFalse();
        mask.Passes(ActiveTile(2)).ShouldBeTrue();
        mask.Passes(new Tile()).ShouldBeTrue();
    }

    [Fact]
    public void ClearAll_ClearsMultipleTileMaskValues()
    {
        var mask = new TileMaskSettings { TileMaskMode = MaskMode.Match };
        mask.TileMaskValues.Add(0);
        mask.TileMaskValues.Add(1);

        mask.ClearAll();

        mask.TileMaskValues.ShouldBeEmpty();
        mask.AllMasksOff.ShouldBeTrue();
    }

    private static Tile ActiveTile(ushort type) => new() { IsActive = true, Type = type };
}
