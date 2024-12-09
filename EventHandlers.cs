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
            plugin.LogDebug("Runda rozpoczęta. Losowanie SCP-457...");

            if (plugin.Config.SpawnChance / 100 >= new System.Random().NextDouble())
            {
                plugin.LogDebug("Wybrano SCP-457. Szukanie gracza...");

                Player player = Player.List.FirstOrDefault(p => p.Role == RoleTypeId.ClassD);

                if (player != null)
                {
                    scp457Spawned = true;

                    player.Role.Set(RoleTypeId.Scp106);
                    player.Scale = new Vector3(0.85f, 1.15f, 0.85f);
                    player.Broadcast(10, "<color=red>Jesteś SCP-457! Podpalaj swoich wrogów!</color>");
                    player.Health = plugin.Config.StartHealth;

                    // Fioletowa tarcza
                    player.HumeShield = plugin.Config.StartShield;

                    Exiled.API.Features.Cassie.Message("Warning . . SCP 4 5 7 detected . .");

                    plugin.LogDebug($"Gracz {player.Nickname} został SCP-457.");
                }
                else
                {
                    plugin.LogDebug("Nie znaleziono gracza dla SCP-457.");
                }
            }
            else
            {
                plugin.LogDebug("SCP-457 nie pojawił się w tej rundzie.");
            }
        }

        public void OnPlayerSpawning(SpawningEventArgs ev)
        {
            if (scp457Spawned && ev.Player.Role == RoleTypeId.Scp106)
            {
                var scp173Room = Room.List.FirstOrDefault(r => r.Name == "HCZ_173");
                if (scp173Room != null)
                {
                    ev.Position = scp173Room.Position + new Vector3(0f, 1f, 0f);
                    plugin.LogDebug("SCP-457 zrespiony w pokoju SCP-173.");
                }
                else
                {
                    plugin.LogDebug("Nie znaleziono pokoju SCP-173. Respi SCP-457 w domyślnej pozycji SCP-106.");
                }
            }
        }

        public void OnPlayerHurting(HurtingEventArgs ev)
        {
            // logowanei obiektow
            plugin.LogDebug($"OnPlayerHurting: ev.Attacker = {(ev.Attacker != null ? ev.Attacker.Nickname : "null")}, ev.Player = {(ev.Player != null ? ev.Player.Nickname : "null")}.");

            // sprawdz, czy obiekt jest null, jezeli tak to go wypierdol
            if (ev.Attacker == null || ev.Player == null)
            {
                plugin.LogDebug("OnPlayerHurting: ev.Attacker lub ev.Player jest null. Pomijanie.");
                return;
            }

            // sprawdz czy scp nie atakuje sam siebie
            if (ev.Attacker.Role == RoleTypeId.Scp106)
            {
                ev.Amount = plugin.Config.TouchDamage;
                plugin.LogDebug($"SCP-457 zadał {ev.Amount} obrażeń graczowi {ev.Player.Nickname}.");
            }
        }

        public void OnPlayerDying(DyingEventArgs ev)
        {
            if (scp457Spawned && ev.Player.Role == RoleTypeId.Scp106)
            {
                plugin.LogDebug("SCP-457 zginął.");
            }
        }
    }
}