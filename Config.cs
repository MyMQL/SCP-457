using Exiled.API.Interfaces;

namespace SCP457Plugin
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        public float SpawnChance { get; set; } = 50f;
        public float BurnRadius { get; set; } = 2f;
        public int BurnDamagePerSecond { get; set; } = 1;
        public int BurnDuration { get; set; } = 5;
        public int TouchDamage { get; set; } = 50;
        public int StartHealth { get; set; } = 2800;
        public int StartAHp { get; set; } = 300;
        public string FireSound { get; set; } = "/home/container/.config/EXILED/Configs/SCP457/fire.ogg";
        public int StartShield { get; set; } = 300;

    }
}

