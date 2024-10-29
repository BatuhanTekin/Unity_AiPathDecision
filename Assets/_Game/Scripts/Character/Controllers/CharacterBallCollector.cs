using System;
using System.Collections.Generic;
using _Game.Scripts.GolfBall;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class CharacterBallCollector
{
    [SerializeField] private GolfBallSpawner golfBallSpawner;
    
    private List<KeyValuePair<GolfBallController, Vector3>> _balls = new();
    private NavMeshPath _path;

    public bool IsEmpty { get; private set; } = false;

    
    public void Init()
    {
        _balls = new List<KeyValuePair<GolfBallController, Vector3>>(golfBallSpawner.GetBallPositions());
        _path = new NavMeshPath();
        UpdateIsEmptyStatus();
    }

    public GolfBallController GetNextBestBall(NavMeshAgent agent, float remainHealth)
    {
        if (IsEmpty) return null;

        GolfBallController bestBall = null;
        var bestScore = float.MinValue;
        var bestBallIndex = -1;

        for (int i = 0; i < _balls.Count; i++)
        {
            var kvp = _balls[i];
            GolfBallController ball = kvp.Key;
            Vector3 position = kvp.Value;
            
            
            if (NavMesh.CalculatePath(agent.transform.position, position, NavMesh.AllAreas, _path) && _path.status == NavMeshPathStatus.PathComplete)
            {
                var distance = GetPathDistance(_path);
                var timeToReach = distance / agent.speed;
                
                if (remainHealth >= timeToReach)
                {
                    float score = ball.Point / timeToReach;
                    
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestBall = ball;
                        bestBallIndex = i;
                    }
                }
            }
        }
        
        if (bestBall != null && bestBallIndex != -1)
        {
            _balls.RemoveAt(bestBallIndex);
            UpdateIsEmptyStatus();
        }

        return bestBall;
    }

    public void ReturnBall(GolfBallController ball)
    {
        _balls.Add(new KeyValuePair<GolfBallController, Vector3>(ball,ball.transform.position));
    }
    
    private void UpdateIsEmptyStatus()
    {
        IsEmpty = _balls.Count == 0;
    }
    
    private float GetPathDistance(NavMeshPath path)
    {
        float distance = 0f;
        for (int i = 1; i < path.corners.Length; i++)
        {
            distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }
        return distance;
    }
    
}
