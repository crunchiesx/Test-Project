// ============================================================
// Script: "GatherObjective.cs"
// ============================================================
// Complete when the player collect N of specific item.
// Driven by: QuestEvents.ItemCollected(itemId, amount)
// Bridge:    ItemPickupQuestBridge.cs
// ============================================================
using System;
using UnityEngine;

namespace Crunchies.QuestSystem
{
    [Serializable]
    public class GatherObjective : QuestObjective
    {
        [Header("Gather Objective")]
        public string itemId = "wood";
        public string itemDisplayName = "Wood";

        public GatherObjective() { }

        public GatherObjective(string itemId, string displayName, int amount)
        {
            this.itemId = itemId;
            itemDisplayName = displayName;
            requiredAmount = amount;
            description = $"Collect {amount} {displayName}";
        }

        protected override void OnRegisterListeners() => QuestEvents.OnItemCollected += OnItemCollected;
        protected override void OnUnregisterListeners() => QuestEvents.OnItemCollected -= OnItemCollected;

        private void OnItemCollected(string id, int amount)
        {
            if (id == itemId) AddProgress(amount);
        }

        public override string GetProgressText() => $"{itemDisplayName}: {currentAmount:0} / {requiredAmount:0}";
    }
}
