using Games.PB;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TrueSync;
using UnityEngine;
using System;

namespace Games.Wars
{
    public class WarPunPlayer : Photon.PunBehaviour, IPunObservable
    {
        public WarRoom room;
        public LegionData legionData;
        public int LegionId
        {
            get
            {
                return legionData.legionId;
            }
        }

        public WarSyncUnitJoy syncJoyMove = new WarSyncUnitJoy();
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
            }
            else
            {
            }
        }



        private void Awake()
        {
            room = War.currentRoom;
            UnitType unitType = (UnitType)photonView.instantiationData[CreateUnitPropertiesKey.UnitType];
            int legionId = (int)photonView.instantiationData[CreateUnitPropertiesKey.LegionId];
            legionData = room.sceneData.GetLegion(legionId);
        }

        private void Update()
        {
            if(photonView.isMine)
            {
            }
        }

        
    }
}