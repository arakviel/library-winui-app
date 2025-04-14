using System;
using System.Threading.Tasks;
using Library.Entities;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using RecipeBook.ViewModel;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace Library.Pages;

// TODO: не змінюється пункт меню в головному вікні
public sealed partial class AddBookPage : Page
{
    private StorageFile? selectedImageFile;
    public AddBookViewModel ViewModel { get; } = new AddBookViewModel();

    public AddBookPage()
    {
        InitializeComponent();
    }

    private async void Save_Click(object sender, RoutedEventArgs e)
    {
        ClearErrors();

        try
        {
            Book newBook = new Book(
                title: TitleTextBox.Text,
                author: AuthorTextBox.Text,
                category: CategoryComboBox.SelectedItem?.ToString() ?? string.Empty,
                description: DescriptionTextBox.Text
            );

            newBook.ImagePath = await SaveImageAsync() ?? "ms-appx:///Assets/book_cover.jpg";
            Frame.Navigate(typeof(AllBooksPage), newBook);
        }
        catch (ValidationException ex)
        {
            if (ex.Errors.ContainsKey("Title"))
                ShowError(TitleErrorTextBlock, string.Join("\n", ex.Errors["Title"]));
            if (ex.Errors.ContainsKey("Author"))
                ShowError(AuthorErrorTextBlock, string.Join("\n", ex.Errors["Author"]));
            if (ex.Errors.ContainsKey("Category"))
                ShowError(CategoryErrorTextBlock, string.Join("\n", ex.Errors["Category"]));
        }
    }

    private async void PickImageButton_Click(object sender, RoutedEventArgs e)
    {
        var picker = new FileOpenPicker();
        picker.FileTypeFilter.Add(".jpg");
        picker.FileTypeFilter.Add(".jpeg");
        picker.FileTypeFilter.Add(".png");

        var hWnd = WindowNative.GetWindowHandle(App.MainWindow);
        InitializeWithWindow.Initialize(picker, hWnd);

        selectedImageFile = await picker.PickSingleFileAsync();
        if (selectedImageFile != null)
        {
            ImagePathTextTextBlock.Text = selectedImageFile.Name;
        }
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(AllBooksPage));
    }

    private async Task<string?> SaveImageAsync()
    {
        if (selectedImageFile == null) return null;

        try
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var newFile = await localFolder.CreateFileAsync(
                selectedImageFile.Name,
                CreationCollisionOption.GenerateUniqueName
            );
            await selectedImageFile.CopyAndReplaceAsync(newFile);
            return newFile.Path;
        }
        catch (Exception ex)
        {
            ShowError(ImagePathTextTextBlock, $"Помилка збереження зображення: {ex.Message}");
            return null;
        }
    }

    private void ShowError(TextBlock errorBlock, string message)
    {
        errorBlock.Text = message;
        errorBlock.Visibility = Visibility.Visible;
    }

    private void ClearErrors()
    {
        TitleErrorTextBlock.Visibility = Visibility.Collapsed;
        AuthorErrorTextBlock.Visibility = Visibility.Collapsed;
        CategoryErrorTextBlock.Visibility = Visibility.Collapsed;
        ImagePathTextTextBlock.Visibility = Visibility.Collapsed;
    }

    private Visibility OnTitleChanged(string title)
    {
        if (string.IsNullOrEmpty(title)) return Visibility.Collapsed;

        Book book = new Book();
        book.Title = title;
        if (book.Errors.ContainsKey("Title"))
        {
            ShowError(TitleErrorTextBlock, string.Join("\n", book.Errors["Title"]));
            return Visibility.Visible;
        }
        return Visibility.Collapsed;
    }

    private Visibility OnAuthorChanged(string author)
    {
        if (string.IsNullOrEmpty(author)) return Visibility.Collapsed;

        Book book = new Book();
        book.Author = author;
        if (book.Errors.ContainsKey("Author"))
        {
            ShowError(AuthorErrorTextBlock, string.Join("\n", book.Errors["Author"]));
            return Visibility.Visible;
        }
        return Visibility.Collapsed;
    }
}