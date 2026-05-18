using System;
using System.Collections.Generic;
using Crunchies.Utility;
using UnityEngine;

namespace Crunchies.UI
{
    public abstract class UIPanel : MonoBehaviour
    {
        public static List<UIPanel> ActivePanels = new();

        public static event Action OnPanelChange;

        public static bool IsAnyPanelActive => ActivePanels.Count > 0;

        protected virtual void OnEnable()
        {
            if (!ActivePanels.Contains(this))
            {
                Log.Info("Panel Added!", this);
                ActivePanels.Add(this);
                OnPanelChange?.Invoke();
            }
        }

        protected virtual void OnDisable()
        {
            if (ActivePanels.Contains(this))
            {
                Log.Info("Panel Removed!", this);
                ActivePanels.Remove(this);
                OnPanelChange?.Invoke();
            }

            ActivePanels.RemoveAll(panel => panel == null);
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
