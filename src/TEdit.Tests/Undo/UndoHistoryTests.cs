using TEdit.Editor.Undo;
using TEdit.Tests.Scripting;
using TEdit.Terraria;
using Xunit;

namespace TEdit.Editor.Undo.Tests;

public sealed class UndoHistoryTests
{
    [Fact]
    public void HistoryEntries_AreOrderedFromInitialStateToLatestEdit()
    {
        using var undo = CreateTwoEditHistory(out _);

        Assert.Equal(new[] { 0, 1, 2 }, undo.HistoryEntries.Select(entry => entry.Index));
        Assert.Equal(new[] { "Before first edit", "After edit 1", "After edit 2" }, undo.HistoryEntries.Select(entry => entry.Label));
        Assert.Equal(2, undo.CurrentIndex);
    }

    [Fact]
    public void SelectHistoryIndex_UndoesAndRedoesToSelectedState()
    {
        using var undo = CreateTwoEditHistory(out var world);

        undo.SelectHistoryIndex(1);
        Assert.Equal((ushort)1, world.Tiles[10, 10].Type);
        Assert.Equal(1, undo.CurrentIndex);

        undo.SelectHistoryIndex(0);
        Assert.False(world.Tiles[10, 10].IsActive);
        Assert.Equal(0, undo.CurrentIndex);

        undo.SelectHistoryIndex(2);
        Assert.Equal((ushort)2, world.Tiles[10, 10].Type);
        Assert.Equal(2, undo.CurrentIndex);
    }

    [Fact]
    public void EmptyHistory_HasNoEntriesAndCanSelectInitialState()
    {
        var world = TestWorldFactory.CreateSmallWorld();
        using var undo = new UndoManager(world, notifyTileChanged: null, undoApplied: () => { });

        Assert.Empty(undo.HistoryEntries);
        Assert.Equal(0, undo.HistoryCount);
        Assert.Empty(undo.SelectHistoryIndex(0));
        Assert.Equal(0, undo.CurrentIndex);
    }

    private static UndoManager CreateTwoEditHistory(out World world)
    {
        world = TestWorldFactory.CreateSmallWorld();
        var undo = new UndoManager(world, notifyTileChanged: null, undoApplied: () => { });
        undo.SaveTile(10, 10);
        world.Tiles[10, 10] = new Tile { IsActive = true, Type = 1 };
        undo.SaveUndo();

        undo.SaveTile(10, 10);
        Assert.False(undo.Buffer.IsEmpty);
        world.Tiles[10, 10] = new Tile { IsActive = true, Type = 2 };
        undo.SaveUndo();
        return undo;
    }
}
