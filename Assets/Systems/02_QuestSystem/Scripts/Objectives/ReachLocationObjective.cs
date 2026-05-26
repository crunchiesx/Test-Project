// ============================================================
// Script: "ReachLocationObjective.cs"
// ============================================================
// Complete when the player reaches a named area trigger.
// Driven by: QuestEvents.LocationReached(locationId)
// Bridge: LocationReachedQuestBridge.cs
// ============================================================
using System;
using UnityEngine;

namespace Crunchies.QuestSystem
{
    [Serializable]
    public class ReachLocationObjective : QuestObjective
    {
        [Header("Reach Objective")]
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

        protected override void OnRegisterListeners() => QuestEvents.OnLocationReached += OnLocationReached;
        protected override void OnUnregisterListeners() => QuestEvents.OnLocationReached -= OnLocationReached;

        private void OnLocationReached(string id)
        {
            if (id == locationId) AddProgress(1);
        }

        public override string GetProgressText() => IsCompleted ? $"{locationDisplayName}: Reached!" : $"{locationDisplayName}: Not Yet";
    }
}
