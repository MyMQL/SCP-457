using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using UnityEngine;

namespace SCP457Plugin
{
    public class EventHandlers
    {
        private readonly Plugin plugin;

        public EventHandlers(Plugin plugin) => this.plugin = plugin;

        private bool scp457Spawned;

        public void OnRoundStart()
        {
            scp457Spawned = false;
            plugin.LogDebug("Round started. Rolling for SCP-457...");

            if (plugin.Config.SpawnChance / 100 >= new System.Random().NextDouble())
            {
                plugin.LogDebug("SCP-457 selected. Searching for a player...");

                Player player = Player.List.FirstOrDefault(p => p.Role == RoleTypeId.ClassD);

                if (player != null)
                {
                    scp457Spawned = true;

                    player.Role.Set(RoleTypeId.Scp106);
                    player.Broadcast(10, "<color=red>You are SCP-457! Ignite your enemies!</color>");
                    player.Health = plugin.Config.StartHealth;

                    var spawnRoom = Room.List.FirstOrDefault(room => room.Type == plugin.Config.SpawnRoomType);

                    if (spawnRoom != null)
                    {
                        player.Position = spawnRoom.Position + new Vector3(0, 1, 0);
                        plugin.LogDebug($"Player {player.Nickname} assigned as SCP-457 and spawned in {plugin.Config.SpawnRoomType}.");
                    }
                    else
                    {
                        plugin.LogDebug($"Room of type {plugin.Config.SpawnRoomType} not found. Assigning SCP-457 to default spawn position.");
                    }
                }
                else
                {
                    plugin.LogDebug("No player found for SCP-457.");
                }
            }
            else
            {
                plugin.LogDebug("SCP-457 did not spawn this round.");
            }
        }

        public void OnPlayerHurting(HurtingEventArgs ev)
        {
            plugin.LogDebug($"OnPlayerHurting: ev.Attacker = {(ev.Attacker != null ? ev.Attacker.Nickname : "null")}, ev.Player = {(ev.Player != null ? ev.Player.Nickname : "null")}.");

            if (ev.Attacker == null || ev.Player == null)
            {
                plugin.LogDebug("OnPlayerHurting: ev.Attacker or ev.Player is null. Ignoring.");
                return;
            }

            if (ev.Attacker.Role == RoleTypeId.Scp106)
            {
                ev.Amount = plugin.Config.TouchDamage;
                ev.IsAllowed = false;
                plugin.LogDebug($"SCP-457 dealt {ev.Amount} damage to {ev.Player.Nickname}.");
            }
        }

        public void OnPlayerDying(DyingEventArgs ev)
        {
            if (scp457Spawned && ev.Player.Role == RoleTypeId.Scp106)
            {
                plugin.LogDebug("SCP-457 has died.");
            }
        }
    }
}