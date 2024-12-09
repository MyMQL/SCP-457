using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using PlayerRoles;
using RemoteAdmin;
using UnityEngine;

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
        public string Description => "Komendy SCP-457.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("scp457.spawn"))
            {
                response = "Brak uprawnień do użycia tej komendy.";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Użycie: scp457 <spawn/npc>";
                return false;
            }

            string subCommand = arguments.At(0).ToLower();

            switch (subCommand)
            {
                case "spawn":
                    return SpawnPlayerAsSCP457(arguments, sender, out response);

                case "npc":
                    return SpawnDummyNPC(sender, out response);

                default:
                    response = "Nieznana podkomenda. Użyj: scp457 <spawn/npc>";
                    return false;
            }
        }

        private bool SpawnPlayerAsSCP457(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count < 2)
            {
                response = "Użycie: scp457 spawn <player_id>";
                return false;
            }

            if (!int.TryParse(arguments.At(1), out int playerId))
            {
                response = "Nieprawidłowy ID gracza.";
                return false;
            }

            Player player = Player.Get(playerId);
            if (player == null)
            {
                response = "Nie znaleziono gracza o podanym ID.";
                return false;
            }

            player.Role.Set(RoleTypeId.Scp106);
            player.Broadcast(10, "<color=red>Jesteś SCP-457! Podpalaj swoich wrogów!</color>");
            player.Health = plugin.Config.StartHealth;

            // Ustawienie fioletowej tarczy
            player.HumeShield = plugin.Config.StartShield;

            plugin.LogDebug($"Gracz {player.Nickname} został zrespiony jako SCP-457.");
            response = $"Gracz {player.Nickname} został SCP-457.";
            return true;
        }

        private bool SpawnDummyNPC(ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("scp457.npc"))
            {
                response = "Brak uprawnień do spawnu SCP-457 jako NPC.";
                return false;
            }

            if (!(sender is PlayerCommandSender playerSender))
            {
                response = "Ta komenda musi być wykonana przez gracza.";
                return false;
            }

            Player player = Player.Get(playerSender.ReferenceHub);

            if (player == null)
            {
                response = "Nie można znaleźć gracza wykonującego polecenie.";
                return false;
            }

            // Pozycja gracza
            var npcPosition = player.Position;

            // Tworzenie dummy
            GameObject dummyObject = new GameObject("SCP-457 Dummy");
            dummyObject.transform.position = npcPosition;

            // Aktywacja
            var npcController = dummyObject.AddComponent<NPCController>();

            // Konfiguracja
            npcController.Initialize(plugin.Config.StartHealth, plugin.Config.StartShield, plugin.Config.BurnRadius, plugin.Config.BurnDamagePerSecond, plugin.Config.BurnDuration);

            // aktywacja aka obiektu
            dummyObject.SetActive(true);

            // odpowiedz
            plugin.LogDebug($"NPC SCP-457 Dummy został zrespiony na pozycji gracza: {npcPosition}");
            response = $"NPC SCP-457 Dummy zrespiony na pozycji gracza: {player.Nickname}.";
            return true;
        }
    }

    public class NPCController : MonoBehaviour
    {
        private float burnRadius;
        private int burnDamagePerSecond;
        private int burnDuration;
        private int health;
        private int shield;

        private void Awake()
        {
            // Dodaj Rigidbody
            var rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;

            //Dodaj Collider
            var collider = gameObject.AddComponent<SphereCollider>();
            collider.radius = burnRadius;
            collider.isTrigger = true;

            Debug.Log("NPC SCP-457 został poprawnie zainicjalizowany.");
        }

        public void Initialize(int health, int shield, float burnRadius, int burnDamagePerSecond, int burnDuration)
        {
            this.health = health;
            this.shield = shield;
            this.burnRadius = burnRadius;
            this.burnDamagePerSecond = burnDamagePerSecond;
            this.burnDuration = burnDuration;
        }

        private void OnTriggerEnter(Collider other)
        {
            var player = Player.Get(other.gameObject);
            if (player != null && player.IsHuman)
            {
                StartCoroutine(ApplyBurn(player));
            }
        }

        private System.Collections.IEnumerator ApplyBurn(Player player)
        {
            float timer = burnDuration;
            while (timer > 0)
            {
                if (Vector3.Distance(transform.position, player.Position) > burnRadius)
                {
                    break;
                }

                player.Hurt(burnDamagePerSecond, "Burn");
                yield return new WaitForSeconds(1);
                timer--;
            }
        }

        //todo naprawić ten syf
    }
}
