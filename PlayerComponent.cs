using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DragPlayer
{
    public class PlayerComponent : UnturnedPlayerComponent
    {

        public List<UnturnedPlayer> victima = new List<UnturnedPlayer>();

        public List<UnturnedPlayer> jugador = new List<UnturnedPlayer>();

        public bool dragging = false;

        byte num, num1;

        Vector3 vector3 = new Vector3();

        protected override void Load()
        {
            VehicleManager.onEnterVehicleRequested += OnEnterVehicle;
            VehicleManager.onExitVehicleRequested += OnLeaveVehicle;
            UnturnedPlayerEvents.OnPlayerUpdatePosition += UnturnedPlayerEvents_PlayerUpdatePosition;
        }

        protected override void Unload()
        {
            VehicleManager.onEnterVehicleRequested -= OnEnterVehicle;
            VehicleManager.onExitVehicleRequested -= OnLeaveVehicle;
            UnturnedPlayerEvents.OnPlayerUpdatePosition += UnturnedPlayerEvents_PlayerUpdatePosition;
        }


        private void OnLeaveVehicle(Player player, InteractableVehicle vehicle, ref bool cancel, ref Vector3 pendingLocation, ref float pendingYaw)
        {

            UnturnedPlayer manigga = UnturnedPlayer.FromPlayer(player);

            foreach (UnturnedPlayer victima in victima)
            {
                foreach (UnturnedPlayer jugador in jugador)
                {

                    if(manigga == jugador)
                    {
                        victima.CurrentVehicle.forceRemovePlayer(out num, victima.CSteamID, out vector3, out num1);
                        cancel = true;
                    }

                    if (manigga == victima)
                    {
                        cancel = false;
                    }

                }
            }
        }

        private void OnEnterVehicle(Player player, InteractableVehicle vehicle, ref bool cancel)
        {

            UnturnedPlayer nigga = UnturnedPlayer.FromPlayer(player);

            foreach (UnturnedPlayer victima in victima)
            {
                foreach (UnturnedPlayer jugador in jugador)
                {
                    if (nigga == jugador)
                    {
                        cancel = true;
                        jugador.CurrentVehicle.tryAddPlayer(out num, victima.Player);
                    }

                    if (nigga == victima)
                    {
                        cancel = false;
                    }
                }
            }
        }


        private void UnturnedPlayerEvents_PlayerUpdatePosition(UnturnedPlayer niggamove, Vector3 californiaposition)
        {
            foreach (UnturnedPlayer jugador in jugador)
            {
                foreach (UnturnedPlayer victima in victima)
                {
                    if (dragging)
                    {
                        victima.Player.transform.position = jugador.Position;
                    }
                }
            }
        }
        /*
        public void FixedUpdate()
        {
                        if (jugador.CurrentVehicle)
                        {
                            jugador.CurrentVehicle.addPlayer(0, victima.CSteamID);
                        }
                        else
                        {
                            victima.Player.transform.position = jugador.Position;
                            victima.CurrentVehicle.forceRemovePlayer(out num, victima.CSteamID, out vector3, out num1);
                        }
                    }
                }
            }
        }*/
    }
}
