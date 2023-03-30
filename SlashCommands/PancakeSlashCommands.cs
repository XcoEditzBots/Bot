using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.CustomSearchAPI.v1;
using Google.Apis.Services;
using System.Diagnostics.Eventing.Reader;

namespace PancakeBot.SlashCommands
{
    public class PancakeSlashCommands: ApplicationCommandModule
    {

        [SlashCommand("caption", "Give any image a Caption")]
        public async Task CaptionCommand(InteractionContext ctx, [Option("caption", "The caption you want the image to have")] string caption,
                                                                [Option("image", "The image you want to upload")] DiscordAttachment picture)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                                                    .WithContent("..."));

            var captionMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.Azure)
                .WithFooter(caption)
                .WithImageUrl(picture.Url)
                );

            await ctx.Channel.SendMessageAsync(captionMessage);
        }

        [SlashCommand("ban", "Bans a user from the server")]
        public async Task Ban(InteractionContext ctx, [Option("user", "The user you want to ban")] DiscordUser user,
                                                     [Option("reason", "Reason for ban")] string reason = null)
        {
            await ctx.DeferAsync();

            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {
                var member = (DiscordMember)user;
                await ctx.Guild.BanMemberAsync(member, 0, reason);

                var banMessage = new DiscordEmbedBuilder()
                {
                    Title = "Banned user " + member.Username,
                    Description = "Reason: " + reason,
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(banMessage));
            }
            else
            {
                var nonAdminMessage = new DiscordEmbedBuilder()
                {
                    Title = "Access Denied",
                    Description = "You need to be an Administrator to execute this command",
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(nonAdminMessage));
            }
        }

        [SlashCommand("kick", "Kick a user from the server")]
        public async Task Kick(InteractionContext ctx, [Option("user", "The user you want to kick")] DiscordUser user)
        {
            await ctx.DeferAsync();

            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {
                var member = (DiscordMember)user;
                await member.RemoveAsync();

                var kickMessage = new DiscordEmbedBuilder()
                {
                    Title = member.Username + " got kicked from the Server",
                    Description = "Kicked by " + ctx.User.Username,
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(kickMessage));
            }
            else
            {
                var nonAdminMessage = new DiscordEmbedBuilder()
                {
                    Title = "Access Denied",
                    Description = "You need to be an Administrator to execute this command",
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(nonAdminMessage));
            }
        }

        [SlashCommand("timeout", "Timeout a user")]
        public async Task Timeout(InteractionContext ctx, [Option("user", "The user you want to timeout")] DiscordUser user,
                                                          [Option("duration", "Duration of the timeout in seconds")] long duration)
        {
            await ctx.DeferAsync();

            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {
                var timeDuration = DateTime.Now + TimeSpan.FromSeconds(duration);
                var member = (DiscordMember)user;
                await member.TimeoutAsync(timeDuration);

                var timeoutMessage = new DiscordEmbedBuilder()
                {
                    Title = member.Username + "has been timeout",
                    Description = "Duration: " + TimeSpan.FromSeconds(duration).ToString(),
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(timeoutMessage));
            }
            else
            {
                var nonAdminMessage = new DiscordEmbedBuilder()
                {
                    Title = "Access Denied",
                    Description = "You need to be an Administrator to execute this command",
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(nonAdminMessage));
            }
        }
        [SlashCommand("announce", "Make an announcement to a specific channel")]
        public async Task Announcement(InteractionContext ctx, [Option("message", "Announcement to send")] string message,
                                                               [Option("channel", "Channel to send this message to")] DiscordChannel channel,
                                                               [Option("image", "Upload an image for this announcement")] DiscordAttachment image = null,
                                                               [Option("everyone", "Pings everyone in the server")] bool pingEveryone = false)
        {
            await ctx.DeferAsync();

            if (pingEveryone == true)
            {
                var announcement = new DiscordEmbedBuilder()
                {
                    Title = "Announcement from " + ctx.User.Username,
                    Description = message,
                    Color = DiscordColor.Azure,
                    ImageUrl = image.Url,
                };
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddMention(EveryoneMention.All).WithContent("Announcement Created"));
                await channel.SendMessageAsync(embed: announcement);
            }
            if (image == null && pingEveryone == true || pingEveryone == false)
            {
                var announcement = new DiscordEmbedBuilder()
                {
                    Title = "Announcement from " + ctx.User.Username,
                    Description = message,
                    Color = DiscordColor.Azure
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Announcement Created"));
                await channel.SendMessageAsync(embed: announcement);
            }
            else
            {
                var announcement = new DiscordEmbedBuilder()
                {
                    Title = "Announcement from " + ctx.User.Username,
                    Description = message,
                    Color = DiscordColor.Azure,
                    ImageUrl = image.Url
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Announcement Created"));
                await channel.SendMessageAsync(embed: announcement);
            }
        }






        [SlashCommand("image", "Searches for images on google")]

        public async Task ImageSearch(InteractionContext ctx, [Option("search", "Your search query")] string search)
        {
            // Replace with your own Custom Search Engine ID and API Key
            string cseId = "Custom-Search-Engine-ID";
            string apiKey = "API-KEY";

            var customSearchService = new CustomSearchAPIService(new BaseClientService.Initializer
            {
                ApplicationName = "Discord Bot",
                ApiKey = apiKey,
            });

            var listRequest = customSearchService.Cse.List();
            listRequest.Cx = cseId;
            listRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;
            listRequest.Q = search;

            var searchRequest = await listRequest.ExecuteAsync();
            var results = searchRequest.Items;

            if (results == null || !results.Any())
            {
                ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("No image found"));
                return;
            }


            // Get the first result from the search and send it as a message
            var firstResult = results.First();
            await ctx.EditResponseAsync(firstResult.Link);

            if (results == null || !results.Any()) //IF THERE ARE NO RESULTS
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("No images found"));
                return;
            }
            else //SEND THE IMAGE
            {
                var firstResult = results.First();
                var imageEmbed = new DiscordEmbedBuilder()
                {
                    Title = "Image",
                    Color = DiscordColour.Azure,
                    ImageURL = firstResult.Link
                };
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(imageEmbed));
            }
        }


    }
}
