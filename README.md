# SCP-457 Plugin for EXILED

## Overview
SCP-457 is a custom SCP class plugin for EXILED servers. The plugin introduces SCP-457 with unique abilities, including the ability to ignite players nearby, making it a challenging opponent for other players.

⚠ **The plugin is not yet fully stable and is not recommended for use on production servers.**

---

## Features
- **Custom SCP-457 Class**:
  - Spawns SCP-457 in the SCP-173 containment chamber.
  - SCP-457 emits a fiery aura that burns players within a configurable radius.
  - Deals damage upon touch.
  - Starts with configurable health and shield values.
  
- **Configurable NPC Mode**:
  - Spawn SCP-457 as a testable dummy NPC for debugging or testing purposes.

- **Cassie Announcement**:
  - Announces the appearance of SCP-457 in the facility.

- **Highly Configurable**:
  - Adjustable health, shield, burn radius, burn duration, and burn damage via the configuration file.

⚠ **The plugin is not yet fully stable and is not recommended for use on production servers.**

---

## Installation
1. Install EXILED on your SCP:SL server.
2. Download the latest release of the SCP-457 plugin from the [Releases](https://github.com/MyMQL/SCP-457/releases) section.
3. Place the `.dll` file into your server's `EXILED/Plugins` directory.
4. Restart your server to load the plugin.

---

## Configuration
The plugin generates a configuration file at `/home/container/.config/EXILED/Configs/SCP457.yml`.

Below is an example configuration:
```yaml
scp457:
  spawn_chance: 50
  start_health: 2800
  start_shield: 300
  burn_radius: 2.0
  burn_duration: 5
  burn_damage_per_second: 1
  touch_damage: 50
  enable_cassie_announcement: true
```

---

## Commands
- `/scp457 spawn <player_id>`: Assigns SCP-457 to a player. Requires permission: `scp457.spawn`.
- `/scp457 npc`: Spawns a dummy NPC SCP-457 at the player's position. Requires permission: `scp457.npc`.

---

## Permissions
| Permission       | Description                             |
|-------------------|-----------------------------------------|
| `scp457.spawn`    | Allows spawning a player as SCP-457.    |
| `scp457.npc`      | Allows spawning an SCP-457 dummy NPC.   |

---

## Known Issues
- SCP-457 may occasionally cause performance issues.
- Some functionalities are still under development.
  
⚠ **The plugin is not yet fully stable and is not recommended for use on production servers.**

---

## Contributions
Contributions are welcome! Feel free to fork the repository, make changes, and submit a pull request.

---

## License
This plugin is licensed under the [MIT License](https://opensource.org/licenses/MIT).

---

### **Languages**

#### English
⚠ **The plugin is not yet fully stable and is not recommended for use on production servers.**

#### Polish (Polski)
⚠ **Plugin nie jest jeszcze w pełni stabilny i nie zaleca się jego używania na serwerach produkcyjnych.**

#### German (Deutsch)
⚠ **Das Plugin ist noch nicht vollständig stabil und wird nicht für den Einsatz auf Produktionsservern empfohlen.**

#### French (Français)
⚠ **Le plugin n'est pas encore totalement stable et son utilisation sur des serveurs de production n'est pas recommandée.**

#### Spanish (Español)
⚠ **El plugin aún no es completamente estable y no se recomienda su uso en servidores de producción.**
