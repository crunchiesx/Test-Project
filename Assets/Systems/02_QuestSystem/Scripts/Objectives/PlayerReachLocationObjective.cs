// ============================================================
// Script: "ReachLocationObjective.cs"
// ============================================================
// Complete when the player reaches a named area trigger.
// Driven by: QuestEvents.LocationReached(locationId)
// Bridge: LocationReachedQuestBridge.cs
// ============================================================
using System;
using Crunchies.ScriptableObjects;
using UnityEngine;

namespace Crunchies.QuestSystem
{
    [Serializable]
    public class PlayerReachLocationObjective : QuestObjective
    {
        [Header("Reach Objective")]
        [SerializeField] private LocationDataSO locationData;

        public PlayerReachLocationObjective() { }

        public PlayerReachLocationObjective(LocationDataSO locationData)
        {
            this.locationData = locationData;
            requiredAmount = 1;
            description = $"Reach: {locationData.locationName}";
        }

        protected override void OnRegisterListeners() => QuestEvents.OnLocationReached += OnLocationReached;
        protected override void OnUnregisterListeners() => QuestEvents.OnLocationReached -= OnLocationReached;

        private void OnLocationReached(string id)
        {
            if (id == locationData.locationId) AddProgress(1);
        }

        public override string GetProgressText() => IsCompleted ? $"{locationData.locationName}: Reached!" : $"{locationData.locationName}: Not Yet";

#if UNITY_EDITOR
        public override void Validate()
        {
            if (locationData == null)
            {
                base.Validate();
                return;
            }

            description = $"Reach: {locationData.locationName}";
        }
#endif
    }
}
