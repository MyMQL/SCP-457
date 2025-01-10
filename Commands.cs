using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using PlayerRoles;

namespace SCP457Plugin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Scp457Command : ICommand
    {
        private readonly Plugin plugin;

        public Scp457Command() : this(Plugin.Instance)
        {
        }

        public Scp457Command(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public string Command => "scp457";
        public string[] Aliases => Array.Empty<string>();
        public string Description => "Assigns a player as SCP-457.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("scp457.spawn"))
            {
                response = "You do not have permission to use this command.";
                return false;
            }

            if (arguments.Count < 2)
            {
                response = "Usage: scp457 spawn <player_id>";
                return false;
            }

            if (!int.TryParse(arguments.At(1), out int playerId))
            {
                response = "Invalid player ID.";
                return false;
            }

            Player player = Player.Get(playerId);
            if (player == null)
            {
                response = "Player not found.";
                return false;
            }

            player.Role.Set(RoleTypeId.Scp106);
            player.Broadcast(10, "<color=red>You are SCP-457! Ignite your enemies!</color>");
            player.Health = plugin.Config.StartHealth;

            plugin.LogDebug($"Player {player.Nickname} has been assigned as SCP-457.");
            response = $"Player {player.Nickname} is now SCP-457.";
            return true;
        }
    }
}