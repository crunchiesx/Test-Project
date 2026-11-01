using Crunchies.Components;
using Crunchies.InputActions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Crunchies.PlayerSystem
{
    public class PlayerCombat : MonoBehaviour
    {
        public UnityEvent OnAttackEvent;

        [Header("Reference")]
        [SerializeField] private Transform attackTransform;

        [Header("Settings")]
        [SerializeField] private float damage = 20f;
        [SerializeField] private float attackRange = 1f;
        [SerializeField] private float attackRate = 1f;
        [SerializeField] private LayerMask attackLayer;

        private bool isAttacking = false;
        private float currentRate = 0f;

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
            currentRate += Time.deltaTime;

            if (currentRate >= attackRate && isAttacking)
            {
                TriggerAttack();
                currentRate = 0f;
            }

            if (isHitWindowActive)
            {
                PerformSweepDetection();
            }
        }

        private void TriggerAttack()
        {
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