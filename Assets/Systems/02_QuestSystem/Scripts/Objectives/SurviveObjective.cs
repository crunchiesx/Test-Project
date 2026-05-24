// ============================================================
// Script: SurviveObjective.cs
// ============================================================
// Complete when the player survives for N seconds.
// Driven by: QuestEvents.TimeSurvived(deltaTime)
// Bridge: SurvivalTimeQuestBridge.cs
// ============================================================
using System;

namespace Crunchies.QuestSystem
{
    [Serializable]
    public class SurviveObjective : QuestObjective
    {
        public SurviveObjective() { }

        public SurviveObjective(float seconds)
        {
            requiredAmount = seconds;
            description = $"Survive for {seconds:0}s";
        }

#pragma warning disable UDR0004
        public override void RegisterListeners() => QuestEvents.OnTimeSurvived += OnTimeSurvived;
#pragma warning restore UDR0004
        public override void UnregisterListeners() => QuestEvents.OnTimeSurvived -= OnTimeSurvived;

        private void OnTimeSurvived(float delta) => AddProgress(delta);

        public override string GetProgressText()
        {
            if (IsCompleted) return "Survived!";
            float remaining = requiredAmount - currentAmount;
            return $"Survive: {remaining:0}s remaining";
        }
    }
}
