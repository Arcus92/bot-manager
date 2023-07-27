using BotManager.Runtime;
using Discord;

namespace BotManager.Discord.Expressions;

/// <summary>
/// Returns a Discord embed message object. Embeds are formatted messages with title, description, a preview image,
/// author and footer. 
/// This can be used in <see cref="DiscordSend.Message"/> and <see cref="DiscordRespond.Message"/>.
/// <para>
/// Returns: <see cref="Embed"/>.
/// </para>
/// </summary>
public class DiscordEmbed : IExpression
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

    /// <inheritdoc />
    public async Task<object?> ExecuteAsync(RuntimeContext context, Type? returnType)
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