using System;
using Library.Entities;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;


namespace Library.UserControls;

public sealed partial class BookCard : UserControl
{
    public Book Book
    {
        get { return (Book)GetValue(BookProperty); }
        set { SetValue(BookProperty, value); }
    }

    public static readonly DependencyProperty BookProperty =
        DependencyProperty.Register(nameof(Book), typeof(Book), typeof(BookCard), new PropertyMetadata(null, OnBookChanged));

    private static void OnBookChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is BookCard control && e.NewValue is Book book)
        {
            control.Title.Text = book.Title;
            control.Author.Text = book.Author;
            control.Category.Text = book.Category;
            control.Description.Text = book.Description;
            control.Image.Source = book.ImagePath != null
                ? new BitmapImage(new Uri(book.ImagePath))
                : null;
        }
    }

    public BookCard()
    {
        this.InitializeComponent();
    }
}
