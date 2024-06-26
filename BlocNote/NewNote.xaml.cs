using BlocNoteLib;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
namespace BlocNote;

public partial class NewNote : ContentPage
{
    public Mgr Mgr => (Microsoft.Maui.Controls.Application.Current as App).Manager;

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

    private string color = "Black";
    public string Color
    {
        get => color;
        set => color = value;
    }

    public ObservableCollection<string> Pictures { get; set; }

    public NewNote()
	{
		InitializeComponent();
        Pictures = new ObservableCollection<string>();
        BindingContext = this;
	}

    async void AddNote(object sender, EventArgs e)
    {
        if (Mgr.notes.Count > 0)
            Mgr.notes.Insert(0,new Note(noteTitle, noteText, color));
        else
            Mgr.notes.Add(new Note(noteTitle, noteText, color));

        MessagingCenter.Send(this, "SaveAndFilter");

        await Navigation.PopAsync();
    }  
}