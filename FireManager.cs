using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Exiled.API.Features;
using PlayerRoles;
using UnityEngine;

namespace SCP457Plugin
{
    public static class FireManager
    {
        private static readonly Dictionary<Player, Timer> BurnTimers = new Dictionary<Player, Timer>();

        public static void StartFireLoop(Player scp457, float burnRadius, int burnDamage, int burnDuration)
        {
            Plugin.Instance.LogDebug($"Rozpoczęto pętlę podpaleń dla SCP-457 (Gracz: {scp457.Nickname}).");

            Timer fireLoop = new Timer(1000) { AutoReset = true };
            fireLoop.Elapsed += (sender, args) =>
            {
                foreach (var player in Player.List.Where(p => p.IsHuman && p.Role != RoleTypeId.Spectator && p != scp457))
                {
                    if (Vector3.Distance(scp457.Position, player.Position) <= burnRadius)
                    {
                        ApplyBurn(player, burnDamage, burnDuration);
                    }
                }
            };
            fireLoop.Start();
        }

        private static void ApplyBurn(Player player, int damage, int duration)
        {
            if (BurnTimers.ContainsKey(player)) return;

            Plugin.Instance.LogDebug($"Gracz {player.Nickname} został podpalony na {duration} sekund.");

            Timer burnTimer = new Timer(1000) { AutoReset = true };
            burnTimer.Elapsed += (sender, args) =>
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

        public static void StopFireLoop(Player scp457)
        {
            Plugin.Instance.LogDebug("Zatrzymano pętlę podpaleń SCP-457.");

            foreach (var player in BurnTimers.Keys.ToList())
            {
                if (BurnTimers.TryGetValue(player, out Timer timer))
                {
                    timer.Stop();
                    timer.Dispose();
                }
            }
            BurnTimers.Clear();
        }
    }
}