using Crunchies.InputActions;
using Crunchies.Interfaces;
using UnityEngine;

namespace Crunchies.PlayerSystem
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform interactTransform;

        [Header("Settings")]
        [SerializeField] private float interactRange = 1f;

        private void OnEnable()
        {
            if (PlayerInputHandler.Instance != null)
            {
                PlayerInputHandler.Instance.OnInteractAction += Interact;
            }
        }

        private void OnDisable()
        {
            if (PlayerInputHandler.Instance != null)
            {
                PlayerInputHandler.Instance.OnInteractAction -= Interact;
            }
        }

        private void Interact(bool isInteracting)
        {
            if (!isInteracting) return;

            if (Physics.Raycast(interactTransform.position, interactTransform.forward, out RaycastHit hit, interactRange))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact();
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (interactTransform == null) return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(interactTransform.position, interactTransform.forward * interactRange);
        }
    }
}
