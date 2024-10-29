using System;
using UnityEngine;

namespace _Game.Scripts.GolfBall
{
    public class GolfBallController : MonoBehaviour
    { 
        public int Point { get; private set; }
        public bool Collected { get; private set; }
        
        
        public void OnCollect()
        {
            Collected = true;
            gameObject.SetActive(false);
        }

        public void SetPoint(int point)
        {
            Point = point;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Point switch
            {
                0 => Color.green,
                1 => Color.blue,
                2 => Color.magenta,
                3 => Color.red
            };
            Gizmos.DrawSphere(transform.position + Vector3.up*0.25f, 0.34f);
        }

    }
}