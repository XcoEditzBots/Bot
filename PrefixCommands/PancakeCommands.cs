using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace PancakeBot.PrefixCommands
{
    public class PancakeCommands : BaseCommandModule
    {
        [Command("botmaker")]
        [Cooldown(5, 10, CooldownBucketType.User)]
        public async Task TestCommand(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("XcoEditz#5788");
        }

        [Command("pancakegame")]
        public async Task SimpleCardGame(CommandContext ctx)
        {
            var UserCard = new External_Classes.CardBuilder(); //Creating an instance of a card for the user

            var userCardMessage = new DiscordMessageBuilder() //Displaying the User's card in an embed
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.Azure)
                .WithTitle("Your Card")
                .WithDescription("You drew a: " + UserCard.SelectedCard)
                );

            await ctx.Channel.SendMessageAsync(userCardMessage);

            var BotCard = new External_Classes.CardBuilder(); //Creating an instance of a card for the Bot

            var botCardMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.Azure)
                .WithTitle("Bot Card")
                .WithDescription("The Bot drew a: " + BotCard.SelectedCard)
                );

            await ctx.Channel.SendMessageAsync(botCardMessage);

            if (UserCard.SelectedNumber > BotCard.SelectedNumber) //Comparing the two cards
            {
                //The user wins
                var winningMessage = new DiscordEmbedBuilder()
                {
                    Title = "**You Win the game!!**",
                    Color = DiscordColor.Green
                };

                await ctx.Channel.SendMessageAsync(embed: winningMessage);
                return;
            }
            else
            {
                //The bot wins
                var losingMessage = new DiscordEmbedBuilder()
                {
                    Title = "**You Lost the game**",
                    Color = DiscordColor.Red
                };

                await ctx.Channel.SendMessageAsync(embed: losingMessage);
                return;
            }
        }

        [Command("pancake")]
        [Cooldown(5, 10, CooldownBucketType.User)]
        public async Task PancakeCommand(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("https://tenor.com/view/pancakes-pancake-syrup-breakfast-brunch-gif-17577274");
        }


    }
}
