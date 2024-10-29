using System;
using _Game.Scripts.Character.Enums;
using UnityEngine;
using UnityEngine.AI;

namespace _Game.Scripts.Character.Controllers
{
    [Serializable]
    public class CharacterMovementController
    {
        public Transform Target => _moveTarget;

        public Action<Transform> OnArriveTarget;
        public NavMeshAgent agent;
        
        [SerializeField] private float stopDistance = 0.5f;

        private Transform _moveTarget;
        private CharacterAnimationController _animator;

        public void Init(CharacterAnimationController controller)
        {
            _animator = controller;
        }
        public void SetMoveTarget(Transform target)
        {
            _moveTarget = target;
            Move();
        }

        public void Move()
        {
            agent.isStopped = false;
            _animator.Animate(CharacterAnimationType.Walk);
            agent.SetDestination(_moveTarget.position);
        }

        public void CheckDestination()
        {
            if (_moveTarget == null)
            {
                return;
            }

            var targetPos = new Vector3(_moveTarget.position.x, agent.transform.position.y, _moveTarget.position.z);
            if (Vector3.Distance(targetPos, agent.transform.position) <= stopDistance)
            {
                agent.isStopped = true;
                OnArriveTarget?.Invoke(_moveTarget);
            }
        }

        public void Stop()
        {
            _moveTarget = null;
        }
    }
}