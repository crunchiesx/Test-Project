using System;
using Crunchies.Components;
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
        public static event Action<bool, Interactable> OnInteractionUpdate;

        [Header("References")]
        [SerializeField] private Transform interactionPivot;
        [SerializeField] private Transform raycastInteraction;
        [SerializeField] private Transform overlapBoxInteraction;

        [Header("Interaction Settings")]
        [SerializeField] private InteractType interactType = InteractType.Raycast;
        [SerializeField] private float raycastRange = 1f;
        [SerializeField] private Vector3 boxZoneSize = new(1f, 1f, 1f);
        [Space]
        [SerializeField] private float pivotRotationSpeed = 5f;

        private Interactable currentInteractable;
        private Interactable previousInteractable;

        private readonly Collider[] _interactionHitBuffer = new Collider[64];

        private void Awake()
        {
            if (interactionPivot == null)
            {
                Log.MissingReference<Transform>(this);
            }

            if (raycastInteraction == null && overlapBoxInteraction == null)
            {
                Log.MissingReference<Transform>(this);
            }

            if (raycastInteraction == null)
            {
                interactType = InteractType.OverlapBox;
            }
            else if (overlapBoxInteraction == null)
            {
                interactType = InteractType.Raycast;
            }
        }

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
            SyncPivotToCameraYaw();

            previousInteractable = currentInteractable;

            currentInteractable = interactType switch
            {
                InteractType.Raycast => PerformRaycast(),
                InteractType.OverlapBox => PerformOverlapBox(),
                _ => throw new System.NotImplementedException()
            };

            EvaluateInteractionState();
        }

        private void Interact()
        {
            if (currentInteractable != null)
            {
                currentInteractable.Interact();
            }
        }

        private void EvaluateInteractionState()
        {
            if (currentInteractable != null && currentInteractable != previousInteractable && currentInteractable.IsInteractable())
            {
                OnInteractionUpdate?.Invoke(true, currentInteractable);
            }
            else if (currentInteractable == null && previousInteractable != null)
            {
                OnInteractionUpdate?.Invoke(false, currentInteractable);
            }
        }

        private Interactable PerformRaycast()
        {
            if (Physics.Raycast(raycastInteraction.position, raycastInteraction.forward, out RaycastHit hit, raycastRange))
            {
                Collider hitCollider = hit.collider;

                if (hitCollider != null)
                {
                    if (hitCollider.TryGetComponent(out Interactable interactable))
                    {
                        return interactable;
                    }

                    interactable = hitCollider.GetComponentInParent<Interactable>();
                    if (interactable != null)
                    {
                        return interactable;
                    }
                }
            }

            return null;
        }

        private Interactable PerformOverlapBox()
        {
            Vector3 halfExtents = boxZoneSize * 0.5f;
            int hitCount = Physics.OverlapBoxNonAlloc
            (
                overlapBoxInteraction.position,
                halfExtents,
                _interactionHitBuffer,
                overlapBoxInteraction.rotation
            );

            Interactable closest = null;
            float closestDistance = float.PositiveInfinity;
            for (int i = 0; i < hitCount; i++)
            {
                Collider collider = _interactionHitBuffer[i];
                if (collider == null) continue;

                if (!collider.TryGetComponent(out Interactable interactable))
                {
                    interactable = collider.GetComponentInParent<Interactable>();
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

        private void SyncPivotToCameraYaw()
        {
            if (interactionPivot == null) return;

            Camera cam = Camera.main;
            if (cam == null) return;

            float cameraYaw = cam.transform.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0f, cameraYaw, 0f);
            interactionPivot.rotation = Quaternion.Slerp(interactionPivot.rotation, targetRotation, pivotRotationSpeed * Time.deltaTime);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            GameObject selected = UnityEditor.Selection.activeGameObject;

            if (selected != null)
            {
                if (selected == gameObject || selected.transform.IsChildOf(transform))
                {
                    if (overlapBoxInteraction == null) return;

                    Gizmos.color = Color.yellow;

                    switch (interactType)
                    {
                        case InteractType.Raycast:
                            Gizmos.DrawRay(raycastInteraction.position, raycastInteraction.forward * raycastRange);
                            break;
                        case InteractType.OverlapBox:
                            Gizmos.DrawWireCube(overlapBoxInteraction.position, boxZoneSize);
                            break;
                    }
                }
            }
        }
#endif
    }
}
