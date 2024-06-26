using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

public class DiscordDownload
{
    private static string _token;
    private static ulong _channelId;
    private static DiscordSocketClient _client;
    private static readonly HttpClient _httpClient = new HttpClient();

    public DiscordDownload(string Dtoken, string id)
    {
        _token = Dtoken;
        _channelId = ulong.Parse(id);
    }

    public DiscordDownload(string Dtoken, ulong id)
    {
        _token = Dtoken;
        _channelId = id;
    }

    private static Task LogAsync(LogMessage log)
    {
        Console.WriteLine(log.ToString());
        return Task.CompletedTask;
    }

    public async Task DownloadFile()
    {
        _client = new DiscordSocketClient();
        
        _client.Log += LogAsync;
        _client.Ready += ReadyAsync;

        await _client.LoginAsync(TokenType.Bot, _token);
        await _client.StartAsync();

        await Task.Delay(5000);

        await DownloadFileAndDisconnectAsync();
    }

    private async Task ReadyAsync()
    {
        Debug.WriteLine($"{_client.CurrentUser} is connected!");
    }

    private async Task DownloadFileAndDisconnectAsync()
    {
        try
        {
            var channel = _client.GetChannel(_channelId) as IMessageChannel;

            if (channel != null)
            {
                var messages = await channel.GetMessagesAsync(limit: 100).FlattenAsync();
                var latestMessageWithAttachment = messages.FirstOrDefault(m => m.Attachments.Any());

                if (latestMessageWithAttachment != null)
                {
                    var attachment = latestMessageWithAttachment.Attachments.First();
                    await DownloadFileAsync(attachment.Url, attachment.Filename);
                }
                else
                {
                    Debug.WriteLine("No messages with attachments found.");
                }
            }
            else
            {
                Debug.WriteLine("Channel not found or is not an IMessageChannel.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error during file download: {ex.Message}");
        }
        finally
        {
            await _client.LogoutAsync();
            await _client.StopAsync();
            _client.Dispose();
            Debug.WriteLine("Disconnected from Discord.");
        }
    }

    private async Task DownloadFileAsync(string fileUrl, string filename)
    {
        var response = await _httpClient.GetAsync(fileUrl);

        if (response.IsSuccessStatusCode)
        {
            var filePath = "C:/Users/Cookie/source/repos/BlocNoteSol/SavedNote.xml";
            await using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await response.Content.CopyToAsync(fs);
            Debug.WriteLine($"File downloaded: {filePath}");
        }
        else
        {
            Debug.WriteLine("Failed to download the file.");
        }
    }

    public static async Task Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: DiscordDownload <token> <channelId>");
            return;
        }

        var token = args[0];
        var channelId = args[1];

        var discordDownload = new DiscordDownload(token, channelId);
        await discordDownload.DownloadFile();
    }
}
