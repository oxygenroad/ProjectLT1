using System;

namespace GameServer.Game
{
    [Serializable]
    public class PlayerInstance
    {
        public PlayerData PlayerData;
        public int TimePassed;
        public int HitCount;
        public int MissCount;
        public int ActivePod = -1;
    }
}