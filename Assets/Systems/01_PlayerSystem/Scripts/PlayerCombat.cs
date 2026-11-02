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

        private bool isAttacking = false;
        private bool canAttack = true;

        private bool isHitWindowActive = false;
        private Vector3 previousAttackPosition;
        private readonly HashSet<Collider> hitTargetsThisSwing = new();

        private readonly RaycastHit[] castBuffer = new RaycastHit[32];
        private readonly Collider[] overlapBuffer = new Collider[32];

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

        private void HandleAttack(bool value) => isAttacking = value;

        private void Update()
        {
            if (canAttack && isAttacking)
            {
                TriggerAttack();
            }
            if (isHitWindowActive)
            {
                PerformSweepDetection();
            }
        }

        private void TriggerAttack()
        {
            canAttack = false;
            OnAttackEvent?.Invoke();
        }

        private void PerformSweepDetection()
        {
            Vector3 currentPos = attackTransform.position;
            Vector3 direction = currentPos - previousAttackPosition;
            float distance = direction.magnitude;

            if (distance > 0.001f)
            {
                int hitCount = Physics.SphereCastNonAlloc
                (
                    previousAttackPosition,
                    attackRange,
                    direction.normalized,
                    castBuffer,
                    distance,
                    attackLayer
                );

                for (int i = 0; i < hitCount; i++)
                {
                    Collider col = castBuffer[i].collider;
                    ProcessHit(col);
                }
            }
            else
            {
                int hitCount = Physics.OverlapSphereNonAlloc
                (
                    currentPos,
                    attackRange,
                    overlapBuffer,
                    attackLayer
                );

                for (int i = 0; i < hitCount; i++)
                {
                    ProcessHit(overlapBuffer[i]);
                }
            }

            previousAttackPosition = currentPos;
        }

        private void ProcessHit(Collider col)
        {
            if (col == null) return;
            if (hitTargetsThisSwing.Contains(col)) return;

            if (col.TryGetComponent(out CharacterHealth health))
            {
                hitTargetsThisSwing.Add(col);
                health.TakeDamage(damage);
            }
        }

        public void OnAttackSwingStart()
        {
            isHitWindowActive = true;
            hitTargetsThisSwing.Clear();
            previousAttackPosition = attackTransform.position;
        }

        public void OnAttackSwingEnd()
        {
            isHitWindowActive = false;
        }

        public void OnAttackFinished()
        {
            canAttack = true;
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
                    if (Application.isPlaying && isHitWindowActive)
                    {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawWireSphere(previousAttackPosition, attackRange);
                        Gizmos.DrawLine(previousAttackPosition, attackTransform.position);
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