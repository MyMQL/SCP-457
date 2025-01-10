using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Exiled.API.Features;
using UnityEngine;

namespace SCP457Plugin
{
    public static class FireManager
    {
        private static readonly Dictionary<Player, Timer> BurnTimers = new();

        public static void StartFireLoop(Player scp457, float burnRadius, int burnDamage, int burnDuration)
        {
            Plugin.Instance.LogDebug($"Started fire loop for SCP-457 (Player: {scp457.Nickname}).");

            Timer fireLoop = new Timer(1000) { AutoReset = true };
            fireLoop.Elapsed += (_, _) =>
            {
                foreach (var player in Player.List.Where(p => p.IsHuman && p.Role != PlayerRoles.RoleTypeId.Spectator && p != scp457))
                {
                    if (Vector3.Distance(scp457.Position, player.Position) <= burnRadius) // Poprawka: Użycie Vector3.Distance
                    {
                        ApplyBurn(player, burnDamage, burnDuration);
                        player.ShowHint("You are burning! Move away from the flames!", 1);
                    }
                }
            };
            fireLoop.Start();
        }

        private static void ApplyBurn(Player player, int damage, int duration)
        {
            if (BurnTimers.ContainsKey(player)) return;

            Plugin.Instance.LogDebug($"Player {player.Nickname} is burning for {duration} seconds.");

            Timer burnTimer = new Timer(1000) { AutoReset = true };
            burnTimer.Elapsed += (_, _) =>
            {
                if (duration > 0)
                {
                    player.Hurt(damage, "Burn");
                    duration--;
                }
                else
                {
                    burnTimer.Stop();
                    BurnTimers.Remove(player);
                }
            };
            burnTimer.Start();
            BurnTimers[player] = burnTimer;
        }

        public static void StopFireLoop()
        {
            Plugin.Instance.LogDebug("Stopped fire loop for SCP-457.");
            foreach (var timer in BurnTimers.Values)
            {
                timer.Stop();
                timer.Dispose();
            }
            BurnTimers.Clear();
        }
    }
}