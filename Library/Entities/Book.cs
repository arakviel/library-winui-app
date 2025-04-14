using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Library.Pages;

namespace Library.Entities;

public class Book
{
    public Dictionary<string, List<string>> Errors { get; } = new Dictionary<string, List<string>>();

    public Guid Id { get; set; } = Guid.NewGuid();
    private string _title = string.Empty;

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            ValidateTitle();
        }
    }

    private string _author = string.Empty;

    public string Author
    {
        get => _author;
        set
        {
            _author = value;
            ValidateAuthor();
        }
    }

    private string _category = string.Empty;

    public string Category
    {
        get => _category;
        set
        {
            _category = value;
            ValidateCategory();
        }
    }

    public string Description { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public const int MaxTitleLength = 128;
    public const int MaxAuthorLength = 256;

    public Book() { }
    public Book(string title, string author, string category, string description = "", string? imagePath = null)
    {
        Title = title;
        Author = author;
        Category = category;
        Description = description;
        ImagePath = imagePath;

        if (!IsValid()) throw new ValidationException(Errors);
    }

    private void ValidateTitle([CallerMemberName] string? propertyName = null)
    {
        Errors.Remove(propertyName);
        Errors[propertyName] = new List<string>();

        if (string.IsNullOrWhiteSpace(_title))
            Errors[propertyName].Add("Назва книги є обов'язковою.");
        if (!Regex.IsMatch(_title, @"^[а-яА-ЯіІїЇєЄґҐ,\-\s]+$"))
            Errors[propertyName].Add("Назва книги повинен містити лише кириличні символи.");
        if (_title.Length > MaxTitleLength)
            Errors[propertyName].Add($"Назва книги не повинна перевищувати {MaxTitleLength} символів.");

        if (Errors[propertyName].Count == 0) Errors.Remove(propertyName);
    }

    private void ValidateAuthor()
    {
        const string AuthorPropertyName = nameof(Author);

        Errors.Remove(AuthorPropertyName);
        Errors[AuthorPropertyName] = new List<string>();

        if (string.IsNullOrWhiteSpace(_author))
            Errors[AuthorPropertyName].Add("Автор книги є обов'язковим.");
        if (!Regex.IsMatch(_author, @"^[а-яА-ЯіІїЇєЄґҐ\-\s]+$"))
            Errors[AuthorPropertyName].Add("Автор книги повинен містити лише кириличні символи.");
        var authorWords = _author.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (authorWords.Length < 2 || authorWords.Length > 3)
            Errors[AuthorPropertyName].Add("Автор книги повинен містити від 2 до 3 слів.");
        if (_author.Length > MaxAuthorLength)
            Errors[AuthorPropertyName].Add($"Автор книги не повинен перевищувати {MaxAuthorLength} символів.");

        if (Errors[AuthorPropertyName].Count == 0) Errors.Remove(AuthorPropertyName);
    }

    private void ValidateCategory()
    {
        const string CategoryPropertyName = nameof(Category);

        Errors.Remove(CategoryPropertyName);
        Errors[CategoryPropertyName] = new List<string>();

        if (string.IsNullOrWhiteSpace(_category)) Errors[nameof(Category)].Add("Категорія книги є обов'язковою.");

        if (Errors[CategoryPropertyName].Count == 0) Errors.Remove(CategoryPropertyName);
    }

    public bool IsValid()
    {
        ValidateTitle();
        ValidateAuthor();
        ValidateCategory();

        return Errors.Count == 0;
    }
}