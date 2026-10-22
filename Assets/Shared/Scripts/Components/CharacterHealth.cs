using System;
using Crunchies.Interfaces;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Crunchies.Components
{
    public class CharacterHealth : MonoBehaviour
    {
        public event Action OnDied;

        public UnityEvent OnHitEvent;

        [Space]

        [SerializeField] private float maxHealth = 100f;

        private float currentHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
            OnHitEvent?.Invoke();

            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        private void Die()
        {
            OnDied?.Invoke();
            Destroy(gameObject);
        }
    }
}
