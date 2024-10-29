using System;
using _Game.Scripts.BaseScripts;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Point
{
    public class PointUiController : CanvasGroupController
    {
        [SerializeField] private TextMeshProUGUI pointText;
        [SerializeField] private TextMeshProUGUI collectedText;
        private int _point;
        private int _collected;

        private void Start()
        {
            SetView();
        }

        public void Show(PointController pointController)
        {
            SetPoint(pointController);
            base.Show();
        }

        private void SetPoint(PointController pointController)
        {
            _point = pointController.Point;
            _collected = pointController.CollectPoint;
            SetView();
        }

        private void SetView()
        {
            pointText.SetText($"Point: {_point}");
            collectedText.SetText($"Collected: {_collected}");
        }
    }
}