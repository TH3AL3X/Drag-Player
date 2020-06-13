using Rocket.Unturned;
using Rocket.Core.Plugins;
using SDG.Unturned;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using Steamworks;
using System;
using System.Linq;
using Rocket.Core;
using Rocket.Unturned.Chat;
using System.Timers;
using System.Net;
using System.Collections;
using UnityEngine;
using Rocket.Unturned.Events;
using Rocket.API;

namespace DragPlayer
{
    public class Main : RocketPlugin<Config>
    {
        public static Main Instance;

        protected override void Load()
        {
            Instance = this;
            UnturnedPlayerEvents.OnPlayerUpdateGesture += UnturnedPlayerEvents_OnPlayerUpdateGesture;
        }

        private void OnDeath(UnturnedPlayer player, SDG.Unturned.EDeathCause cause, SDG.Unturned.ELimb limb, CSteamID killer)
        {
            if (player.GetComponent<PlayerComponent>().dragging)
            {
                foreach (UnturnedPlayer jugadordic in player.GetComponent<PlayerComponent>().jugador)
                {
                    foreach (UnturnedPlayer victimas in player.GetComponent<PlayerComponent>().victima)
                    {
                        // Don't ask me why i put this, pFunctions
                        if (jugadordic != null || victimas != null)
                        {
                            player.GetComponent<PlayerComponent>().dragging = false;
                            player.GetComponent<PlayerComponent>().victima.Remove(victimas);
                            player.GetComponent<PlayerComponent>().jugador.Remove(jugadordic);
                        }
                    }
                }
            }
        }

        private void UnturnedPlayerEvents_OnPlayerUpdateGesture(UnturnedPlayer player, UnturnedPlayerEvents.PlayerGesture gesture)
        {
            Player victimPlayer = RaycastHelper.GetPlayerFromHits(player.Player, 2);

            UnturnedPlayer victim = UnturnedPlayer.FromPlayer(victimPlayer);
            RocketPlayer rocketPlayer = new RocketPlayer(player.Id);

            foreach (UnturnedPlayer victimas in player.GetComponent<PlayerComponent>().victima)
            {
                foreach (UnturnedPlayer jugador in player.GetComponent<PlayerComponent>().jugador)
                {
                    if (gesture == UnturnedPlayerEvents.PlayerGesture.SurrenderStart)
                    {
                        if (rocketPlayer.HasPermission("dragpeople"))
                        {
                            if (victimPlayer != null)
                            {
                                if (victim.Player.animator.gesture == EPlayerGesture.ARREST_START && !player.GetComponent<PlayerComponent>().dragging)
                                {
                                    player.GetComponent<PlayerComponent>().victima.Add(victim);
                                    player.GetComponent<PlayerComponent>().jugador.Add(player);
                                    player.GetComponent<PlayerComponent>().dragging = true;
                                    victim.Player.movement.pluginSpeedMultiplier = 0;
                                }
                            }
                        }
                    }
                    else if (gesture == UnturnedPlayerEvents.PlayerGesture.SurrenderStop || gesture == UnturnedPlayerEvents.PlayerGesture.PunchLeft || gesture == UnturnedPlayerEvents.PlayerGesture.PunchRight/* || gesture == UnturnedPlayerEvents.PlayerGesture.None*/ && !jugador.IsInVehicle)
                    {
                        if (rocketPlayer.HasPermission("dragpeople"))
                        {
                            if (victimPlayer != null)
                            {
                                victimas.Player.movement.pluginSpeedMultiplier = 1;
                                player.GetComponent<PlayerComponent>().jugador.Remove(player);
                                player.GetComponent<PlayerComponent>().victima.Remove(victimas);
                                player.GetComponent<PlayerComponent>().dragging = false;
                            }
                        }
                    }
                }
            }
        }

        protected override void Unload()
        {
            UnturnedPlayerEvents.OnPlayerUpdateGesture -= UnturnedPlayerEvents_OnPlayerUpdateGesture;
        }
    }
}
