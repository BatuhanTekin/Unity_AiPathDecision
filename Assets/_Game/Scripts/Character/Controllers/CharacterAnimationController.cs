using System;
using _Game.Scripts.Character.Enums;
using UnityEngine;

namespace _Game.Scripts.Character.Controllers
{
    [Serializable]
    public class CharacterAnimationController
    {
        [SerializeField] private Animator animator;

        public static readonly int Idle = Animator.StringToHash("Idle");
        public static readonly int Walk = Animator.StringToHash("Walk");
        public static readonly int Grab = Animator.StringToHash("Grab");
        
        private CharacterAnimationType _characterAnimationType;
        private CharacterAnimationType _lastType;
        private int _lastTrigger;

        public void Animate(CharacterAnimationType type)
        {
            if (_lastType == type)
            {
                return;
            }
            
            switch (type)
            {
                case CharacterAnimationType.Idle:
                    SetTrigger(Idle);
                    break;
                case CharacterAnimationType.Walk:
                    SetTrigger(Walk);
                    break;
                case CharacterAnimationType.Grab:
                    SetTrigger(Grab);
                    break;
                default:
                    SetTrigger(Idle);
                    break;
            }
            
            _lastType = type;
        }

        private void SetTrigger(int id)
        {
            animator.ResetTrigger(_lastTrigger);
            _lastTrigger = id;
            animator.ResetTrigger(id);
            animator.SetTrigger(id);
        }
    }
}