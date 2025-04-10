using System.Collections.ObjectModel;
using Library.Entities;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Library.Pages;

public sealed partial class AllBooksPage : Page
{
    private ObservableCollection<Book> Books { get; } = new ObservableCollection<Book>();

    public AllBooksPage()
    {
        this.InitializeComponent();
        // показати в ItemsView всі книги
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is Book newBook)
        {
            Books.Add(newBook);
        }
    }
}
