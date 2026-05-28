// ============================================================
// Script: "QuestFactory.cs"
// ============================================================
// Builds QuestSO definitions in code.
// Use for quick prototyping, tutorials, or procedural quest templates.
// For hand-authored content, prefer ScriptableObject assets assigned directly to QuestGiver in the Inspector.
// ============================================================
using System.Collections.Generic;
using UnityEngine;
using Crunchies.ScriptableObjects;


namespace Crunchies.QuestSystem
{
    public static class QuestFactory
    {
        public static QuestSO CreateGatherQuest(string id, string itemId, string itemName, int amount)
        {
            QuestSO quest = ScriptableObject.CreateInstance<QuestSO>();
            quest.questId = id;
            quest.questName = $"Gather {amount} {itemName}";
            quest.description = $"Collect {amount} {itemName} and bring them back.";

            ItemDataSO itemData = ScriptableObject.CreateInstance<ItemDataSO>();
            itemData.itemId = itemId;
            itemData.itemName = itemName;
            quest.objectives.Add(new GatherObjective(itemData, amount));
            return quest;
        }

        public static QuestSO CreateKillQuest(string id, string enemyId, string enemyName, int count)
        {
            QuestSO quest = ScriptableObject.CreateInstance<QuestSO>();
            quest.questId = id;
            quest.questName = $"Hunt {count} {enemyName}";
            quest.description = $"Kill {count} {enemyName} in the area.";
            quest.xpReward = count * 25;

            EnemyDataSO enemyData = ScriptableObject.CreateInstance<EnemyDataSO>();
            enemyData.characterId = enemyId;
            enemyData.characterName = enemyName;
            quest.objectives.Add(new KillObjective(enemyData, count));
            return quest;
        }

        public static QuestSO CreateWalkQuest(string id, float distance, string unit = "m")
        {
            QuestSO quest = ScriptableObject.CreateInstance<QuestSO>();
            quest.questId = id;
            quest.questName = $"Walk {distance}{unit}";
            quest.description = $"Travel {distance}{unit}.";
            quest.xpReward = (int)(distance * 0.5f);
            quest.objectives.Add(new WalkDistanceObjective(distance, unit));
            return quest;
        }

        public static QuestSO CreateEscortQuest(string id, string npcId, string npcName, string destId, string destName)
        {
            QuestSO quest = ScriptableObject.CreateInstance<QuestSO>();
            quest.questId = id;
            quest.questName = $"Escort {npcName}";
            quest.description = $"Bring {npcName} safely to {destName}";
            quest.xpReward = 200;

            NpcDataSO npcDataSO = ScriptableObject.CreateInstance<NpcDataSO>();
            npcDataSO.characterId = npcId;
            npcDataSO.characterName = npcName;
            quest.objectives.Add(new EscortObjective(npcDataSO));

            LocationDataSO locationDataSO = ScriptableObject.CreateInstance<LocationDataSO>();
            locationDataSO.locationId = destId;
            locationDataSO.locationName = destName;
            quest.objectives.Add(new ReachLocationObjective(locationDataSO));
            return quest;
        }

        public static QuestSO CreateSurvivalQuest(string id, float seconds)
        {
            QuestSO quest = ScriptableObject.CreateInstance<QuestSO>();
            quest.questId = id;
            quest.questName = $"Survive for {seconds:0}s";
            quest.description = $"Stay alive for {seconds:0} seconds.";
            quest.xpReward = 500;
            quest.objectives.Add(new SurviveObjective(seconds));
            return quest;
        }

        public static QuestSO CreateCraftQuest(string id, string itemId, string itemName, int count)
        {
            QuestSO quest = ScriptableObject.CreateInstance<QuestSO>();
            quest.questId = id;
            quest.questName = $"Craft {count} {itemName}";
            quest.description = $"Use crafting table to make {count} {itemName}";
            quest.xpReward = count * 30;

            ItemDataSO itemDataSO = ScriptableObject.CreateInstance<ItemDataSO>();
            itemDataSO.itemId = itemId;
            itemDataSO.itemName = itemName;
            quest.objectives.Add(new CraftObjective(itemDataSO, count));
            return quest;
        }

        public static QuestSO CreateReachLocationQuest(string id, string locationId, string locationName)
        {
            QuestSO quest = ScriptableObject.CreateInstance<QuestSO>();
            quest.questId = id;
            quest.questName = $"Reach {locationName}";
            quest.description = $"Travel to {locationName}.";
            quest.xpReward = 150;

            LocationDataSO locationDataSO = ScriptableObject.CreateInstance<LocationDataSO>();
            locationDataSO.locationId = locationId;
            locationDataSO.locationName = locationName;
            quest.objectives.Add(new ReachLocationObjective(locationDataSO));
            return quest;
        }

        /// <summary>
        /// Multi-objective: kill + gather + reach location.
        /// </summary>
        public static QuestSO CreateRangersErrand()
        {
            QuestSO quest = ScriptableObject.CreateInstance<QuestSO>();
            quest.questId = "compound_rangers_errand";
            quest.questName = "Ranger's Errand";
            quest.description = "Hunt wolves, gather wood, and report to the outpost";
            quest.xpReward = 500;
            quest.goldReward = 50;

            EnemyDataSO enemyDataSO = ScriptableObject.CreateInstance<EnemyDataSO>();
            enemyDataSO.characterId = "wolf";
            enemyDataSO.characterName = "Wolves";
            quest.objectives.Add(new KillObjective(enemyDataSO, 3));

            ItemDataSO itemDataSO = ScriptableObject.CreateInstance<ItemDataSO>();
            itemDataSO.itemId = "wood";
            itemDataSO.itemName = "Wood";
            quest.objectives.Add(new GatherObjective(itemDataSO, 5));

            LocationDataSO locationDataSO = ScriptableObject.CreateInstance<LocationDataSO>();
            locationDataSO.locationId = "outpost_north";
            locationDataSO.locationName = "Northern Outpost";
            quest.objectives.Add(new ReachLocationObjective(locationDataSO));
            return quest;
        }

#if UNITY_EDITOR
        public static QuestSO GetRandomQuest(int id)
        {
            return id switch
            {
                1 => CreateGatherQuest("gather_wood", "wood", "Wood", 5),
                2 => CreateKillQuest("kill_wolf", "wolf", "Wolves", 3),
                3 => CreateWalkQuest("walk_around", 100f),
                4 => CreateEscortQuest("escort_farmer", "farmer_john", "Farmer John", "village_gate", "Village Gate"),
                5 => CreateSurvivalQuest("survive_night", 60f),
                6 => CreateCraftQuest("craft_potion", "potion_health", "Health Potion", 5),
                7 => CreateReachLocationQuest("reach_village", "village_gate", "Village Gate"),
                8 => CreateRangersErrand(),
                _ => null
            };
        }

        public static List<QuestSO> GetAllQuest()
        {
            List<QuestSO> quests = new()
            {
                CreateGatherQuest("gather_wood", "wood", "Wood", 5),
                CreateKillQuest("kill_wolf", "wolf", "Wolves", 3),
                CreateWalkQuest("walk_around", 100f),
                CreateEscortQuest("escort_farmer", "farmer_john", "Farmer John", "farmhouse", "Farmhouse"),
                CreateSurvivalQuest("survive_night", 60f),
                CreateCraftQuest("craft_potion", "potion_health", "Health Potion", 5),
                CreateReachLocationQuest("reach_village", "village_gate", "Village Gate"),
                CreateRangersErrand(),
            };
            return quests;
        }
#endif
    }
}
