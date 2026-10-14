using System;
using Crunchies.Managers;
using Crunchies.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Crunchies.UI
{
    public class PauseMenuUI : UIPanel
    {
        [Header("UI References")]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button controlsButton;
        [SerializeField] private Button optionsButton;

        [Space]
        [SerializeField] private ControlsUI controlsUI;
        [SerializeField] private OptionsUI optionsUI;

        protected override void Awake()
        {
            base.Awake();

            resumeButton.onClick.AddListener(OnResumeButtonPressed);
            controlsButton.onClick.AddListener(OnControlsButtonPressed);
            optionsButton.onClick.AddListener(OnOptionsButtonPressed);
        }

        private void OnEnable() => GameManager.Instance.OnPauseStateChange += OnPauseStateChange;
        private void OnDisable() => GameManager.Instance.OnPauseStateChange -= OnPauseStateChange;

        private void OnPauseStateChange()
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

        private void OnResumeButtonPressed()
        {
            GameManager.Instance.TogglePause();
            ClosePanel();
        }

        private void OnControlsButtonPressed()
        {
            if (controlsUI.IsOpen)
            {
                controlsUI.ClosePanel();
            }
            else
            {
                controlsUI.OpenPanel();
            }
        }

        private void OnOptionsButtonPressed()
        {
            if (optionsUI.IsOpen)
            {
                optionsUI.ClosePanel();
            }
            else
            {
                optionsUI.OpenPanel();
            }
        }
    }
}
