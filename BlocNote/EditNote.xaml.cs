using BlocNoteLib;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BlocNote;

public partial class EditNote : ContentPage, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public Mgr Mgr => (Application.Current as App).Manager;

    private string noteTitle;
    public string NoteTitle
    {
        get => noteTitle;
        set
        {
            if (value == null)
                noteTitle = "";
            else
                noteTitle = value;
            OnPropertyChanged();
        }
    }
    private string noteText;
    public string NoteText
    {
        get => noteText;
        set
        {
            if (value == null)
                noteText = "";
            else
                noteText = value;
            OnPropertyChanged();
        }
    }

    private int NoteIndex;

    public EditNote(int noteIndex)
    {
        InitializeComponent();
        BindingContext = this;
        NoteIndex = noteIndex;
        var note = (Application.Current as App).Manager.Notes[NoteIndex];
        NoteTitle = note.Title;
        NoteText = note.Text;
    }

    async void Edit(object sender, EventArgs e)
    {
        Mgr.notes[NoteIndex].Title = noteTitle;
        Mgr.notes[NoteIndex].Text = noteText;
        Mgr.notes[NoteIndex].DateUpdate = DateTime.Now;

        MessagingCenter.Send(this, "SaveAndFilter");

        await Navigation.PopAsync();
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}