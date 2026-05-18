// ============================================================
// Script: "ReachLocationObjective.cs"
// ============================================================
// Complete when the player reaches a named area trigger.
// Driven by: QuestEvents.LocationReached(locationId)
// Bridge: LocationReachedQuestBridge.cs
// ============================================================
using System;

namespace Crunchies.QuestSystem
{
    [Serializable]
    public class ReachLocationObjective : QuestObjective
    {
        public string locationId = "village_gate";
        public string locationDisplayName = "Village Gate";

        public ReachLocationObjective() { }

        public ReachLocationObjective(string locationId, string displayName)
        {
            this.locationId = locationId;
            locationDisplayName = displayName;
            requiredAmount = 1;
            description = $"Reach: {displayName}";
        }

        public override void RegisterListeners() => QuestEvents.OnLocationReached += OnLocationReached;
        public override void UnregisterListeners() => QuestEvents.OnLocationReached -= OnLocationReached;

        private void OnLocationReached(string id)
        {
            if (id == locationId) AddProgress(1);
        }

        public override string GetProgressText() => IsCompleted ? $"{locationDisplayName}: Reached!" : $"{locationDisplayName}: Not Yet";
    }
}
