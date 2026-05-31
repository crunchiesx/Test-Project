// ============================================================
// Script: "EscortObjective.cs"
// ============================================================
// Complete when a named NPC reaches its destination.
// Driven by: QuestEvents.NpcReachedDestination(npcId)
// Bridge: NpcArrivalQuestBridge.cs (on the NPC GameObject)
// ============================================================
using System;
using Crunchies.ScriptableObjects;
using Crunchies.Utility;
using UnityEngine;

namespace Crunchies.QuestSystem
{
    [Serializable]
    public class EscortObjective : QuestObjective
    {
        [Header("Escort Objective")]
        [SerializeField] private NpcDataSO npcData;
        [SerializeField] private LocationDataSO locationData;

        public EscortObjective() { }

        public EscortObjective(NpcDataSO npcData, LocationDataSO locationData)
        {
            this.npcData = npcData;
            this.locationData = locationData;
            requiredAmount = 1;
            description = $"Escort {npcData.characterName} to {locationData.locationName}";
        }

        protected override void OnRegisterListeners() => QuestEvents.OnNpcReachedDestination += OnNpcArrived;
        protected override void OnUnregisterListeners() => QuestEvents.OnNpcReachedDestination -= OnNpcArrived;

        private void OnNpcArrived(CharacterDataSO charData, LocationDataSO locData)
        {
            if (charData.characterId != npcData.characterId) return;
            if (locData.locationId != locationData.locationId) return;

            AddProgress(1);
        }

        public override string GetProgressText() => IsCompleted ? $"{npcData.characterName}: Safe!" : $"Escorting {npcData.characterName}...";

#if UNITY_EDITOR
        public override void Validate()
        {
            if (npcData == null && locationData == null)
            {
                base.Validate();
                return;
            }

            description = $"Escort {(npcData == null ? "???" : npcData.characterName)} to {(locationData == null ? "???" : locationData.locationName)}";
        }
#endif
    }
}
