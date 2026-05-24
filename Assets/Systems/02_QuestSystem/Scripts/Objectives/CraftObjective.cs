// ============================================================
// CraftObjective.cs
// ============================================================
// Complete when the player crafts N of a specific recipe.
// Driven by: QuestEvents.ItemCrafted(recipeId, amount)
// Bridge: none needed - fire directly from your crafting system: QuestEvents.ItemCrafted("iron_sword", 1);
// ============================================================
using System;

namespace Crunchies.QuestSystem
{
    [Serializable]
    public class CraftObjective : QuestObjective
    {
        public string recipeId = "iron_sword";
        public string itemDisplayName = "Iron Sword";

        public CraftObjective() { }

        public CraftObjective(string recipeId, string displayName, int count)
        {
            this.recipeId = recipeId;
            itemDisplayName = displayName;
            requiredAmount = count;
            description = $"Craft {count} {displayName}";
        }

        public override void RegisterListeners() => QuestEvents.OnItemCrafted += OnItemCrafted;
        public override void UnregisterListeners() => QuestEvents.OnItemCrafted -= OnItemCrafted;

        private void OnItemCrafted(string id, int amount)
        {
            if (id == recipeId) AddProgress(amount);
        }

        public override string GetProgressText() => $"{itemDisplayName} crafted: {currentAmount:0} / {requiredAmount:0}";
    }
}
