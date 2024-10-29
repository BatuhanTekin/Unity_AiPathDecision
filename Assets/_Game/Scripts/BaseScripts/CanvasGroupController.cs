using UnityEngine;

namespace _Game.Scripts.BaseScripts
{
    public class CanvasGroupController : MonoBehaviour
    {
        [SerializeField] private bool isInteractable = true;
        [SerializeField] private CanvasGroup canvasGroup;

        public virtual void Show()
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = isInteractable;
            canvasGroup.blocksRaycasts = isInteractable;
        }

        public virtual void Hide()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}