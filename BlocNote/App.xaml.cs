
using BlocNoteLib;
namespace BlocNote;

public partial class App : Application
{
    public Mgr Manager { get; set; } = new Mgr();

    public App()
    {
        InitializeComponent();
        Manager.Notes = Manager.LoadResult();
        MainPage = new AppShell();
    }
}
