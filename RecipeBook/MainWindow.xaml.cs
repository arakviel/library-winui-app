using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

namespace RecipeBook;
public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
    }

    private void MainNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is NavigationViewItem navigationViewItem)
        {
            string pageName = navigationViewItem.Tag.ToString()!;
            Type pageType = Type.GetType($"RecipeBook.Pages.{pageName}")!;
            ContentFrame.Navigate(pageType, null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });

            sender.Header = navigationViewItem.Content.ToString();
        }
    }
}
