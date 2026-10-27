using TEdit.ViewModel;
using System.Windows;

namespace TEdit.Editor.Plugins
{
    public class ImageToPixelartEditor : BasePlugin
    {
        private ImageToPixelartEditorView _view;

        // Constructor for ImageToPixelartEditor.
        public ImageToPixelartEditor(WorldViewModel worldViewModel) : base(worldViewModel)
        {
            Name = "Image To Pixelart Editor";
        }

        public override void Execute()
        {
            if (_view != null)
            {
                _view.Activate();
                return;
            }

            _view = new ImageToPixelartEditorView(_wvm);
            _view.Owner = Application.Current.MainWindow;
            _view.Closed += (_, _) => _view = null;
            _view.Show();
        }
    }
}
