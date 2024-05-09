using System;
using System.Threading.Tasks;

using Discord;
using Discord.Webhook;

namespace Plogon;

/// <summary>
/// Responsible for sending discord webhooks
/// </summary>
public class DiscordWebhook
{
    /// <summary>
    /// Webhook client
    /// </summary>
    public DiscordWebhookClient? Client { get; }

    /// <summary>
    /// Init with webhook from env var
    /// </summary>
    public DiscordWebhook(string? url)
    {
        if (string.IsNullOrEmpty(url))
            return;

        this.Client = new DiscordWebhookClient(url);
    }

    private static DateTime GetKoreaStandardTime()
    {
        var utc = DateTime.UtcNow;
        var koreaStandardZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Seoul");
        var koreaStandardTime = TimeZoneInfo.ConvertTimeFromUtc(utc, koreaStandardZone);
        return koreaStandardTime;
    }

    /// <summary>
    /// Send a webhook
    /// </summary>
    /// <param name="color"></param>
    /// <param name="message"></param>
    /// <param name="title"></param>
    /// <param name="footer"></param>
    public async Task<ulong> Send(Color color, string message, string title, string footer)
    {
        if (this.Client == null)
            throw new Exception("Webhooks not set up");

        var embed = new EmbedBuilder()
            .WithColor(color)
            .WithTitle(title)
            .WithFooter(footer)
            .WithDescription(message)
            .Build();

        var time = GetKoreaStandardTime();
        var username = "도";
        var avatarUrl = "https://ndiv.rayd.cc/icons/do.png";
        if (time.Hour is > 20 or < 7)
        {
            username = "화";
            avatarUrl = "https://ndiv.rayd.cc/icons/hwa.png";
        }

        return await this.Client.SendMessageAsync(embeds: new[] { embed }, username: username, avatarUrl: avatarUrl);
    }
}
