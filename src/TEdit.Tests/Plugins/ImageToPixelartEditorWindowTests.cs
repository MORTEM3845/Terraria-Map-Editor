using System.IO;
using System.Xml.Linq;
using Xunit;

namespace TEdit.Tests.Plugins;

public class ImageToPixelartEditorWindowTests
{
    [Fact]
    public void WindowUsesNativeTitleBarForCloseButton()
    {
        var xamlPath = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "../../../../TEdit/Editor/Plugins/ImageToPixelartEditorView.xaml"));
        var root = XDocument.Load(xamlPath).Root;

        Assert.NotNull(root);
        Assert.Equal("False", (string?)root.Attribute("ExtendsContentIntoTitleBar"));
        Assert.Null(root.Attribute("WindowBackdropType"));
    }
}
