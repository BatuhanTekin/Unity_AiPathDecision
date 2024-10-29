using System;
using System.Collections.Generic;
using _Game.Scripts.Character.Enums;
using _Game.Scripts.GolfBall;
using _Game.Scripts.Health;
using _Game.Scripts.Point;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace _Game.Scripts.Character.Controllers
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private Transform baseCart;
        
        [SerializeField] private CharacterAnimationController characterAnimationController;
        [SerializeField] private CharacterMovementController characterMovementController;
        [SerializeField] private CharacterBallCollector ballCollector;  
        [SerializeField] private HealthController healthController;
        [SerializeField] private PointUiController pointUiController;
        public NavMeshAgent Agent => characterMovementController.agent;

        private PointController _pointController;
        private GolfBallController _currentBall;

        private void Awake()
        {
            healthController.Init();
            characterMovementController.Init(characterAnimationController);
        }
        
        public void OnBallSpawn()
        {
            ballCollector.Init();
            MoveToBall(); 
        }
        
        // animation event
        public void OnGrabBall()
        {
            _pointController.OnCollectBall(_currentBall);
            pointUiController.Show(_pointController);
            _currentBall = null;
            characterMovementController.OnArriveTarget -= OnCollectPoint;
            MoveToBall();
        }
        
        private void FixedUpdate()
        {
            healthController.OnHealthDecrease();
            characterMovementController.CheckDestination();

            if (healthController.Health <= 0)
            {
                if (_currentBall != null)
                {
                    characterMovementController.OnArriveTarget -= OnCollectPoint;
                    ballCollector.ReturnBall(_currentBall);
                    _currentBall = null;
                }
                MoveBase();
            }
        }

        private void MoveToBall()
        {
            if (ballCollector.IsEmpty)
            {
                MoveBase();
                return;
            }

            _currentBall = ballCollector.GetNextBestBall(characterMovementController.agent, healthController.Health);
            if (_currentBall == null)
            {
                MoveBase();
                return;
            }
            
            characterMovementController.SetMoveTarget(_currentBall.transform);
            characterMovementController.OnArriveTarget += OnCollectPoint;
        }

        private void MoveBase()
        {
            if (characterMovementController.Target == baseCart)
            {
                return;
            }
            characterMovementController.SetMoveTarget(baseCart);
            characterMovementController.OnArriveTarget += OnReachBase;
        }

        private void OnReachBase(Transform baseTransform)
        {
            if (baseTransform != baseCart)
            {
                return;
            }

            healthController.ResetHealth();
            characterMovementController.OnArriveTarget -= OnReachBase;
            _pointController.SavePoint();
            pointUiController.Show(_pointController);

            if (ballCollector.IsEmpty)
            {
                characterMovementController.Stop();
                characterAnimationController.Animate(CharacterAnimationType.Idle);
                return;
            }
            
            MoveToBall();
        }
        
        
        private void OnCollectPoint(Transform ballTransform)
        {
            if (_currentBall != null || ballTransform == _currentBall.transform)
            {
                characterAnimationController.Animate(CharacterAnimationType.Grab);
            }
        }
    }
}