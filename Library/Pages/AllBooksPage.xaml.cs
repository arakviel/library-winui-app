using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Library.Entities;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Library.Pages;

public sealed partial class AllBooksPage : Page, INotifyPropertyChanged
{
    public ObservableCollection<Book> Books { get; } = new ObservableCollection<Book>();

    private string _searchText = string.Empty;

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
        }
    }

    public AllBooksPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        // Шлях до спільної картинки в Assets
        const string sharedImagePath = "ms-appx:///Assets/book_cover.jpg";

        // Список реальних українських книг
        var booksToAdd = new[]
        {
            new Book(
                title: "Тигролови",
                author: "Іван Багряний",
                category: "Пригодницький роман",
                description: "Роман про боротьбу українського емігранта Григорія Многогрішного за свободу в умовах сталінських репресій.",
                imagePath: sharedImagePath
            ),
            new Book(
                title: "Лісова пісня",
                author: "Леся Українка",
                category: "Драма",
                description: "Поетична драма-феєрія про любов між людиною та лісовими духами, що розкриває красу природи та людських почуттів.",
                imagePath: sharedImagePath
            ),
            new Book(
                title: "Чорна рада",
                author: "Пантелеймон Куліш",
                category: "Історичний роман",
                description: "Перший український історичний роман, що змальовує козацьку добу та боротьбу за владу після смерті Хмельницького.",
                imagePath: sharedImagePath
            ),
            new Book(
                title: "Кайдашева сімя",
                author: "Іван Нечуй-Левицький",
                category: "Повість",
                description: "Сатирична повість про життя української селянської родини, їхні конфлікти та побут.",
                imagePath: sharedImagePath
            ),
            new Book(
                title: "Захар Беркут",
                author: "Іван Франко",
                category: "Історична повість",
                description: "Повість про боротьбу карпатської громади проти монгольської навали в XIII столітті.",
                imagePath: sharedImagePath
            ),
            new Book(
                title: "Місто",
                author: "Валерян Підмогильний",
                category: "Роман",
                description: "Психологічний роман про молодого українця, який намагається знайти себе в міському середовищі 1920-х років.",
                imagePath: sharedImagePath
            ),
            new Book(
                title: "Собор",
                author: "Олесь Гончар",
                category: "Роман",
                description: "Роман про духовність, екологію та боротьбу за збереження культурної спадщини в радянські часи.",
                imagePath: sharedImagePath
            ),
            new Book(
                title: "Марія",
                author: "Улас Самчук",
                category: "Роман",
                description: "Епічний роман про життя української селянки Марії на тлі історичних потрясінь XX століття.",
                imagePath: sharedImagePath
            ),
            new Book(
                title: "Енеїда",
                author: "Іван Котляревський",
                category: "Бурлескна поема",
                description: "Весела переробка класичної поеми Вергілія, що зображує козацьке життя XVIII століття.",
                imagePath: sharedImagePath
            ),
            new Book(
                title: "Хіба ревуть воли, як ясла повні",
                author: "Панас Мирний",
                category: "Соціальний роман",
                description: "Роман про долю селянина Чіпки, який через несправедливість стає розбійником.",
                imagePath: sharedImagePath
            )
        };

        foreach (var book in booksToAdd)
        {
            Books.Add(book);
        }

        // Якщо передано нову книгу через параметри навігації
        if (e.Parameter is Book newBook)
        {
            Books.Add(newBook);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Реалізуй це так, щоб воно відновлювало список коли очистити пошук
    private void SearchButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var _books = new List<Book>(Books);
        if (string.IsNullOrWhiteSpace(SearchText) && SearchText.Length <= 3)
        {

        }

        var _books = new List<Book>(Books);
        Books.Clear();

        foreach (var book in _books)
        {
            if (book.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                book.Author.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
            {
                Books.Add(book);
            }
            else
            {
                Books.Remove(book);
            }
        }
    }
}