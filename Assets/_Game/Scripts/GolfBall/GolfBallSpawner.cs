using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CharacterController = _Game.Scripts.Character.Controllers.CharacterController;

namespace _Game.Scripts.GolfBall
{
    public class GolfBallSpawner : MonoBehaviour
    {
        [SerializeField] private GolfBallController golfBallPrefab;
        [SerializeField] private CharacterController character;
        [SerializeField] private Transform spawnParent;
        [SerializeField] private GolfBallData golfBallData;

        private Dictionary<GolfBallController, Vector3> _spawnedPositions = new();
        private int _retries;
        private Vector3 _cellCenter;
        private Vector3 _randomPoint;
        private NavMeshPath _path;

        private void Start()
        {
            _spawnedPositions.EnsureCapacity(golfBallData.spawnCount);
            SpawnGolfBalls();
            character.OnBallSpawn();
        }

        public Dictionary<GolfBallController, Vector3> GetBallPositions()
        {
            return _spawnedPositions;
        }

        private void SpawnGolfBalls()
        {
            var cellsPerRow = Mathf.CeilToInt(Mathf.Sqrt(golfBallData.spawnCount));
            var cellSize = (2 * golfBallData.spawnRadius) / cellsPerRow;
            var successfulSpawns = 0;

            for (int i = 0; i < cellsPerRow && successfulSpawns < golfBallData.spawnCount; i++)
            {
                for (int j = 0; j < cellsPerRow && successfulSpawns < golfBallData.spawnCount; j++)
                {
                    _retries = 0;
                    

                    while (_retries < golfBallData.maxRetriesPerCell && successfulSpawns < golfBallData.spawnCount)
                    {
                        _cellCenter = GetCellCenter(i, j, cellSize);
                        _randomPoint = GetRandomNavMeshPointInCell(_cellCenter, cellSize);

                        if (_randomPoint != Vector3.zero && IsFarEnoughFromOthers(_randomPoint))
                        {
                            SpawnGolfBall(_randomPoint);
                            successfulSpawns++;
                            break;
                        }
                        _retries++;
                    }
                }
            }

            while (successfulSpawns < golfBallData.spawnCount)
            {
                _randomPoint = GetRandomNavMeshPoint();
                if (_randomPoint != Vector3.zero && IsFarEnoughFromOthers(_randomPoint))
                {
                    SpawnGolfBall(_randomPoint);
                    successfulSpawns++;
                }
            }
        }

        private Vector3 GetCellCenter(int row, int col, float cellSize)
        {
            Vector3 origin = transform.position - new Vector3(golfBallData.spawnRadius, 0, golfBallData.spawnRadius);
            return origin + new Vector3(col * cellSize + cellSize / 2, 0, row * cellSize + cellSize / 2);
        }

        private Vector3 GetRandomNavMeshPointInCell(Vector3 cellCenter, float cellSize)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-cellSize / 2, cellSize / 2),
                0,
                Random.Range(-cellSize / 2, cellSize / 2)
            );

            Vector3 candidatePoint = cellCenter + randomOffset;

            if (NavMesh.SamplePosition(candidatePoint, out var hit, golfBallData.minDistance, NavMesh.AllAreas))
            {
                return hit.position;
            }

            return Vector3.zero;
        }

        private Vector3 GetRandomNavMeshPoint()
        {
            Vector3 randomDirection = Random.insideUnitSphere * golfBallData.spawnRadius;
            randomDirection += transform.position;

            return NavMesh.SamplePosition(randomDirection, out var hit, golfBallData.spawnRadius, NavMesh.AllAreas)
                ? hit.position
                : Vector3.zero;
        }

        private bool IsFarEnoughFromOthers(Vector3 position)
        {
            foreach (var spawnedPosition in _spawnedPositions)
            {
                if (Vector3.Distance(spawnedPosition.Value, position) < golfBallData.minDistance)
                {
                    return false;
                }
            }
            return true;
        }

        private void SpawnGolfBall(Vector3 position)
        {
            var golfBall = Instantiate(golfBallPrefab, position, Quaternion.identity, spawnParent);
            golfBall.SetPoint(CalculatePoint(position));
            _spawnedPositions.Add(golfBall, position);

        }

        private int CalculatePoint(Vector3 position)
        {
            _path = new NavMeshPath();
            character.Agent.CalculatePath(position, _path);

            if (_path.status != NavMeshPathStatus.PathComplete)
                return 1; 

            var pathDistance = 0.0f;
            
            for (int i = 1; i < _path.corners.Length; i++)
            {
                pathDistance += Vector3.Distance(_path.corners[i - 1], _path.corners[i]);
            }

            return GetPointByDistance(pathDistance);
        }

        private int GetPointByDistance(float pathDistance)
        {
            if (pathDistance > golfBallData.midBallDistance && pathDistance < golfBallData.hardBallDistance)
            {
                return 2;
            }

            return golfBallData.hardBallDistance < pathDistance ? 3 : 1;
        }

        public Vector3 GetPosition(GolfBallController ball)
        {
            return _spawnedPositions[ball];
        }
    }
}
