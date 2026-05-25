// ============================================================
// Script: "WalkDistanceObjective.cs"
// ============================================================
// Complete when the player walks N units of distance.
// Driven by: QuestEvents.DistanceTraveled(delta)
// Bridge: PlayerDistanceQuestBridge.cs
// ============================================================
using System;

namespace Crunchies.QuestSystem
{
    [Serializable]
    public class WalkDistanceObjective : QuestObjective
    {
        public string unitLabel = "m";

        public WalkDistanceObjective() { }

        public WalkDistanceObjective(float distance, string unitLabel = "m")
        {
            requiredAmount = distance;
            this.unitLabel = unitLabel;
            description = $"Travel {distance}{unitLabel}";
        }

        protected override void OnRegisterListeners() => QuestEvents.OnDistanceTraveled += OnDistanceTraveled;
        protected override void OnUnregisterListeners() => QuestEvents.OnDistanceTraveled -= OnDistanceTraveled;

        private void OnDistanceTraveled(float delta) => AddProgress(delta);

        public override string GetProgressText() => $"Distance: {currentAmount:F1}{unitLabel} / {requiredAmount:F1}{unitLabel}";
    }
}
