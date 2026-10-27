using System.Globalization;
using TEdit.Editor.Plugins;
using Xunit;

namespace TEdit.Tests.Editor;

public class PluginTitleTests
{
    [Fact]
    public void Name_RemainsStableWhileTitleIsLocalized()
    {
        var plugin = new TestPlugin("BlockShuffle");
        var originalCulture = CultureInfo.CurrentUICulture;

        try
        {
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("ru-RU");

            Assert.Equal("BlockShuffle", plugin.Name);
            Assert.Equal("Перемешать расположение блоков", plugin.Title);
        }
        finally
        {
            CultureInfo.CurrentUICulture = originalCulture;
        }
    }

    [Fact]
    public void Title_FallsBackToStableNameWhenResourceIsMissing()
    {
        var plugin = new TestPlugin("MissingPlugin");

        Assert.Equal("MissingPlugin", plugin.Title);
    }

    private sealed class TestPlugin : BasePlugin
    {
        public TestPlugin(string name) : base(null!)
        {
            Name = name;
        }

        public override void Execute() { }
    }
}
