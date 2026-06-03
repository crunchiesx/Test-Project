// ============================================================
// Script: "QuestEvents.cs"
// ============================================================
// Static event bus. 
// Decouples all gameplay systems from the quest system.
// Any system fires an event; objectives listen.
// ============================================================
using System;
using Crunchies.ScriptableObjects;
using Crunchies.Utility;

namespace Crunchies.QuestSystem
{
    public enum QuestType
    {
        Gather,
        Kill,
        Walk,
        Escort,
        Survival,
        Craft,
        Compound
    }

    public static class QuestEvents
    {
        // ==== QUEST LIFECYCLE ====

        // Quest Started
        public static event Action<QuestInstance> OnQuestStarted;
        public static void QuestStarted(QuestInstance quest)
        {
            Log.Info("[EVENT] Quest Started: " + quest.QuestName);
            OnQuestStarted?.Invoke(quest);
        }

        // Quest Completed
        public static event Action<QuestInstance> OnQuestCompleted;
        public static void QuestCompleted(QuestInstance quest)
        {
            Log.Info("[EVENT] Quest Completed: " + quest.QuestName);
            OnQuestCompleted?.Invoke(quest);
        }

        // Quest Failed
        public static event Action<QuestInstance> OnQuestFailed;
        public static void QuestFailed(QuestInstance quest)
        {
            Log.Info("[EVENT] Quest Failed: " + quest.QuestName);
            OnQuestFailed?.Invoke(quest);
        }

        // Quest Objective Updated
        public static event Action<QuestObjective> OnObjectiveUpdated;
        public static void ObjectiveUpdated(QuestObjective objective)
        {
            Log.Info("[EVENT] Objective Updated: " + objective.description + " (" + objective.Progress * 100f + "%)");
            OnObjectiveUpdated?.Invoke(objective);
        }

        // Quest Offered
        public static event Action<QuestGiver> OnQuestOffered;
        public static void QuestOffered(QuestGiver giver)
        {
            Log.Info("[EVENT] Quest Offered: " + giver.QuestAsset.QuestName);
            OnQuestOffered?.Invoke(giver);
        }

        // ==== OBJECTIVE EVENTS ====

        // Item / Gathering
        public static event Action<string, int> OnItemCollected;
        public static void ItemCollected(string itemId, int amount = 1)
        {
            Log.Info("[EVENT] Item Collected: " + itemId);
            OnItemCollected?.Invoke(itemId, amount);
        }

        // Crafting
        public static event Action<string, int> OnItemCrafted;
        public static void ItemCrafted(string recipeID, int amount = 1)
        {
            Log.Info("[EVENT] Item Crafted: " + recipeID);
            OnItemCrafted?.Invoke(recipeID, amount);
        }

        // Combat
        public static event Action<string> OnEnemyKilled;
        public static void EnemyKilled(string enemyId)
        {
            Log.Info("[EVENT] Enemy Killed: " + enemyId);
            OnEnemyKilled?.Invoke(enemyId);
        }

        // Movement
        public static event Action<float> OnDistanceTraveled;
        public static void DistanceTraveled(float delta)
        {
            // Log.Info("[EVENT] Distance Traveled: " + delta);
            OnDistanceTraveled?.Invoke(delta);
        }

        // Interaction
        public static event Action<string> OnObjectInteracted;
        public static void ObjectInteracted(string objectId)
        {
            Log.Info("[EVENT] Object Interacted: " + objectId);
            OnObjectInteracted?.Invoke(objectId);
        }

        // Location
        public static event Action<string> OnLocationReached;
        public static void LocationReached(string locationId)
        {
            Log.Info("[EVENT] Location Reached: " + locationId);
            OnLocationReached?.Invoke(locationId);
        }

        // Escort
        public static event Action<CharacterDataSO, LocationDataSO> OnNpcReachedDestination;
        public static void NpcReachDestination(CharacterDataSO characterData, LocationDataSO locationData)
        {
            Log.Info("[EVENT] NPC Reached Destination: " + characterData.characterId + " @ " + locationData.locationId);
            OnNpcReachedDestination?.Invoke(characterData, locationData);
        }

        // Survival
        public static event Action<float> OnTimeSurvived;
        public static void TimeSurvived(float seconds)
        {
            Log.Info("[EVENT] Time Survived: " + seconds);
            OnTimeSurvived?.Invoke(seconds);
        }
    }
}
