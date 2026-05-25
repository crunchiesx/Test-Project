// ============================================================
// Script: "KillObjective.cs"
// ============================================================
// Complete when player kills N enemies of a given type.
// Driven by: QuestEvents.EnemyKilled(enemyId)
// Bridge: EnemyDeathQuestBridge.cs
// ============================================================
using System;

namespace Crunchies.QuestSystem
{
    [Serializable]
    public class KillObjective : QuestObjective
    {
        public string enemyId = "wolf";
        public string enemyDisplayName = "Wolves";

        public KillObjective() { }

        public KillObjective(string enemyId, string displayName, int count)
        {
            this.enemyId = enemyId;
            enemyDisplayName = displayName;
            requiredAmount = count;
            description = $"Kill {count} {displayName}";
        }

        protected override void OnRegisterListeners() => QuestEvents.OnEnemyKilled += OnEnemyKilled;
        protected override void OnUnregisterListeners() => QuestEvents.OnEnemyKilled -= OnEnemyKilled;

        private void OnEnemyKilled(string id)
        {
            if (id == enemyId) AddProgress(1);
        }

        public override string GetProgressText() => $"{enemyDisplayName} Killed: {currentAmount:0} / {requiredAmount:0}";
    }
}
