using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DesktopApp.ViewModels.Pages;

namespace DesktopApp.Views.Pages;

public partial class MenuAndContentPage : UserControl
{
    public MenuAndContentPage()
    {
        InitializeComponent();
    }

    private void MenuTreeView_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (DataContext is MenuAndContentPageViewModel vm && sender is TreeView treeView
            && treeView.SelectedItem is TreeViewItem item)
        {
            vm.MenuOptionClick(item.Header as string);
        }
    }
}