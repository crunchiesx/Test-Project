// ============================================================
// Script: "ItemPickupQuestBridge.cs"
// ============================================================
// Attach alongside your existing ItemPickup (or Collectible)
// component on a pickup GameObject. Listens for the pickup
// event and forwards it to the quest system.
// ============================================================
// Example GameObject layout:
//   Herb_Red
//   ├── ItemPickup.cs            ← adds item to inventory, plays sound
//   └── ItemPickupQuestBridge.cs ← this file
// ============================================================
using Crunchies.Components;
using Crunchies.ScriptableObjects;
using Crunchies.Utility;
using UnityEngine;

namespace Crunchies.QuestSystem
{
    [RequireComponent(typeof(ItemPickup))]
    public class ItemPickupQuestBridge : MonoBehaviour
    {
        [Tooltip("Must match the ItemDataSO set in GatherObjective")]
        [SerializeField] private ItemDataSO itemData;
        [SerializeField] private int amount = 1;

        private ItemPickup _pickup;

        private void Awake()
        {
            if (!TryGetComponent(out _pickup))
            {
                Log.MissingComponent<ItemPickup>(this);
            }
        }

        private void OnEnable() => _pickup.OnPickedUp += HandlePickup;
        private void OnDisable() => _pickup.OnPickedUp -= HandlePickup;

        private void HandlePickup() => QuestEvents.ItemCollected(itemData.itemId, amount);
    }
}