using System;
using System.Threading.Tasks;
using Library.Entities;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;


namespace Library.Pages;

public sealed partial class AddBookPage : Page
{
    private StorageFile selectedImageFile = null;

    public AddBookPage()
    {
        this.InitializeComponent();
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        ClearErrors();

        try
        {
            Book newBook = new Book(
                title: TitleTextBox.Text,
                author: AuthorTextBox.Text,
                category: CategoryComboBox.SelectedItem?.ToString()!,
                description: DescriptionTextBox.Text
            );

            newBook.ImagePath = SaveImageAync().Result;
            Frame.Navigate(typeof(AllBooksPage), newBook);
        }
        catch (ValidationException ex)
        {
            if (ex.Errors.ContainsKey("Title"))
                ShowError(TitleError, string.Join("\n", ex.Errors["Title"]));
            if (ex.Errors.ContainsKey("Author"))
                ShowError(AuthorError, string.Join("\n", ex.Errors["Author"]));
            if (ex.Errors.ContainsKey("Category"))
                ShowError(CategoryError, string.Join("\n", ex.Errors["Category"]));
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
            ImagePathText.Text = selectedImageFile.Name;
        }
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(AllBooksPage));
    }

    private async Task<string> SaveImageAync()
    {
        if (selectedImageFile == null) return null;

        var localFolder = ApplicationData.Current.LocalFolder;
        var newFile = await localFolder.CreateFileAsync(selectedImageFile.Name, CreationCollisionOption.ReplaceExisting);
        await selectedImageFile.CopyAndReplaceAsync(newFile);
        return newFile.Name;
    }

    private void ShowError(TextBlock errorBlock, string message)
    {
        errorBlock.Text = message;
        errorBlock.Visibility = Visibility.Visible;
    }

    private void ClearErrors()
    {
        TitleError.Visibility = Visibility.Collapsed;
        AuthorError.Visibility = Visibility.Collapsed;
        CategoryError.Visibility = Visibility.Collapsed;
    }
}
