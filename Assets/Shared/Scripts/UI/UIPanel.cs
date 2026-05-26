using System;
using System.Collections.Generic;
using Crunchies.Utility;
using UnityEngine;

namespace Crunchies.UI
{
    public abstract class UIPanel : MonoBehaviour
    {
        private static readonly List<UIPanel> ActivePanels = new();

        public static event Action OnPanelChange;
        public static bool IsAnyPanelActive => ActivePanels.Count > 0;

        [Header("Base")]
        [SerializeField] private GameObject panel;

        public bool IsOpen => panel.activeSelf;

        public static void CloseRecentActivePanel()
        {
            ActivePanels.RemoveAll(panel => panel == null);

            if (ActivePanels.Count == 0) return;

            UIPanel recentPanel = ActivePanels[^1];
            recentPanel.Close();
        }

        public virtual void Open()
        {
            ActivePanels.RemoveAll(panel => panel == null);

            if (ActivePanels.Contains(this))
            {
                ActivePanels.Remove(this);
            }

            Log.Info("Panel Added!", this);
            ActivePanels.Add(this);
            OnPanelChange?.Invoke();

            panel.SetActive(true);
        }

        public virtual void Close()
        {
            if (ActivePanels.Contains(this))
            {
                Log.Info("Panel Removed!", this);
                ActivePanels.Remove(this);
                OnPanelChange?.Invoke();
            }

            panel.SetActive(false);

            ActivePanels.RemoveAll(panel => panel == null);
        }
    }
}
