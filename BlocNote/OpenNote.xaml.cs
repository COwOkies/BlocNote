using BlocNoteLib;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.CompilerServices;

namespace BlocNote
{
    public partial class OpenNote : ContentPage, INotifyPropertyChanged
    {
        public Mgr Mgr => (Application.Current as App).Manager;

        private int index;
        public int Index
        {
            get => index + 1;
            set
            {
                index = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentNote));
            }
        }

        public Note CurrentNote => Mgr.Notes[index];
        public string TextColor
        {
            get
            {
                if (CurrentNote.Color == "White" || CurrentNote.Color == "Yellow" || CurrentNote.Color == "Cyan")
                    return "black";
                else
                    return "white";
            }
        }

        public OpenNote(int noteIndex)
        {
            InitializeComponent();
            index = noteIndex;
            BindingContext = this;

            MessagingCenter.Subscribe<EditNote>(this, "SaveAndFilter", (sender) =>
            {
                OnPropertyChanged(nameof(Index));
                OnPropertyChanged(nameof(CurrentNote));
            });
        }

        private void Next(object sender, EventArgs e)
        {
            if (index == Mgr.Notes.Count - 1)
                index = 0;
            else
                index += 1;
            OnPropertyChanged(nameof(Index));
            OnPropertyChanged(nameof(TextColor));
            OnPropertyChanged(nameof(CurrentNote));
        }

        private void Previous(object sender, EventArgs e)
        {
            if (index == 0)
                index = Mgr.Notes.Count - 1;
            else
                index -= 1;

            OnPropertyChanged(nameof(Index));
            OnPropertyChanged(nameof(TextColor));
            OnPropertyChanged(nameof(CurrentNote));
        }

        async private void Edit(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditNote(index));
        }

        async private void Delete(object sender, EventArgs e)
        {
            bool choice = await DisplayAlert("Question", "❓ Do you want to proceed?", "✔️ Yes", "❌ No");

            if (choice)
            {
                Mgr.Notes.RemoveAt(index);
                MessagingCenter.Send(this, "SaveAndFilter");

                if (Mgr.Notes.Count == 0)
                    await Navigation.PopAsync();
                else if (index >= Mgr.Notes.Count)
                    index = Mgr.Notes.Count - 1;

                OnPropertyChanged(nameof(Index));
                OnPropertyChanged(nameof(CurrentNote));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
