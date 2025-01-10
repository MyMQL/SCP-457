using System;
using Exiled.API.Features;

namespace SCP457Plugin
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance { get; private set; }

        public override string Name => "SCP-457";
        public override string Author => "Mikul";
        public override Version RequiredExiledVersion => new Version(9, 1, 1);
        public override Version Version => new Version(1, 0, 0);

        private EventHandlers eventHandlers;

        public override void OnEnabled()
        {
            Instance = this;

            eventHandlers = new EventHandlers(this);

            Exiled.Events.Handlers.Server.RoundStarted += eventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Player.Hurting += eventHandlers.OnPlayerHurting;
            Exiled.Events.Handlers.Player.Dying += eventHandlers.OnPlayerDying;

            LogDebug("Plugin SCP-457 has been enabled.");
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= eventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Player.Hurting -= eventHandlers.OnPlayerHurting;
            Exiled.Events.Handlers.Player.Dying -= eventHandlers.OnPlayerDying;

            eventHandlers = null;
            Instance = null;

            LogDebug("Plugin SCP-457 has been disabled.");
            base.OnDisabled();
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