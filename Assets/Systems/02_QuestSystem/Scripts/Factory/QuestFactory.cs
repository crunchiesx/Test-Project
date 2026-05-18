// ============================================================
// Script: "QuestFactory.cs"
// ============================================================
// Builds ready-to-use Quest instances in code.
// Use for quick prototyping, tutorials, or procedural quests.
// For hand-authored content, prefer ScriptableObject assets assigned directly to QuestGiver in the Inspector.
// ============================================================
using UnityEngine;

namespace Crunchies.QuestSystem
{
    public static class QuestFactory
    {
        public static Quest CreateGatherQuest(string id, string itemId, string itemName, int amount)
        {
            Quest quest = ScriptableObject.CreateInstance<Quest>();
            quest.questId = id;
            quest.questName = $"Gather {amount} {itemName}";
            quest.description = $"Collect {amount} {itemName} and bring them back.";
            quest.objectives.Add(new GatherObjective(itemId, itemName, amount));
            return quest;
        }

        public static Quest CreateKillQuest(string id, string enemyId, string enemyName, int count)
        {
            Quest quest = ScriptableObject.CreateInstance<Quest>();
            quest.questId = id;
            quest.questName = $"Hunt {count} {enemyName}";
            quest.description = $"Kill {count} {enemyName} in the area.";
            quest.xpReward = count * 25;
            quest.objectives.Add(new KillObjective(enemyId, enemyName, count));
            return quest;
        }

        public static Quest CreateWalkQuest(string id, float distance, string unit = "m")
        {
            Quest quest = ScriptableObject.CreateInstance<Quest>();
            quest.questId = id;
            quest.questName = $"Walk {distance}{unit}";
            quest.description = $"Travel {distance}{unit}.";
            quest.xpReward = (int)(distance * 0.5f);
            quest.objectives.Add(new WalkDistanceObjective(distance, unit));
            return quest;
        }

        public static Quest CreateEscortQuest(string id, string npcId, string npcName, string destId, string destName)
        {
            Quest quest = ScriptableObject.CreateInstance<Quest>();
            quest.questId = id;
            quest.questName = $"Escort {npcName}";
            quest.description = $"Bring {npcName} safely to {destName}";
            quest.xpReward = 200;
            quest.objectives.Add(new EscortObjective(npcId, npcName));
            quest.objectives.Add(new ReachLocationObjective(destId, destName));
            return quest;
        }

        public static Quest CreateSurvivalQuest(string id, float seconds)
        {
            Quest quest = ScriptableObject.CreateInstance<Quest>();
            quest.questId = id;
            quest.questName = $"Survive for {seconds:0}s";
            quest.description = $"Stay alive for {seconds:0} seconds.";
            quest.xpReward = 500;
            quest.objectives.Add(new SurviveObjective(seconds));
            return quest;
        }

        public static Quest CreateCraftQuest(string id, string recipeId, string itemName, int count)
        {
            Quest quest = ScriptableObject.CreateInstance<Quest>();
            quest.questId = id;
            quest.questName = $"Craft {count} {itemName}";
            quest.description = $"Use crafting table to make {count} {itemName}";
            quest.xpReward = count * 30;
            quest.objectives.Add(new CraftObjective(recipeId, itemName, count));
            return quest;
        }

        /// <summary>
        /// Multi-objective: kill + gather + reach location.
        /// </summary>
        public static Quest CreateRangersErrand()
        {
            Quest quest = ScriptableObject.CreateInstance<Quest>();
            quest.questId = "compound_rangers_errand";
            quest.questName = "Ranger's Errand";
            quest.description = "Hunt wolves, gather herbs, and report to the outpost";
            quest.xpReward = 500;
            quest.goldReward = 50;
            quest.objectives.Add(new KillObjective("wolf", "Wolves", 5));
            quest.objectives.Add(new GatherObjective("herb_red", "Red Herbs", 3));
            quest.objectives.Add(new ReachLocationObjective("outpost_north", "Northern Outpost"));
            return quest;
        }
    }
}
