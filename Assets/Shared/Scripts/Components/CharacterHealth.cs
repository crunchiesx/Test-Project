using System;
using Crunchies.Interfaces;
using Unity.Mathematics;
using UnityEngine;

namespace Crunchies.Components
{
    public class CharacterHealth : MonoBehaviour, IInteractable
    {
        public event Action OnDied;
        public event Action OnInteract;

        [SerializeField] private float maxHealth = 100f;

        private float currentHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);

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

        public void Interact()
        {
            OnInteract?.Invoke();
        }
    }
}
