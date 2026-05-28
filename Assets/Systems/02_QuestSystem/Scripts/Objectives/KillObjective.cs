// ============================================================
// Script: "KillObjective.cs"
// ============================================================
// Complete when player kills N enemies of a given type.
// Driven by: QuestEvents.EnemyKilled(enemyId)
// Bridge: EnemyDeathQuestBridge.cs
// ============================================================
using System;
using Crunchies.ScriptableObjects;
using UnityEngine;

namespace Crunchies.QuestSystem
{
    [Serializable]
    public class KillObjective : QuestObjective
    {
        [Header("Kill Objective")]
        [SerializeField] private EnemyDataSO enemyData;

        public KillObjective() { }

        public KillObjective(EnemyDataSO enemyData, int count)
        {
            this.enemyData = enemyData;
            requiredAmount = count;
            description = $"Kill {count} {enemyData.characterName}";
        }

        protected override void OnRegisterListeners() => QuestEvents.OnEnemyKilled += OnEnemyKilled;
        protected override void OnUnregisterListeners() => QuestEvents.OnEnemyKilled -= OnEnemyKilled;

        private void OnEnemyKilled(string id)
        {
            if (id == enemyData.characterId) AddProgress(1);
        }

        public override string GetProgressText() => $"{enemyData.characterName} Killed: {currentAmount:0} / {requiredAmount:0}";

#if UNITY_EDITOR
        public override void Validate()
        {
            if (enemyData == null)
            {
                base.Validate();
                return;
            }

            description = $"Kill {requiredAmount} {enemyData.characterName}";
        }
#endif
    }
}
