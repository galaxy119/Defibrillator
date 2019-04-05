using System.Xml.Linq;
using System;
using Smod2;
using Smod2.API;
using ItemManager;
using ItemManager.Recipes;
using ItemManager.Utilities;
using ServerMod2.API;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MEC;

namespace Defib
{
	public class Defib : CustomItem
	{

		public override bool OnPickup()
		{
			Player player = new SmodPlayer(base.PlayerObject);

			player.PersonalBroadcast(3, "You have picked up a Defibrilator!", false);

			return base.OnPickup();
		}

		public override bool OnDrop()
		{
			Player player = new SmodPlayer(base.PlayerObject);
			GameObject ply = (GameObject)player.GetGameObject();

			foreach (Ragdoll ragdoll in DefibPlugin.ragdolls.Where(rg => rg.transform.position.x <= ply.transform.position.x + 5f || rg.transform.position.z <= ply.transform.position.z + 5f))
			{
				int limit = 0;
				if (limit > 0) break;

				if (FriendlyRoles((int)player.TeamRole.Role).Contains(ragdoll.owner.charclass))
				{
					foreach (Player target in PluginManager.Manager.Server.GetPlayers())
					{
						if (target.PlayerId == ragdoll.owner.PlayerId && target.TeamRole.Role == Role.SPECTATOR)
						{
							Delete();

							if (DefibPlugin.Delay > 0)
								Timing.RunCoroutine(_Delay(() => target.ChangeRole(player.TeamRole.Role, false, false, false, false), DefibPlugin.Delay));
							else
								target.ChangeRole(player.TeamRole.Role, false, false, false, false);

							Timing.RunCoroutine(_Delay(() => target.SetHealth(Mathf.RoundToInt(target.GetHealth() * DefibPlugin.ReviveHealth)), 2f));
							limit++;
							RagdollManager.Destroy(ragdoll, 0f);
						}
					}
				}
			}
			return base.OnDrop();
		}

		private List<int> FriendlyRoles(int role)
		{
			List<int> friends = new List<int>();

			if (role == 1 || role == 8)
			{
				friends.Add(1);
				friends.Add(8);
			}
			else if (role == 4 || role == 6 || role == 11 || role == 12 || role == 13 || role == 15)
			{
				friends.Add(4);
				friends.Add(6);
				friends.Add(11);
				friends.Add(12);
				friends.Add(13);
				friends.Add(15);
			}

			return friends;
		}

		private IEnumerator<float> _Delay(Action action, float delay)
		{
			yield return Timing.WaitForSeconds(delay);
			action();
		}
	}
}