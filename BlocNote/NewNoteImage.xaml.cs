using BlocNoteLib;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
namespace BlocNote;

public partial class NewNoteImage : ContentPage
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

    private string color = "White";
    public string Color
    {
        get => color;
        set => color = value;
    }

    public ObservableCollection<string> Pictures { get; set; }

    public NewNoteImage()
    {
        InitializeComponent();
        Pictures = new ObservableCollection<string>();
        BindingContext = this;
    }


    async private void OpenFile(object sender, EventArgs e)
    {
        var options = new PickOptions
        {
            FileTypes = FilePickerFileType.Images,
            PickerTitle = "Please select an image file"
        };
        var result = await PickAndShow(options);

        if (result != null)
        {
            Debug.WriteLine($"Picked path: {result.FullPath}");
            string destination = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "..", "..", "images", result.FileName);
            Debug.WriteLine($"Copying from {result.FullPath} to {destination}.");
            try
            {
                File.Copy(result.FullPath, destination);
                Debug.WriteLine("File copied.");
                Pictures.Add(destination);
                MyCarouselView.ItemsSource = Pictures.ToArray();
            }
            catch
            {
                Debug.WriteLine("An error occured");
                await DisplayAlert("File issue", "The file already exist", "Ok");
            }

        }
    }
    public async Task<FileResult> PickAndShow(PickOptions options)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                    result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                {
                    return result;
                }
                else
                    await DisplayAlert("Invalid file format", "The file format must be .jpg or .png", "Ok");
            }

        }
        catch
        {
            // The user canceled or something went wrong
        }

        return null;
    }

    async void AddNote(object sender, EventArgs e)
    {
        if (Mgr.notes.Count > 0)
            Mgr.notes.Insert(0, new Note(noteTitle, noteText, color));
        else
            Mgr.notes.Add(new Note(noteTitle, noteText, color));

        MessagingCenter.Send(this, "SaveAndFilter");

        await Navigation.PopAsync();
    }
}