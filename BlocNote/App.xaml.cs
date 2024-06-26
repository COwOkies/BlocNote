
using BlocNoteLib;
namespace BlocNote;

public partial class App : Application
{
    public Mgr Manager { get; set; } = new Mgr();

    public App()
    {
        InitializeComponent();
        Manager.Notes = Manager.LoadNotes();
        (string, string) creds = Manager.LoadCreds();
        Manager.Creds = new Credentials(creds.Item1,creds.Item2);
        MainPage = new AppShell();
    }
}
