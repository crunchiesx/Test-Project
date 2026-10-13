using Crunchies.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Crunchies.UI
{
    public class PauseMenuUI : UIPanel
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button controlsButton;

        private void OnEnable() => GameManager.Instance.OnGamePauseChange += OnGamePauseChange;
        private void OnDisable() => GameManager.Instance.OnGamePauseChange -= OnGamePauseChange;

        private void OnGamePauseChange()
        {
            if (GameManager.Instance.IsPaused)
            {
                OpenPanel();
            }
            else
            {
                ClosePanel();
            }
        }
    }
}
