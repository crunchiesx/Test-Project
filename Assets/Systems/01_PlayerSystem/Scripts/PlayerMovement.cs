using UnityEngine;
using Crunchies.InputActions;
using Crunchies.Utility;
using System;
using UnityEngine.TextCore.Text;

namespace Crunchies.PlayerSystem
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        public readonly float GRAVITY = Physics.gravity.y;

        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float sprintMultiplier = 2f;
        [SerializeField] private float jumpHeight = 3f;
        [SerializeField] private float weight = 2f;

        [Header("Rotation Settings")]
        [SerializeField] private float turnSmoothTime = 0.1f;

        [Header("Ground Settings")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private float groundDistance;
        [SerializeField] private float groundForce = 2f;

        private CharacterController _characterController;

        private bool _isGrounded;
        private bool _isMoving;
        private bool _isJumping;
        private bool _isSprinting;

        private float turnSoothVelocity;
        private Vector2 _moveDirection;

        private Vector3 _velocity;
        private Vector3 _lastPosition = Vector3.zero;

        private void Awake()
        {
            if (!TryGetComponent(out _characterController))
            {
                Log.MissingComponent<CharacterController>(this);
            }
        }

        private void OnEnable()
        {
            if (PlayerInputHandler.Instance != null)
            {
                PlayerInputHandler.Instance.OnMovementAction += HandleMovement;
                PlayerInputHandler.Instance.OnJumpAction += HandleJump;
                PlayerInputHandler.Instance.OnSprintAction += HandleSprint;
            }
        }

        private void OnDisable()
        {
            if (PlayerInputHandler.Instance != null)
            {
                PlayerInputHandler.Instance.OnMovementAction -= HandleMovement;
                PlayerInputHandler.Instance.OnJumpAction -= HandleJump;
                PlayerInputHandler.Instance.OnSprintAction -= HandleSprint;
            }
        }

        private void HandleMovement(Vector2 value) => _moveDirection = value;
        private void HandleJump(bool value) => _isJumping = value;
        private void HandleSprint(bool value) => _isSprinting = value;

        private void Update()
        {
            _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -groundForce;
            }

            if (_moveDirection.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(_moveDirection.x, _moveDirection.y) * Mathf.Rad2Deg + PlayerCamera.Instance.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 direction = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                float currentSpeed = _isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;
                _characterController.Move(currentSpeed * Time.deltaTime * direction.normalized);
            }

            if (_isJumping && _isGrounded)
            {
                _velocity.y = Mathf.Sqrt(-2 * GRAVITY * jumpHeight);
            }

            _velocity.y += GRAVITY * weight * Time.deltaTime;

            _characterController.Move(_velocity * Time.deltaTime);

            _isMoving = _lastPosition != transform.position;
            _lastPosition = transform.position;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}
