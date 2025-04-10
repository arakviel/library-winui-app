using System;
using System.Collections.Generic;
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

    public Book(string title, string author, string category, string description = "", string? imagePath = null)
    {
        Title = title;
        Author = author;
        Category = category;
        Description = description;
        ImagePath = imagePath;

        if (!IsValid()) throw new ValidationException(Errors);
    }

    private void ValidateTitle()
    {
        Errors.Remove(nameof(Title));
        Errors[nameof(Title)] = new List<string>();

        if (string.IsNullOrWhiteSpace(_title))
        {
            Errors[nameof(Title)].Add("Назва книги є обов'язковою.");
        }
        if (!Regex.IsMatch(_author, @"^[а-яА-ЯіІїЇєЄґҐ\s]+$"))
        {
            Errors[nameof(Title)].Add("Назва книги повинен містити лише кириличні символи.");
        }
        if (_title.Length > MaxTitleLength)
        {
            Errors[nameof(Title)].Add($"Назва книги не повинна перевищувати {MaxTitleLength} символів.");
        }
    }

    private void ValidateAuthor()
    {
        Errors.Remove(nameof(Author));
        Errors[nameof(Author)] = new List<string>();

        if (string.IsNullOrWhiteSpace(_author))
        {
            Errors[nameof(Author)].Add("Автор книги є обов'язковим.");
        }
        if (!Regex.IsMatch(_author, @"^[а-яА-ЯіІїЇєЄґҐ\s]+$"))
        {
            Errors[nameof(Author)].Add("Автор книги повинен містити лише кириличні символи.");
        }
        var authorWords = _author.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (authorWords.Length < 2 || authorWords.Length > 3)
        {
            Errors[nameof(Author)].Add("Автор книги повинен містити від 2 до 3 слів.");
        }
        if (_author.Length > MaxAuthorLength)
        {
            Errors[nameof(Author)].Add($"Автор книги не повинен перевищувати {MaxAuthorLength} символів.");
        }
    }

    private void ValidateCategory()
    {
        Errors.Remove(nameof(Category));
        Errors[nameof(Category)] = new List<string>();

        if (string.IsNullOrWhiteSpace(_category))
        {
            Errors[nameof(Category)].Add("Категорія книги є обов'язковою.");
        }
    }

    public bool IsValid()
    {
        ValidateTitle();
        ValidateAuthor();
        ValidateCategory();
        RemoveEmptyListOfErrors();

        return Errors.Count == 0;
    }

    private void RemoveEmptyListOfErrors()
    {
        var keysToRemove = new List<string>();
        foreach (var key in Errors.Keys)
        {
            if (Errors[key].Count == 0)
            {
                keysToRemove.Add(key);
            }
        }

        foreach (var key in keysToRemove)
        {
            Errors.Remove(key);
        }
    }
}