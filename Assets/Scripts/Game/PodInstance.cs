using System;
using UnityEngine;

namespace GameServer.Game
{
    [Serializable]
    public class PodInstance
    {
        public bool IsOn;
        public Color CurrentColor;
        public float TimePassed;
        public bool IsHit;
    }
}