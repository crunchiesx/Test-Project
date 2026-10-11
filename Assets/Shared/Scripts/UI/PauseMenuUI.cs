using Crunchies.Managers;
using UnityEngine;

namespace Crunchies.UI
{
    public class PauseMenuUI : UIPanel
    {
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
