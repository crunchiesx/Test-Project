using Crunchies.Components;
using Crunchies.InputActions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Crunchies.PlayerSystem
{
    public class PlayerCombat : MonoBehaviour
    {
        private static readonly int AttackSpeedHash = Animator.StringToHash("AttackSpeed");

        public UnityEvent OnAttackEvent;

        [Header("Reference")]
        [SerializeField] private Transform attackTransform;
        [SerializeField] private Animator playerAnimator;

        [Header("Settings")]
        [SerializeField] private float damage = 20f;
        [SerializeField] private float attackRange = 1f;
        [SerializeField] private LayerMask attackLayer;

        private bool _isAttacking = false;
        private bool _canAttack = true;

        private bool _isHitWindowActive = false;
        private Vector3 _previousAttackPosition;
        private readonly HashSet<Collider> _hitTargetsThisSwing = new();

        private readonly RaycastHit[] _castBuffer = new RaycastHit[32];
        private readonly Collider[] _overlapBuffer = new Collider[32];

        private PlayerMovement _playerMovement;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }

        private void OnEnable()
        {
            if (PlayerInputHandler.Instance != null)
            {
                PlayerInputHandler.Instance.OnPlayerAttackAction += HandleAttack;
            }
        }

        private void OnDisable()
        {
            if (PlayerInputHandler.Instance != null)
            {
                PlayerInputHandler.Instance.OnPlayerAttackAction -= HandleAttack;
            }
        }

        private void HandleAttack(bool value) => _isAttacking = value;

        private void Update()
        {
            if (_canAttack && _isAttacking)
            {
                TriggerAttack();
            }
            if (_isHitWindowActive)
            {
                PerformSweepDetection();
            }
        }

        private void PerformSweepDetection()
        {
            Vector3 currentPos = attackTransform.position;
            Vector3 direction = currentPos - _previousAttackPosition;
            float distance = direction.magnitude;

            if (distance > 0.001f)
            {
                int hitCount = Physics.SphereCastNonAlloc
                (
                    _previousAttackPosition,
                    attackRange,
                    direction.normalized,
                    _castBuffer,
                    distance,
                    attackLayer
                );

                for (int i = 0; i < hitCount; i++)
                {
                    Collider col = _castBuffer[i].collider;
                    ProcessHit(col);
                }
            }
            else
            {
                int hitCount = Physics.OverlapSphereNonAlloc
                (
                    currentPos,
                    attackRange,
                    _overlapBuffer,
                    attackLayer
                );

                for (int i = 0; i < hitCount; i++)
                {
                    ProcessHit(_overlapBuffer[i]);
                }
            }

            _previousAttackPosition = currentPos;
        }

        private void ProcessHit(Collider col)
        {
            if (col == null) return;
            if (_hitTargetsThisSwing.Contains(col)) return;

            if (col.TryGetComponent(out CharacterHealth health))
            {
                _hitTargetsThisSwing.Add(col);
                health.TakeDamage(damage);
            }
        }

        private void TriggerAttack()
        {
            _canAttack = false;
            _playerMovement.SetCameraRotationOverride(true);
            OnAttackEvent?.Invoke();
        }

        public void OnAttackSwingStart()
        {
            _isHitWindowActive = true;
            _hitTargetsThisSwing.Clear();
            _previousAttackPosition = attackTransform.position;
        }

        public void OnAttackSwingEnd()
        {
            _isHitWindowActive = false;
        }

        public void OnAttackFinished()
        {
            _canAttack = true;
            _playerMovement.SetCameraRotationOverride(false);
        }

        public void SetAttackSpeed(float multiplier)
        {
            multiplier = Mathf.Clamp(multiplier, 0.5f, 3f);
            playerAnimator.SetFloat(AttackSpeedHash, multiplier);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (attackTransform == null) return;

            GameObject selected = UnityEditor.Selection.activeGameObject;

            if (selected != null)
            {
                if (selected == gameObject || selected.transform.IsChildOf(transform))
                {
                    if (Application.isPlaying && _isHitWindowActive)
                    {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawWireSphere(_previousAttackPosition, attackRange);
                        Gizmos.DrawLine(_previousAttackPosition, attackTransform.position);
                        Gizmos.color = Color.green;
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                    }

                    Gizmos.DrawWireSphere(attackTransform.position, attackRange);
                }
            }
        }
#endif
    }
}