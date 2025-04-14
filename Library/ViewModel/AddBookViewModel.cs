using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RecipeBook.ViewModel;

public class AddBookViewModel : INotifyPropertyChanged
{
    private string _title = string.Empty;
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }

    private string _author = string.Empty;

    public string Author
    {
        get => _author;
        set
        {
            _author = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
