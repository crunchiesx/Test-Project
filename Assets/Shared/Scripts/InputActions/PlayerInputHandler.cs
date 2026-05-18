using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Crunchies.InputActions
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public static PlayerInputHandler Instance { get; private set; }

        public event Action OnQuestAction;

        public event Action<Vector2> OnMovementAction;
        public event Action<Vector2> OnLookAction;

        public event Action<bool> OnJumpAction;
        public event Action<bool> OnSprintAction;
        public event Action<bool> OnInteractAction;
        public event Action<bool> OnAttackAction;

        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool JumpInput { get; private set; }
        public bool SprintInput { get; private set; }
        public bool InteractInput { get; private set; }
        public bool AttackInput { get; private set; }

        private PlayerInputActions _playerInputActions;

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
            _playerInputActions.Player.Move.performed += HandleMovement;
            _playerInputActions.Player.Move.canceled += HandleMovement;

            _playerInputActions.Player.Look.performed += HandleLook;
            _playerInputActions.Player.Look.canceled += HandleLook;

            _playerInputActions.Player.Jump.performed += HandleJump;
            _playerInputActions.Player.Jump.canceled += HandleJump;

            _playerInputActions.Player.Sprint.performed += HandleSprint;
            _playerInputActions.Player.Sprint.canceled += HandleSprint;

            _playerInputActions.Player.Interact.performed += HandleInteract;
            _playerInputActions.Player.Interact.canceled += HandleInteract;

            _playerInputActions.Player.Attack.performed += HandleAttack;
            _playerInputActions.Player.Attack.canceled += HandleAttack;
        }

        private void UnsubscribePlayerEventActions()
        {
            _playerInputActions.Player.Move.performed -= HandleMovement;
            _playerInputActions.Player.Move.canceled -= HandleMovement;

            _playerInputActions.Player.Look.performed -= HandleLook;
            _playerInputActions.Player.Look.canceled -= HandleLook;

            _playerInputActions.Player.Jump.performed -= HandleJump;
            _playerInputActions.Player.Jump.canceled -= HandleJump;

            _playerInputActions.Player.Sprint.performed -= HandleSprint;
            _playerInputActions.Player.Sprint.canceled -= HandleSprint;

            _playerInputActions.Player.Interact.performed -= HandleInteract;
            _playerInputActions.Player.Interact.canceled -= HandleInteract;

            _playerInputActions.Player.Attack.performed -= HandleAttack;
            _playerInputActions.Player.Attack.canceled -= HandleAttack;
        }

        private void SubscribeUIEventActions()
        {
            _playerInputActions.UI.Quest.performed += HandleQuest;
        }

        private void UnsubscribeUIEventActions()
        {
            _playerInputActions.UI.Quest.performed -= HandleQuest;
        }

        private void HandleMovement(InputAction.CallbackContext ctx)
        {
            MoveInput = ctx.ReadValue<Vector2>();
            OnMovementAction?.Invoke(MoveInput);
        }

        private void HandleLook(InputAction.CallbackContext ctx)
        {
            LookInput = ctx.ReadValue<Vector2>();
            OnLookAction?.Invoke(LookInput);
        }

        private void HandleJump(InputAction.CallbackContext ctx)
        {
            JumpInput = ctx.ReadValueAsButton();
            OnJumpAction?.Invoke(JumpInput);
        }

        private void HandleSprint(InputAction.CallbackContext ctx)
        {
            SprintInput = ctx.ReadValueAsButton();
            OnSprintAction?.Invoke(SprintInput);
        }

        private void HandleInteract(InputAction.CallbackContext ctx)
        {
            InteractInput = ctx.ReadValueAsButton();
            OnInteractAction?.Invoke(InteractInput);
        }

        private void HandleAttack(InputAction.CallbackContext ctx)
        {
            AttackInput = ctx.ReadValueAsButton();
            OnAttackAction?.Invoke(AttackInput);
        }

        private void HandleQuest(InputAction.CallbackContext _) => OnQuestAction?.Invoke();
    }
}
