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
			int limit = 0;

			foreach (Ragdoll ragdoll in DefibPlugin.ragdolls.Where(rg => rg.transform.position.x <= ply.transform.position.x + 5f || rg.transform.position.z <= ply.transform.position.z + 2.5f))
			{
				if (limit > 0) break;

				if (FriendlyRoles((int)player.TeamRole.Role).Contains(ragdoll.owner.charclass))
				{
					foreach (Player target in PluginManager.Manager.Server.GetPlayers())
					{
						if (target.PlayerId == ragdoll.owner.PlayerId && target.TeamRole.Role == Role.SPECTATOR)
						{
							Delete();

							switch (ragdoll.owner.charclass)
							{
								case 1:
									DefibPlugin.TargetRole = Role.CLASSD;
									break;
								case 4:
									DefibPlugin.TargetRole = Role.NTF_SCIENTIST;
									break;
								case 6:
									DefibPlugin.TargetRole = Role.SCIENTIST;
									break;
								case 8:
									DefibPlugin.TargetRole = Role.CHAOS_INSURGENCY;
									break;
								case 11:
									DefibPlugin.TargetRole = Role.NTF_LIEUTENANT;
									break;
								case 12:
									DefibPlugin.TargetRole = Role.NTF_COMMANDER;
									break;
								case 13:
									DefibPlugin.TargetRole = Role.NTF_CADET;
									break;
								case 15:
									DefibPlugin.TargetRole = Role.FACILITY_GUARD;
									break;
							}

							if (DefibPlugin.Delay > 0)
								Timing.RunCoroutine(_Delay(() => target.ChangeRole(DefibPlugin.TargetRole, false, false, false, false), DefibPlugin.Delay));
							else
								target.ChangeRole(DefibPlugin.TargetRole, false, false, false, false);

							Timing.RunCoroutine(_Delay(() => target.SetHealth(Mathf.RoundToInt(target.GetHealth() * DefibPlugin.ReviveHealth)), 2f));
							limit++;
							NetworkServer.Destroy(ragdoll.gameObject);
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