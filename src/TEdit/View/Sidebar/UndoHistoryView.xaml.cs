using System.Windows.Controls;
using TEdit.Editor.Undo;
using TEdit.ViewModel;

namespace TEdit.View.Sidebar;

public partial class UndoHistoryView : UserControl
{
    private readonly WorldViewModel _viewModel;

    public UndoHistoryView()
    {
        InitializeComponent();
        HistoryTitle.Text = TEdit.Properties.Language.ResourceManager.GetString("undo_history_title", TEdit.Properties.Language.Culture) ?? "Undo and redo history";
        EmptyHistoryText.Text = TEdit.Properties.Language.ResourceManager.GetString("undo_history_empty", TEdit.Properties.Language.Culture) ?? "No undo history yet";
        _viewModel = ViewModelLocator.WorldViewModel;
        DataContext = _viewModel;
    }

    private void HistoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (HistoryList.SelectedItem is UndoHistoryEntry entry &&
            entry.Index != _viewModel.UndoManager?.CurrentIndex)
        {
            _viewModel.SelectUndoHistoryIndex(entry.Index);
        }
    }
}
