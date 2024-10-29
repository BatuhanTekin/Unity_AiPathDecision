using System;
using UnityEngine;

namespace _Game.Scripts.GolfBall
{
    [Serializable]
    public struct GolfBallData
    {
        public int spawnCount;
        public float minDistance;
        public float spawnRadius;
        public int maxRetriesPerCell;
        public int hardBallDistance;
        public int midBallDistance;
    }
}