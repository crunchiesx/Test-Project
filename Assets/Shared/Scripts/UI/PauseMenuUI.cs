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
            Log.Info("Resume Pressed");

            GameManager.Instance.TogglePause();
            ClosePanel();
        }

        private void OnControlsButtonPressed()
        {
            Log.Info("Controls Panel Pressed");
        }

        private void OnOptionsButtonPressed()
        {
            Log.Info("Options Panel Pressed");
        }
    }
}
