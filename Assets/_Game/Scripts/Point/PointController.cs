using System;
using _Game.Scripts.GolfBall;
using UnityEngine;

namespace _Game.Scripts.Point
{
    [Serializable]
    public struct PointController
    {
        public int CollectPoint { get; private set; }
        public int Point { get; private set; }
        
        public void OnCollectBall(GolfBallController ball)
        {
            CollectPoint += ball.Point;
            ball.OnCollect();
        }

        public void SavePoint()
        {
            Point += CollectPoint;
            CollectPoint = 0;
        }   
    }
}