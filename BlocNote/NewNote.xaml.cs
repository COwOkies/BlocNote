
using BlocNoteLib;
namespace BlocNote;

public partial class NewNote : ContentPage
{
    public Mgr Mgr => (Application.Current as App).Manager;

    private string noteTitle = "";
    public string NoteTitle
    {
        get => noteTitle;
        set
        {
            if (value == null)
                noteTitle = "";
            else
                noteTitle = value;

        }
    }
    private string noteText = "";
    public string NoteText
    {
        get => noteText;
        set
        {
            if (value == null)
                noteText = "";
            else
                noteText = value;

        }
    }

    public NewNote()
	{
		InitializeComponent();
        BindingContext = this;
	}

    async void AddNote(object sender, EventArgs e)
    {
        if (Mgr.notes.Count > 0)
            Mgr.notes.Insert(0,new Note(noteTitle, noteText));
        else
            Mgr.notes.Add(new Note(noteTitle, noteText));

        MessagingCenter.Send(this, "SaveAndFilter");

        await Navigation.PopAsync();
    }     
}