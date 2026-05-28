// ============================================================
// Script: "InteractQuestBridge.cs"
// ============================================================
// Attach alongside your existing Interactable (or UsableObject)
// component on an interactive prop. Listens for the interact
// event and forwards it to the quest system.
// ============================================================
// Example GameObject layout:
//   Lever_01
//   ├── LeverMechanism.cs   ← rotates lever, opens a door
//   ├── Interactable.cs     ← handles E-key prompt, calls OnUsed
//   └── InteractQuestBridge.cs  ← this file
// ============================================================
using Crunchies.Interfaces;
using Crunchies.ScriptableObjects;
using Crunchies.Utility;
using UnityEngine;

namespace Crunchies.QuestSystem
{
    [RequireComponent(typeof(IInteractable))]
    public class InteractQuestBridge : MonoBehaviour
    {
        [Tooltip("Must match the ObjectDataSO set in InteractObjective")]
        [SerializeField] private ObjectDataSO objectData;

        public IInteractable _interactable;

        private void Awake()
        {
            if (!TryGetComponent(out _interactable))
            {
                Log.MissingComponent(nameof(IInteractable), this);
            }
        }

        private void OnEnable() => _interactable.OnInteract += HandleInteract;
        private void OnDisable() => _interactable.OnInteract -= HandleInteract;

        private void HandleInteract() => QuestEvents.ObjectInteracted(objectData.objectId);
    }
}