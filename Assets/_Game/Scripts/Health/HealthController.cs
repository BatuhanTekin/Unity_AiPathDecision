using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Health
{
    [Serializable]
    public struct HealthController
    {
        public bool IsFinished => Health <= 0;
        public float Health { get; private set; }

        [SerializeField] private float startHealth;
        [SerializeField] private Image fillImage;
        [SerializeField] private TextMeshProUGUI healthText;
        
        private float _remainHealthRatio;

        public void Init()
        {
           ResetHealth();
        }
        
        public void OnHealthDecrease()
        {
            if (IsFinished)
            {
                return;
            }
            
            Health -= Time.fixedDeltaTime;
            Health = Math.Max(Health, 0);
            
            _remainHealthRatio = Health / startHealth;
            fillImage.fillAmount = _remainHealthRatio;
            healthText.text = "%"+(_remainHealthRatio * 100).ToString("0");
        }

        public void ResetHealth()
        {
            Health = startHealth;
        }
    }
}
