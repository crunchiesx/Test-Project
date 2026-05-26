using System;
using Crunchies.Utility;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Crunchies.InputActions
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public static PlayerInputHandler Instance { get; private set; }

        public event Action OnUIQuestAction;
        public event Action OnUIEscapeAction;

        public event Action<Vector2> OnPlayerMovementAction;
        public event Action<Vector2> OnPlayerLookAction;

        public event Action<bool> OnPlayerJumpAction;
        public event Action<bool> OnPlayerSprintAction;
        public event Action<bool> OnPlayerInteractAction;
        public event Action<bool> OnPlayerAttackAction;

        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool JumpInput { get; private set; }
        public bool SprintInput { get; private set; }
        public bool InteractInput { get; private set; }
        public bool AttackInput { get; private set; }

        private PlayerInputActions _playerInputActions;

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

            _playerInputActions = new PlayerInputActions();
            SubscribePlayerEventActions();
            SubscribeUIEventActions();
        }

        private void OnEnable() => _playerInputActions?.Enable();
        private void OnDisable() => _playerInputActions?.Disable();

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;

            if (_playerInputActions != null)
            {
                UnsubscribePlayerEventActions();
                UnsubscribeUIEventActions();
                _playerInputActions.Dispose();
            }
        }

        private void SubscribePlayerEventActions()
        {
            _playerInputActions.Player.Move.performed += HandlePlayerMovement;
            _playerInputActions.Player.Move.canceled += HandlePlayerMovement;

            _playerInputActions.Player.Look.performed += HandlePlayerLook;
            _playerInputActions.Player.Look.canceled += HandlePlayerLook;

            _playerInputActions.Player.Jump.performed += HandlePlayerJump;
            _playerInputActions.Player.Jump.canceled += HandlePlayerJump;

            _playerInputActions.Player.Sprint.performed += HandlePlayerSprint;
            _playerInputActions.Player.Sprint.canceled += HandlePlayerSprint;

            _playerInputActions.Player.Interact.performed += HandlePlayerInteract;
            _playerInputActions.Player.Interact.canceled += HandlePlayerInteract;

            _playerInputActions.Player.Attack.performed += HandlePlayerAttack;
            _playerInputActions.Player.Attack.canceled += HandlePlayerAttack;
        }

        private void UnsubscribePlayerEventActions()
        {
            _playerInputActions.Player.Move.performed -= HandlePlayerMovement;
            _playerInputActions.Player.Move.canceled -= HandlePlayerMovement;

            _playerInputActions.Player.Look.performed -= HandlePlayerLook;
            _playerInputActions.Player.Look.canceled -= HandlePlayerLook;

            _playerInputActions.Player.Jump.performed -= HandlePlayerJump;
            _playerInputActions.Player.Jump.canceled -= HandlePlayerJump;

            _playerInputActions.Player.Sprint.performed -= HandlePlayerSprint;
            _playerInputActions.Player.Sprint.canceled -= HandlePlayerSprint;

            _playerInputActions.Player.Interact.performed -= HandlePlayerInteract;
            _playerInputActions.Player.Interact.canceled -= HandlePlayerInteract;

            _playerInputActions.Player.Attack.performed -= HandlePlayerAttack;
            _playerInputActions.Player.Attack.canceled -= HandlePlayerAttack;
        }

        private void SubscribeUIEventActions()
        {
            _playerInputActions.UI.Quest.performed += HandleUIQuest;
            _playerInputActions.UI.Escape.performed += HandleUIEscape;
        }

        private void UnsubscribeUIEventActions()
        {
            _playerInputActions.UI.Quest.performed -= HandleUIQuest;
            _playerInputActions.UI.Escape.performed -= HandleUIEscape;
        }

        private void HandlePlayerMovement(InputAction.CallbackContext ctx)
        {
            MoveInput = ctx.ReadValue<Vector2>();
            OnPlayerMovementAction?.Invoke(MoveInput);
        }

        private void HandlePlayerLook(InputAction.CallbackContext ctx)
        {
            LookInput = ctx.ReadValue<Vector2>();
            OnPlayerLookAction?.Invoke(LookInput);
        }

        private void HandlePlayerJump(InputAction.CallbackContext ctx)
        {
            JumpInput = ctx.ReadValueAsButton();
            OnPlayerJumpAction?.Invoke(JumpInput);
        }

        private void HandlePlayerSprint(InputAction.CallbackContext ctx)
        {
            SprintInput = ctx.ReadValueAsButton();
            OnPlayerSprintAction?.Invoke(SprintInput);
        }

        private void HandlePlayerInteract(InputAction.CallbackContext ctx)
        {
            InteractInput = ctx.ReadValueAsButton();
            OnPlayerInteractAction?.Invoke(InteractInput);
        }

        private void HandlePlayerAttack(InputAction.CallbackContext ctx)
        {
            AttackInput = ctx.ReadValueAsButton();
            OnPlayerAttackAction?.Invoke(AttackInput);
        }

        private void HandleUIQuest(InputAction.CallbackContext _) => OnUIQuestAction?.Invoke();
        private void HandleUIEscape(InputAction.CallbackContext _) => OnUIEscapeAction?.Invoke();
    }
}
