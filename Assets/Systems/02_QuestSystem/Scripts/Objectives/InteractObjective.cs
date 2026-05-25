// ============================================================
// Script: "InteractObjective.cs"
// ============================================================
// Complete when the player interacts with N objects by id.
// Driven by: QuestEvents.ObjectInteracted(objectId)
// Bridge: InteractQuestBridge.cs
// ============================================================
using System;

namespace Crunchies.QuestSystem
{
    [Serializable]
    public class InteractObjective : QuestObjective
    {
        public string objectId = "lever_01";
        public string objectDisplayName = "Ancient Lever";

        /// <summary>
        /// If true, the same object only counts once even if used multiple times.
        /// Useful for "pull 3 different levers".
        /// </summary>
        public bool uniqueOnly = false;

        [NonSerialized] private int _uniqueCount;

        public InteractObjective() { }

        public InteractObjective(string objectId, string displayName, int count = 1, bool uniqueOnly = false)
        {
            this.objectId = objectId;
            objectDisplayName = displayName;
            this.uniqueOnly = uniqueOnly;
            requiredAmount = count;
            description = $"Interact with {displayName} ({count}x)";
        }

        protected override void OnRegisterListeners() => QuestEvents.OnObjectInteracted += OnObjectInteracted;
        protected override void OnUnregisterListeners() => QuestEvents.OnObjectInteracted -= OnObjectInteracted;

        private void OnObjectInteracted(string id)
        {
            if (id != objectId) return;
            if (uniqueOnly && _uniqueCount >= 1) return;
            _uniqueCount++;
            AddProgress(1);
        }

        protected override void OnReset() => _uniqueCount = 0;

        public override string GetProgressText() => $"{objectDisplayName}: {currentAmount:0} / {requiredAmount:0}";
    }
}
