using Discord;
using Discord.WebSocket;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

public class DiscordUpload
{
    public static string token;
    public static ulong channelId;

    public DiscordUpload(string Dtoken, string id)
    {
        token = Dtoken;
        channelId = ulong.Parse(id);
    }

    private static DiscordSocketClient _client;

    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private static async Task SendFileAndDisconnectAsync()
    {
        try
        {
            var channel = _client.GetChannel(channelId) as IMessageChannel;

            if (channel != null)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "..", "..", "Note.xml");
                if (File.Exists(path))
                {
                    await channel.SendFileAsync(path, "Save file");
                    Debug.WriteLine("File uploaded successfully.");
                }
                else
                    Debug.WriteLine("The file doesn't exist");
            }
            else
            {
                Debug.WriteLine("Failed to find the channel.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error during file upload: {ex.Message}");
        }
        finally
        {
            await _client.LogoutAsync();
            await _client.StopAsync();
            _client.Dispose();
            Debug.WriteLine("Disconnected from Discord.");
        }
    }

    public static async Task UploadFile()
    {
        _client = new DiscordSocketClient();

        _client.Log += Log;

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        await Task.Delay(5000);

        await SendFileAndDisconnectAsync();
    }
}
