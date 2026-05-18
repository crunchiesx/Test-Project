// ============================================================
// Script: "EscortObjective.cs"
// ============================================================
// Complete when a named NPC reaches its destination.
// Driven by: QuestEvents.NpcReachedDestination(npcId)
// Bridge: NpcArrivalQuestBridge.cs (on the NPC GameObject)
// ============================================================
using System;

namespace Crunchies.QuestSystem
{
    [Serializable]
    public class EscortObjective : QuestObjective
    {
        public string npcId = "merchant_01";
        public string npcDisplayName = "Merchant";

        public EscortObjective() { }

        public EscortObjective(string npcId, string displayName)
        {
            this.npcId = npcId;
            npcDisplayName = displayName;
            requiredAmount = 1;
            description = $"Escort {displayName} to safety";
        }

        public override void RegisterListeners() => QuestEvents.OnNpcReachedDestination += OnNpcArrived;

        public override void UnregisterListeners() => QuestEvents.OnNpcReachedDestination -= OnNpcArrived;

        private void OnNpcArrived(string id)
        {
            if (id == npcId) AddProgress(1);
        }

        public override string GetProgressText() => IsCompleted ? $"{npcDisplayName}: Safe!" : $"Escorting {npcDisplayName}...";
    }
}
