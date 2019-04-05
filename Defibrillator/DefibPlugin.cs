using Smod2;
using Smod2.API;
using Smod2.Attributes;
using Smod2.Config;
using Smod2.Commands;
using ItemManager;
using ItemManager.Recipes;
using ItemManager.Utilities;
using System;
using System.Collections.Generic;
using MEC;

namespace Defib
{
	[PluginDetails(
		author = "Joker119",
		name = "Defibrillator",
		description = "Adds a Defibrillator that revives dead teammates.",
		id = "joker.customitems.Defibrilator",
		version = "1.0.0",
		SmodMajor = 3,
		SmodMinor = 4,
		SmodRevision = 0
	)]

	public class DefibPlugin : Plugin
	{
		public const int DefibID = 106;

		public static CustomItemHandler<Defib> Handler { get; private set; }

		public static Ragdoll[] ragdolls;

		public static float ReviveHealth { get; private set; }
		public static float Delay { get; private set; }

		public static List<string> SpawnLocations;

		public override void OnDisable()
		{
			this.Info("Defibrillator has been disabled.");
		}

		public override void OnEnable()
		{
			this.Info("Defibrilator has been enabled.");
		}

		public override void Register()
		{
			this.AddConfig(new ConfigSetting("defib_health", 0.1f, true, "The percentage of health the target is revived with."));
			this.AddConfig(new ConfigSetting("defib_delay", 5, true, "The number of seconds before a Defib revives a player."));
			this.AddConfig(new ConfigSetting("defib_spawns", new string[] { "049chamber" }, true, true, "Locations the Defib can spawn in."));
			this.AddConfig(new ConfigSetting("defib_enabled", true, true, "If the defib item should be enabled."));

			this.AddEventHandlers(new EventHandler(this), Smod2.Events.Priority.Low);

			Handler = new CustomItemHandler<Defib>(DefibPlugin.DefibID)
			{
				DefaultType = ItemType.WEAPON_MANAGER_TABLET,
			};

			Handler.Register();
			Items.AddRecipe(new Id914Recipe(KnobSetting.FINE, (int)ItemType.WEAPON_MANAGER_TABLET, DefibPlugin.DefibID, 1));
		}

		public void ReloadConfig()
		{
			ReviveHealth = GetConfigFloat("defib_health");
			Delay = GetConfigFloat("defib_delay");
			SpawnLocations = new List<string>(GetConfigList("defib_spawns"));
		}
	}
}