using System;
using Crunchies.PlayerSystem;
using UnityEngine;

namespace Crunchies.Components
{
    public class ItemPickup : MonoBehaviour
    {
        public event Action OnPickedUp;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent<Player>(out _))
            {
                OnPickedUp?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
