using System;
using Exiled.API.Features;

namespace SCP457Plugin
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance { get; private set; }

        public override string Name => "SCP-457";
        public override string Author => "MyMQL";
        public override string Prefix => "scp457";
        public override Version RequiredExiledVersion => new Version(8, 14, 0);
        public override Version Version => new Version(1, 0, 0);

        private EventHandlers eventHandlers;

        public override void OnEnabled()
        {
            Instance = this;
            CreateConfigFolder();

            eventHandlers = new EventHandlers(this);

            Exiled.Events.Handlers.Server.RoundStarted += eventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Player.Spawning += eventHandlers.OnPlayerSpawning;
            Exiled.Events.Handlers.Player.Hurting += eventHandlers.OnPlayerHurting;
            Exiled.Events.Handlers.Player.Dying += eventHandlers.OnPlayerDying;

            LogDebug("Plugin SCP-457 został włączony.");
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= eventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Player.Spawning -= eventHandlers.OnPlayerSpawning;
            Exiled.Events.Handlers.Player.Hurting -= eventHandlers.OnPlayerHurting;
            Exiled.Events.Handlers.Player.Dying -= eventHandlers.OnPlayerDying;

            eventHandlers = null;
            Instance = null;

            LogDebug("Plugin SCP-457 został wyłączony.");
            base.OnDisabled();
        }

        private void CreateConfigFolder()
        {
            string path = "/home/container/.config/EXILED/Configs/SCP457";
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
                LogDebug($"Utworzono folder konfiguracyjny SCP-457: {path}");
            }
        }

        public void LogDebug(string message)
        {
            if (Config.Debug)
            {
                Log.Info(message);
            }
        }
    }
}
