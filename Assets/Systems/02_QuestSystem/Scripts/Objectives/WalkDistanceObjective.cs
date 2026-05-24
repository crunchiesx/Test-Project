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

#pragma warning disable UDR0004
        public override void RegisterListeners() => QuestEvents.OnDistanceTraveled += OnDistanceTraveled;
#pragma warning restore UDR0004
        public override void UnregisterListeners() => QuestEvents.OnDistanceTraveled -= OnDistanceTraveled;

        private void OnDistanceTraveled(float delta) => AddProgress(delta);

        public override string GetProgressText() => $"Distance: {currentAmount:F1}{unitLabel} / {requiredAmount:F1}{unitLabel}";
    }
}
