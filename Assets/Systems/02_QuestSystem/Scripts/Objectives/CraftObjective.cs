// ============================================================
// CraftObjective.cs
// ============================================================
// Complete when the player crafts N of a specific recipe.
// Driven by: QuestEvents.ItemCrafted(itemId, amount)
// Bridge: none needed - fire directly from your crafting system: QuestEvents.ItemCrafted("iron_sword", 1);
// ============================================================
using System;
using Crunchies.ScriptableObjects;
using UnityEngine;

namespace Crunchies.QuestSystem
{
    [Serializable]
    public class CraftObjective : QuestObjective
    {
        [Header("Craft Objective")]
        [SerializeField] private ItemDataSO itemData;

        public CraftObjective() { }

        public CraftObjective(ItemDataSO itemData, int count)
        {
            this.itemData = itemData;
            requiredAmount = count;
            description = $"Craft {count} {itemData.itemName}";
        }

        protected override void OnRegisterListeners() => QuestEvents.OnItemCrafted += OnItemCrafted;
        protected override void OnUnregisterListeners() => QuestEvents.OnItemCrafted -= OnItemCrafted;

        private void OnItemCrafted(string id, int amount)
        {
            if (id == itemData.itemId) AddProgress(amount);
        }

        public override string GetProgressText() => $"{itemData.itemName} crafted: {currentAmount:0} / {requiredAmount:0}";

#if UNITY_EDITOR
        public override void Validate()
        {
            if (itemData == null)
            {
                base.Validate();
                return;
            }

            description = $"Craft {requiredAmount} {itemData.itemName}";
        }
#endif
    }
}
