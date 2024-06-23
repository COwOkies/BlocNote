
using BlocNoteLib;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace BlocNote
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<Note> filteredNotes;
        private string searchText;

        public ObservableCollection<Note> FilteredNotes
        {
            get => filteredNotes;
            set
            {
                filteredNotes = value;
                OnPropertyChanged(nameof(FilteredNotes));
            }
        }

        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterNotes();
            }
        }

        public ICommand SearchCommand { get; set; }



        public Mgr Mgr => (Application.Current as App).Manager;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;

            FilteredNotes = new ObservableCollection<Note>();
            SearchCommand = new Command(FilterNotes);

            MessagingCenter.Subscribe<NewNote>(this, "SaveAndFilter", (sender) =>
            {
                FilterNotes();
                Mgr.SaveResult();
            });
            MessagingCenter.Subscribe<EditNote>(this, "SaveAndFilter", (sender) =>
            {
                Mgr.SaveResult();
            });

            FilterNotes();
        }

        public void FilterNotes()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredNotes = new ObservableCollection<Note>(Mgr.Notes);
            }
            else
            {
                FilteredNotes = new ObservableCollection<Note>(Mgr.Notes.Where(n => n.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase) || n.Text.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
            }
        }

        async private void NewNote(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewNote());
        }

        private void NewNoteFast(object sender, EventArgs e)
        {
            if (Mgr.notes.Count > 0)
                Mgr.notes.Insert(0, new Note("title", "text"));
            else
                Mgr.notes.Add(new Note("title", "text"));
            FilterNotes();

            Mgr.SaveResult();
        }

        async private void EditNote(object sender, EventArgs e)
        {
            var button = sender as Button;
            var note = button?.CommandParameter as Note;
            if (note != null)
            {
                int index = Mgr.Notes.IndexOf(note);
                if (index >= 0)
                {
                    await Navigation.PushAsync(new EditNote(index));
                }
            }
        }

        private void DeleteNote(object sender, EventArgs e)
        {
            var button = sender as Button;
            var note = button?.CommandParameter as Note;
            if (note != null)
            {
                int index = Mgr.Notes.IndexOf(note);
                if (index >= 0)
                {
                    Mgr.Notes.RemoveAt(index);
                }
            }
            Mgr.SaveResult();
            FilterNotes();
        }
    }

}
