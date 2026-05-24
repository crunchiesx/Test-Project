// ============================================================
// Script: "GatherObjective.cs"
// ============================================================
// Complete when the player collect N of specific item.
// Driven by: QuestEvents.ItemCollected(itemId, amount)
// Bridge:    ItemPickupQuestBridge.cs
// ============================================================
using System;

namespace Crunchies.QuestSystem
{
    [Serializable]
    public class GatherObjective : QuestObjective
    {
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

#pragma warning disable UDR0004
        public override void RegisterListeners() => QuestEvents.OnItemCollected += OnItemCollected;
#pragma warning restore UDR0004
        public override void UnregisterListeners() => QuestEvents.OnItemCollected -= OnItemCollected;

        private void OnItemCollected(string id, int amount)
        {
            if (id == itemId) AddProgress(amount);
        }

        public override string GetProgressText() => $"{itemDisplayName}: {currentAmount:0} / {requiredAmount:0}";
    }
}