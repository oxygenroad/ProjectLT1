using System;
using System.Collections.Generic;
using Unity;
using UnityEngine;

namespace GameServer.Game
{
    [Serializable]
    public class GameSetupData
    {
        //public int StationCount;
        
        public List<PlayerData> PlayerData;
        [Range(0,10)]public int PodCount;
        public int ColorsPerPlayer;
        public ELightsOutMode LightsOutMode;
        public float LightsOutModeTimeOutSeconds;
        
        public ECompetitionMode CompetitionMode;
        
        public EActivityDurationMode ActivityDurationMode;
        public int ActivityDurationHitCount;
        public float ActivityDurationTimeSeconds;
        //public int Cycle;


    }
}