using BotManager.Discord.Expressions;
using BotManager.Runtime;
using Discord;

namespace BotManager.Discord.Models;

/// <summary>
/// Returns a Discord embed message object. Embeds are formatted messages with title, description, a preview image,
/// author and footer. 
/// This can be used in <see cref="DiscordSend"/> and <see cref="DiscordRespond"/>.
/// </summary>
[Serializable]
public sealed class DiscordEmbed
{
    /// <summary>
    /// Gets and sets the expression to resolve the title.
    /// </summary>
    public IExpression? Title { get; set; }
    
    /// <summary>
    /// Gets and sets the expression to resolve the description.
    /// </summary>
    public IExpression? Description { get; set; }
    
    /// <summary>
    /// Gets and sets the expression to resolve the target url.
    /// </summary>
    public IExpression? Url { get; set; }
    
    /// <summary>
    /// Gets and sets the expression to resolve the image url.
    /// </summary>
    public IExpression? ImageUrl { get; set; }
    
    /// <summary>
    /// Gets and sets the expression to resolve the thumbnail url.
    /// </summary>
    public IExpression? ThumbnailUrl { get; set; }
    
    /// <summary>
    /// Gets and sets the expression to resolve the author name.
    /// </summary>
    public IExpression? Author { get; set; }
    
    /// <summary>
    /// Gets and sets the expression to resolve the footer text.
    /// </summary>
    public IExpression? Footer { get; set; }

    /// <summary>
    /// Builds the <see cref="Embed"/> object.
    /// </summary>
    /// <param name="context">The runtime context.</param>
    /// <returns></returns>
    public async Task<Embed> BuildAsync(RuntimeContext context)
    {
        var builder = new EmbedBuilder();

        if (Title is not null)
            builder.Title = await context.ExecuteAsync<string>(Title);
        if (Description is not null)
            builder.Description = await context.ExecuteAsync<string>(Description);

        if (Url is not null)
            builder.Url = await context.ExecuteAsync<string>(Url);
        if (ImageUrl is not null)
            builder.ImageUrl = await context.ExecuteAsync<string>(ImageUrl);
        if (ThumbnailUrl is not null)
            builder.ThumbnailUrl = await context.ExecuteAsync<string>(ThumbnailUrl);
        
        if (Author is not null)
        {
            builder.Author = new EmbedAuthorBuilder
            {
                Name = await context.ExecuteAsync<string>(Footer)
            };
        }
        
        if (Footer is not null)
        {
            builder.Footer = new EmbedFooterBuilder
            {
                Text = await context.ExecuteAsync<string>(Footer)
            };
        }
        
        return builder.Build();
    }
}