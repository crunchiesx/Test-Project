// ============================================================
// Script: "GatherObjective.cs"
// ============================================================
// Complete when the player collect N of specific item.
// Driven by: QuestEvents.ItemCollected(itemId, amount)
// Bridge:    ItemPickupQuestBridge.cs
// ============================================================
using System;
using Crunchies.ScriptableObjects;
using UnityEngine;

namespace Crunchies.QuestSystem
{
    [Serializable]
    public class GatherObjective : QuestObjective
    {
        [Header("Gather Objective")]
        [SerializeField] private ItemDataSO itemData;

        public GatherObjective() { }

        public GatherObjective(ItemDataSO itemData, int amount)
        {
            this.itemData = itemData;
            requiredAmount = amount;
            description = $"Collect {amount} {itemData.itemName}";
        }

        protected override void OnRegisterListeners() => QuestEvents.OnItemCollected += OnItemCollected;
        protected override void OnUnregisterListeners() => QuestEvents.OnItemCollected -= OnItemCollected;

        private void OnItemCollected(string id, int amount)
        {
            if (id == itemData.itemId) AddProgress(amount);
        }

        public override string GetProgressText() => $"{itemData.itemName}: {currentAmount:0} / {requiredAmount:0}";

#if UNITY_EDITOR
        public override void Validate()
        {
            if (itemData == null)
            {
                base.Validate();
                return;
            }

            description = $"Collect {requiredAmount} {itemData.itemName}";
        }
#endif 
    }
}
