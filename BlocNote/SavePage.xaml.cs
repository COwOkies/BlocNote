using BlocNoteLib;
using System.ComponentModel;
using System.Diagnostics;

namespace BlocNote;

public partial class SavePage : ContentPage
{
    public Mgr Mgr => (Application.Current as App).Manager;

    private string token = "";
	public string Token { get => token; set => token = value; }

	private string channelID = "";
	public string ChannelID { get => channelID; set => channelID = value; }

	public SavePage()
	{
        InitializeComponent();
		Token = Mgr.Creds.discordToken;
		ChannelID = Mgr.Creds.channelID;
        BindingContext = this;
	}

	private async void SaveCreds(object sender, EventArgs e)
	{
        Mgr.Creds = new Credentials(Token, ChannelID);
        Mgr.SaveCreds();
        await DisplayAlert("Done", "Credentials saved successfully", "Ok");
    }

    async private void Upload(object sender, EventArgs e)
    {
        URunning.IsRunning = true;
        var discordUpload = new DiscordUpload(Mgr.Creds.DiscordToken, Mgr.Creds.channelID);
        try
        {
            await DiscordUpload.UploadFile();
            URunning.IsRunning = false;
            await DisplayAlert("Success", "File uploaded to Discord successfully.", "OK");
        }
        catch (Exception ex)
        {
            URunning.IsRunning = false;
            await DisplayAlert("Error", $"Failed to upload file: {ex.Message}", "OK");
        }
    }

    async private void Download(object sender, EventArgs e)
    {
        DRunning.IsRunning = true;
        var discordDownload = new DiscordDownload(Mgr.Creds.DiscordToken, Mgr.Creds.channelID);
        try
        {
            await discordDownload.DownloadFile();
            DRunning.IsRunning = false;
            await DisplayAlert("Success", "File downloaded from Discord successfully.", "OK");
            Mgr.Notes = Mgr.LoadSavedNotes();
            MessagingCenter.Send(this, "SaveAndFilter");
        }
        catch (Exception ex)
        {
            DRunning.IsRunning = false;
            await DisplayAlert("Error", $"Failed to download file: {ex.Message}", "OK");
        }
    }

}