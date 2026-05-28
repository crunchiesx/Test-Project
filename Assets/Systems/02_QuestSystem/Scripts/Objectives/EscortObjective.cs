// ============================================================
// Script: "EscortObjective.cs"
// ============================================================
// Complete when a named NPC reaches its destination.
// Driven by: QuestEvents.NpcReachedDestination(npcId)
// Bridge: NpcArrivalQuestBridge.cs (on the NPC GameObject)
// ============================================================
using System;
using Crunchies.ScriptableObjects;
using UnityEngine;

namespace Crunchies.QuestSystem
{
    [Serializable]
    public class EscortObjective : QuestObjective
    {
        [Header("Escort Objective")]
        [SerializeField] private NpcDataSO npcData;

        public EscortObjective() { }

        public EscortObjective(NpcDataSO npcData)
        {
            this.npcData = npcData;
            requiredAmount = 1;
            description = $"Escort {npcData.characterName} to safety";
        }

        protected override void OnRegisterListeners() => QuestEvents.OnNpcReachedDestination += OnNpcArrived;
        protected override void OnUnregisterListeners() => QuestEvents.OnNpcReachedDestination -= OnNpcArrived;

        private void OnNpcArrived(string id)
        {
            if (id == npcData.characterId) AddProgress(1);
        }

        public override string GetProgressText() => IsCompleted ? $"{npcData.characterName}: Safe!" : $"Escorting {npcData.characterName}...";

#if UNITY_EDITOR
        public override void Validate()
        {
            if (npcData == null)
            {
                base.Validate();
                return;
            }

            description = $"Escort {npcData.characterName} to safety";
        }
#endif
    }
}
