using System;
using Crunchies.InputActions;
using Crunchies.Managers;
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

        [Header("Camera Zoom Settings")]
        [SerializeField] private float maxCameraZoom = 5f;
        [SerializeField] private float minCameraZoom = 2f;

        [Space]

        [SerializeField] private float zoomSensitivity = 1f;
        [SerializeField] private float zoomSpeed = 5f;

        private float _currentCameraSensitivity;
        private float _targetRadius;

        private CinemachineInputAxisController _cinemachineAxisController;
        private CinemachineOrbitalFollow _cinemachineOrbitalFollow;

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

            if (!TryGetComponent(out _cinemachineAxisController))
            {
                Log.MissingComponent<CinemachineInputAxisController>(this);
            }

            if (!TryGetComponent(out _cinemachineOrbitalFollow))
            {
                Log.MissingComponent<CinemachineOrbitalFollow>(this);
            }

            _currentCameraSensitivity = cameraSensitivity;
            SetCameraSensitivity(_currentCameraSensitivity);
        }

        private void OnEnable()
        {
            UIPanel.OnPanelListUpdate += RefreshCameraInput;
            GameManager.Instance.OnPauseStateChange += RefreshCameraInput;
            PlayerInputHandler.Instance.OnCameraZoomAction += SetCameraZoom;
        }

        private void OnDisable()
        {
            UIPanel.OnPanelListUpdate -= RefreshCameraInput;
            GameManager.Instance.OnPauseStateChange -= RefreshCameraInput;
            PlayerInputHandler.Instance.OnCameraZoomAction -= SetCameraZoom;
        }

        private void Start()
        {
            RefreshCameraInput();

            _targetRadius = _cinemachineOrbitalFollow.Radius;
        }
        private void Update()
        {
            if (_currentCameraSensitivity != cameraSensitivity)
            {
                _currentCameraSensitivity = cameraSensitivity;
                SetCameraSensitivity(_currentCameraSensitivity);
            }

            _cinemachineOrbitalFollow.Radius = Mathf.Lerp
            (
                _cinemachineOrbitalFollow.Radius,
                _targetRadius,
                Time.deltaTime * zoomSpeed
            );
        }

        private void RefreshCameraInput()
        {
            bool enabled = !UIPanel.IsAnyPanelActive && !GameManager.Instance.IsPaused;
            SetCameraInputEnabled(enabled);
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

        private void SetCameraZoom(float zoomInput)
        {
            if (_cinemachineOrbitalFollow == null || GameManager.Instance.IsPaused || UIPanel.IsAnyPanelActive)
            {
                return;
            }

            _targetRadius -= zoomInput * zoomSensitivity;
            _targetRadius = Mathf.Clamp(_targetRadius, minCameraZoom, maxCameraZoom);
        }

        private bool TryGetController(string controllerName, out CinemachineInputAxisController.Controller controller)
        {
            controller = default;

            if (_cinemachineAxisController == null)
            {
                return false;
            }

            foreach (var candidate in _cinemachineAxisController.Controllers)
            {
                if (candidate.Name == controllerName)
                {
                    controller = candidate;
                    return true;
                }
            }

            return false;
        }
    }
}

