#SCP-575
======
made by Joker119
## Description
Defibrillators are small electronic devices used to restart the hearts of a dieing person.
A Defibrillator will appear in-game as a Weapon manager Tablet, however it will notify you when you pick it up that it is infact something special.
You use this by dropping it on the ground near a friendly players body, if they are still dead (IE: Not respawned) it will revive them with a small amount of health.
The Defibrillator is consumed in the process, but only if it successfully revives someone.

### Features
 - ItemManager support
 - Configurable delay before reviving
 - Configurable health percentage to set people to upon reviving
 - configurable spawn locations.

### Config Settings
Config option | Config Type | Default Value | Description
:---: | :---: | :---: | :------
defib_enabled | Bool | true | Wether or not this custom item should be enabled and usable in the game
defib_delay | Int | 5 | The number of seconds a Defib takes to revive someone.
defib_spawns | List | 049chamber | The rooms a Defib can spawn in. Accepts: 049chamber, 096chamber, 173armory and bathrooms
defib_health | Float | | 0.1 | The percentage multiplier used to set revived players health. 1=100%, 0.1=10%, 0.01=1%

### Commands
  Command |  |  | Description
:---: | :---: | :---: | :------
imgive PlayerID 106 | | | Gives PlayerID a Defibrilator

