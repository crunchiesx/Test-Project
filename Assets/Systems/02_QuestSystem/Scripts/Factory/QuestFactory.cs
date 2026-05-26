// ============================================================
// Script: "QuestFactory.cs"
// ============================================================
// Builds QuestSO definitions in code.
// Use for quick prototyping, tutorials, or procedural quest templates.
// For hand-authored content, prefer ScriptableObject assets assigned directly to QuestGiver in the Inspector.
// ============================================================
using System.Collections.Generic;
using UnityEngine;

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
            quest.objectives.Add(new GatherObjective(itemId, itemName, amount));
            return quest;
        }

        public static QuestSO CreateKillQuest(string id, string enemyId, string enemyName, int count)
        {
            QuestSO quest = ScriptableObject.CreateInstance<QuestSO>();
            quest.questId = id;
            quest.questName = $"Hunt {count} {enemyName}";
            quest.description = $"Kill {count} {enemyName} in the area.";
            quest.xpReward = count * 25;
            quest.objectives.Add(new KillObjective(enemyId, enemyName, count));
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
            quest.objectives.Add(new EscortObjective(npcId, npcName));
            quest.objectives.Add(new ReachLocationObjective(destId, destName));
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

        public static QuestSO CreateCraftQuest(string id, string recipeId, string itemName, int count)
        {
            QuestSO quest = ScriptableObject.CreateInstance<QuestSO>();
            quest.questId = id;
            quest.questName = $"Craft {count} {itemName}";
            quest.description = $"Use crafting table to make {count} {itemName}";
            quest.xpReward = count * 30;
            quest.objectives.Add(new CraftObjective(recipeId, itemName, count));
            return quest;
        }

        public static QuestSO CreateReachLocationQuest(string id, string locationId, string locationName)
        {
            QuestSO quest = ScriptableObject.CreateInstance<QuestSO>();
            quest.questId = id;
            quest.questName = $"Reach {locationName}";
            quest.description = $"Travel to {locationName}.";
            quest.xpReward = 150;
            quest.objectives.Add(new ReachLocationObjective(locationId, locationName));
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
            quest.objectives.Add(new KillObjective("wolf", "Wolves", 3));
            quest.objectives.Add(new GatherObjective("wood", "Wood", 5));
            quest.objectives.Add(new ReachLocationObjective("outpost_north", "Northern Outpost"));
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
                4 => CreateEscortQuest("escort_farmer", "farmer_john", "Farmer John", "farmhouse", "Farmhouse"),
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
