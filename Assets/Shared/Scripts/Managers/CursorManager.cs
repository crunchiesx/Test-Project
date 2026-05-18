using System;
using Crunchies.UI;
using UnityEngine;

namespace Crunchies.Managers
{
    public class CursorManager : MonoBehaviour
    {
        public static CursorManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnEnable() => UIPanel.OnPanelChange += OnPanelChange;
        private void OnDisable() => UIPanel.OnPanelChange -= OnPanelChange;

        private void Start() => SetGameplayCursor();

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

        private void OnPanelChange()
        {
            if (UIPanel.IsAnyPanelActive)
            {
                SetMenuCursor();
            }
            else
            {
                SetGameplayCursor();
            }
        }
    }
}
