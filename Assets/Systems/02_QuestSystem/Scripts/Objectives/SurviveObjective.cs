// ============================================================
// Script: SurviveObjective.cs
// ============================================================
// Complete when the player survives for N seconds.
// Driven by: QuestEvents.TimeSurvived(deltaTime)
// Bridge: SurvivalTimeQuestBridge.cs
// ============================================================
using System;
using UnityEngine;

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

        protected override void OnRegisterListeners() => QuestEvents.OnTimeSurvived += OnTimeSurvived;
        protected override void OnUnregisterListeners() => QuestEvents.OnTimeSurvived -= OnTimeSurvived;

        private void OnTimeSurvived(float delta) => AddProgress(delta);

        public override string GetProgressText()
        {
            if (IsCompleted) return "Survived!";
            float remaining = requiredAmount - currentAmount;
            return $"Survive: {remaining:0}s remaining";
        }

#if UNITY_EDITOR
        public override void Validate()
        {
            description = $"Survive for {requiredAmount:0}s";
        }
#endif
    }
}
