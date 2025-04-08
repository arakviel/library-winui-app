using Microsoft.UI.Xaml;


namespace RecipeBook;

public partial class App : Application
{
    public App()
    {
        this.InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        window = new MainWindow();
        window.Activate();
        window.ExtendsContentIntoTitleBar = true;
    }

    private Window? window;
}
