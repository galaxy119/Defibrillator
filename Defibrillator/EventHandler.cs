using System.Linq;
using Smod2;
using Smod2.API;
using Smod2.Events;
using Smod2.EventSystem;
using Smod2.EventHandlers;
using ItemManager;
using ItemManager.Recipes;
using ItemManager.Utilities;
using UnityEngine;
using UnityEngine.Networking;
using MEC;
using System;
using System.Collections.Generic;

namespace Defib
{
	public class EventHandler : IEventHandlerPlayerDie, IEventHandlerRoundStart, IEventHandlerWaitingForPlayers
	{
		private readonly DefibPlugin plugin;
		public EventHandler(DefibPlugin plugin) => this.plugin = plugin;

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			if (!this.plugin.GetConfigBool("defib_enabled"))
				PluginManager.Manager.DisablePlugin(plugin);

			plugin.ReloadConfig();
		}

		public void OnRoundStart(RoundStartEvent ev)
		{
			RandomItemSpawner ris = UnityEngine.Object.FindObjectOfType<RandomItemSpawner>();
			List<SpawnLocation> spawns = new List<SpawnLocation>();

			foreach (string spawn in DefibPlugin.SpawnLocations)
			{
				string sl = spawn.ToLower();
				plugin.Info("Spawning Defibrilator at: " + sl);
				List<SpawnLocation> choices = null;

				switch (sl)
				{
					case "049chamber":
						choices = ris.posIds
							.Where(x => x.posID == "049_Medkit")
							.Select(x => new SpawnLocation(x.position.position, x.position.rotation))
							.ToList();
						return;
					case "096chamber":
						choices = ris.posIds
							.Where(x => x.posID == "Fireman")
							.Select(x => new SpawnLocation(x.position.position, x.position.rotation))
							.ToList();
						return;
					case "173armory":
						choices = ris.posIds
							.Where(x => x.posID == "RandomPistol" && x.position.parent.parent.gameObject.name == "Root_173")
							.Select(x => new SpawnLocation(x.position.position, x.position.rotation))
							.ToList();
						return;
					case "bathrooms":
						choices = ris.posIds
							.Where(x => x.posID == "toilet_keycard")
							.Select(x => new SpawnLocation(x.position.position, x.position.rotation))
							.ToList();
						return;
				}

				if (choices == null || choices.Count == 0)
				{
					plugin.Info("Invalid spawn location: " + sl);
					return;
				}

				spawns.Add(choices[UnityEngine.Random.Range(0, choices.Count)]);
			}

			foreach (SpawnLocation sl in spawns)
			{
				DefibPlugin.Handler.CreateOfType(sl.Position, sl.Rotation);
			}
		}

		public void OnPlayerDie(PlayerDeathEvent ev)
		{
			Timing.RunCoroutine(_RunAfter(() => DefibPlugin.ragdolls = GameObject.FindObjectsOfType<Ragdoll>()));
		}

		public IEnumerator<float> _RunAfter(Action action)
		{
			yield return Timing.WaitForOneFrame;
			action();
		}

		public struct SpawnLocation
		{
			public Vector3 Position { get; private set; }
			public Quaternion Rotation { get; private set; }
			public SpawnLocation(Vector3 pos, Quaternion rot)
			{
				this.Position = pos;
				this.Rotation = rot;
			}
		}
	}
}