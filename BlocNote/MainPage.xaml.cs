
using BlocNoteLib;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
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
                Mgr.SaveNotes();
            });
            MessagingCenter.Subscribe<EditNote>(this, "SaveAndFilter", (sender) =>
            {
                Mgr.SaveNotes();
            });
            MessagingCenter.Subscribe<SavePage>(this, "SaveAndFilter", (sender) =>
            {
                FilterNotes();
                Mgr.SaveNotes();
            });
            MessagingCenter.Subscribe<OpenNote>(this, "SaveAndFilter", (sender) =>
            {
                FilterNotes();
                Mgr.SaveNotes();
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

        async private void GoToSavePage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SavePage());
        }
        async private void OpenNote(object sender, EventArgs e)
        {
            Note note = (sender as Button)?.CommandParameter as Note;
            if (note != null)
            {
                
                int index = Mgr.Notes.IndexOf(note);
                if (index >= 0)
                {
                    await Navigation.PushAsync(new OpenNote(index));
                }
            }
        }

        async private void EditNote(object sender, EventArgs e)
        {
            Note note = ((MenuItem)sender)?.CommandParameter as Note;
            
            if (note != null)
            {
                int index = Mgr.Notes.IndexOf(note);
                if (index >= 0)
                {
                    await Navigation.PushAsync(new EditNote(index));
                }
            }
        }

        async private void DeleteNote(object sender, EventArgs e)
        {
            Note note = ((MenuItem)sender)?.CommandParameter as Note;

            if (note != null)
            {
                bool choice = await DisplayAlert("Question", "❓ Do you want to proceed?", "✔️ Yes", "❌ No");

                if (choice)
                {
                    int index = Mgr.Notes.IndexOf(note);
                    if (index >= 0)
                    {
                        Mgr.Notes.RemoveAt(index);
                        Mgr.SaveNotes();
                        FilterNotes();
                    }
                }
                
            }

        }
    }

}