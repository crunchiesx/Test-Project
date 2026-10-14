using System;
using Crunchies.InputActions;
using Crunchies.UI;
using Crunchies.Utility;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Crunchies.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public event Action OnPauseStateChange;

        public bool IsPaused { get; private set; } = false;

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

        public void TogglePause()
        {
            IsPaused = !IsPaused;
            Time.timeScale = IsPaused ? 0 : 1;

            OnPauseStateChange?.Invoke();
        }
    }
}
