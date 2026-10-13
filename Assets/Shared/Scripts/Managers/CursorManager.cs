using System;
using Crunchies.UI;
using Crunchies.Utility;
using UnityEngine;

namespace Crunchies.Managers
{
    public class CursorManager : MonoBehaviour
    {
        public static CursorManager Instance { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetInstance()
        {
            Instance = null;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnEnable()
        {
            UIPanel.OnPanelChange += SetCursorState;
            GameManager.Instance.OnPauseStateChange += SetCursorState;
        }

        private void OnDisable()
        {
            UIPanel.OnPanelChange -= SetCursorState;
            GameManager.Instance.OnPauseStateChange -= SetCursorState;
        }

        private void Start() => SetCursorState();

        private void SetCursorState()
        {
            if (UIPanel.IsAnyPanelActive || GameManager.Instance.IsPaused)
            {
                SetMenuCursor();
            }
            else
            {
                SetGameplayCursor();
            }
        }

        public void SetGameplayCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void SetMenuCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void SetConfinedCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
