using System;
using Crunchies.UI;
using Crunchies.Utility;
using Unity.Cinemachine;
using UnityEngine;

namespace Crunchies.PlayerSystem
{
    [RequireComponent(typeof(CinemachineInputAxisController))]
    public class PlayerCamera : MonoBehaviour
    {
        private const string LOOK_ORBIT_X = "Look Orbit X";
        private const string LOOK_ORBIT_Y = "Look Orbit Y";

        public static PlayerCamera Instance { get; private set; }

        [Header("Camera Settings")]
        [SerializeField] private float cameraSensitivity = 5f;

        private float _currentCameraSensitivity;

        private CinemachineInputAxisController _cineAxisController;

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

            if (!TryGetComponent(out _cineAxisController))
            {
                Log.MissingComponent<CinemachineInputAxisController>(this);
            }

            _currentCameraSensitivity = cameraSensitivity;
            SetCameraSensitivity(_currentCameraSensitivity);
        }

        private void OnEnable() => UIPanel.OnPanelChange += OnPanelChange;
        private void OnDisable() => UIPanel.OnPanelChange -= OnPanelChange;

        private void Start() => OnPanelChange();

        private void Update()
        {
            if (_currentCameraSensitivity != cameraSensitivity)
            {
                _currentCameraSensitivity = cameraSensitivity;
                SetCameraSensitivity(_currentCameraSensitivity);
            }
        }

        private void SetCameraSensitivity(float sens)
        {
            if (!TryGetController(LOOK_ORBIT_X, out var lookOrbitX) || !TryGetController(LOOK_ORBIT_Y, out var lookOrbitY))
            {
                return;
            }

            lookOrbitX.Input.Gain = sens;
            lookOrbitY.Input.Gain = -sens;
        }

        private void SetCameraInputEnabled(bool enable)
        {
            if (!TryGetController(LOOK_ORBIT_X, out var lookOrbitX) || !TryGetController(LOOK_ORBIT_Y, out var lookOrbitY))
            {
                return;
            }

            lookOrbitX.Enabled = enable;
            lookOrbitY.Enabled = enable;
        }

        private bool TryGetController(string controllerName, out CinemachineInputAxisController.Controller controller)
        {
            controller = default;

            if (_cineAxisController == null)
            {
                return false;
            }

            foreach (var candidate in _cineAxisController.Controllers)
            {
                if (candidate.Name == controllerName)
                {
                    controller = candidate;
                    return true;
                }
            }

            return false;
        }

        private void OnPanelChange() => SetCameraInputEnabled(!UIPanel.IsAnyPanelActive);
    }
}

