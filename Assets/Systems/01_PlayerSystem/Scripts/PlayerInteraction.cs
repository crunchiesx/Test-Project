using System;
using Crunchies.InputActions;
using Crunchies.Interfaces;
using Crunchies.Utility;
using UnityEngine;

namespace Crunchies.PlayerSystem
{
    public enum InteractType
    {
        Raycast,
        OverlapBox
    }

    public class PlayerInteraction : MonoBehaviour
    {
        public static event Action<bool> OnInteractionChanged;

        [Header("References")]
        [SerializeField] private Transform interactTransform;

        [Header("Interaction Settings")]
        [SerializeField] private InteractType interactType = InteractType.Raycast;
        [SerializeField] private float raycastRange = 1f;
        [SerializeField] private Vector3 boxZoneSize = new(1f, 1f, 1f);

        private IInteractable currentInteractable;
        private IInteractable previousInteractable;

        private readonly Collider[] _interactionHitBuffer = new Collider[64];

        private void OnEnable()
        {
            if (PlayerInputHandler.Instance != null)
            {
                PlayerInputHandler.Instance.OnPlayerInteractAction += Interact;
            }
        }

        private void OnDisable()
        {
            if (PlayerInputHandler.Instance != null)
            {
                PlayerInputHandler.Instance.OnPlayerInteractAction -= Interact;
            }
        }

        private void Update()
        {
            if (interactTransform == null) return;

            previousInteractable = currentInteractable;

            currentInteractable = interactType switch
            {
                InteractType.Raycast => PerformRaycast(),
                InteractType.OverlapBox => PerformOverlapBox(),
                _ => throw new System.NotImplementedException()
            };

            if (currentInteractable != null && currentInteractable != previousInteractable)
            {
                OnInteractionChanged?.Invoke(true);
            }
            else if (currentInteractable == null && previousInteractable != null)
            {
                OnInteractionChanged?.Invoke(false);
            }
        }

        private void Interact()
        {
            if (currentInteractable == null) return;
            currentInteractable?.Interact();
        }

        private IInteractable PerformRaycast()
        {
            if (Physics.Raycast(interactTransform.position, interactTransform.forward, out RaycastHit hit, raycastRange))
            {
                Collider hitCollider = hit.collider;

                if (hitCollider != null)
                {
                    if (hitCollider.TryGetComponent(out IInteractable interactable))
                    {
                        return interactable;
                    }

                    interactable = hitCollider.GetComponentInParent<IInteractable>();
                    if (interactable != null)
                    {
                        return interactable;
                    }
                }
            }

            return null;
        }

        private IInteractable PerformOverlapBox()
        {
            Vector3 halfExtents = boxZoneSize * 0.5f;
            int hitCount = Physics.OverlapBoxNonAlloc
            (
                interactTransform.position,
                halfExtents,
                _interactionHitBuffer,
                interactTransform.rotation
            );

            IInteractable closest = null;
            float closestDistance = float.PositiveInfinity;
            for (int i = 0; i < hitCount; i++)
            {
                Collider collider = _interactionHitBuffer[i];
                if (collider == null) continue;

                if (!collider.TryGetComponent(out IInteractable interactable))
                {
                    interactable = collider.GetComponentInParent<IInteractable>();
                }

                if (interactable == null) continue;

                Vector3 closestPoint = collider.ClosestPoint(transform.position);
                Vector3 directionToTarget = closestPoint - transform.position;
                float distance = directionToTarget.sqrMagnitude;

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = interactable;
                }
            }

            return closest;
        }

        private void OnDrawGizmosSelected()
        {
            if (interactTransform == null) return;

            Gizmos.color = Color.yellow;

            switch (interactType)
            {
                case InteractType.Raycast:
                    Gizmos.DrawRay(interactTransform.position, interactTransform.forward * raycastRange);
                    break;
                case InteractType.OverlapBox:
                    Gizmos.DrawWireCube(interactTransform.position, boxZoneSize);
                    break;
            }
        }
    }
}
