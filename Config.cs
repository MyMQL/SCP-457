using Exiled.API.Interfaces;
using PlayerRoles;
using Exiled.API.Enums;

namespace SCP457Plugin
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        public float SpawnChance { get; set; } = 50f;
        public float BurnRadius { get; set; } = 2f;
        public int BurnDamagePerSecond { get; set; } = 15;
        public int BurnDuration { get; set; } = 5;
        public int TouchDamage { get; set; } = 50;
        public int StartHealth { get; set; } = 2800;
        public RoomType SpawnRoomType { get; set; } = RoomType.Hcz939;
    }
}