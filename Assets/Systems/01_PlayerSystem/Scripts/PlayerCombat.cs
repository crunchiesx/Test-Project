using Crunchies.Components;
using Crunchies.InputActions;
using UnityEngine;

namespace Crunchies.PlayerSystem
{
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private Transform attackTransform;

        [Header("Settings")]
        [SerializeField] private float damage = 20f;
        [SerializeField] private float attackRange = 1f;
        [SerializeField] private float attackRate = 1f;
        [SerializeField] private LayerMask attackLayer;

        private bool isAttacking = false;
        private float currentRate = 0f;

        private readonly Collider[] resultsBuffer = new Collider[32];

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
                HandleAttack();
                currentRate = 0f;
            }
        }

        private void HandleAttack()
        {
            int hits = Physics.OverlapSphereNonAlloc(attackTransform.position, attackRange, resultsBuffer, attackLayer);

            for (int i = 0; i < hits; i++)
            {
                if (resultsBuffer[i].transform.TryGetComponent(out CharacterHealth health))
                {
                    health.TakeDamage(damage);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackTransform.position, attackRange);
        }
    }
}
