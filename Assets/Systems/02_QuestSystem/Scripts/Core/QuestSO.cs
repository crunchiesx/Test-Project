// ============================================================
// Script: "Quest.cs"
// ============================================================
// ScriptableObject that holds quest metadata and objective templates.
// One asset per quest definition.
// Create via Assets > Create > Scriptable Objects > Quests > New Quest 
// ============================================================
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Crunchies.QuestSystem
{
    public enum QuestStatus
    {
        NotStarted,
        Active,
        Completed,
        Failed
    }

    [CreateAssetMenu(fileName = "QuestSO", menuName = "ScriptableObjects/Quests/New Quest")]
    public class QuestSO : ScriptableObject
    {
        [Header("Identity")]
        public string questId;
        public string questName = "Unnamed Quest";

        [TextArea]
        public string description = "";

        [Header("Rewards")]
        public int xpReward = 0;
        public int goldReward = 0;

        // public Item[] itemRewards; // swap in your inventory item type

        [Header("Objectives")]
        // Unity cannot serialize abstract types in the Inspector without SerializeReference.
        // These are objective templates; CreateInstance() clones them into runtime objectives.
        [SerializeReference] public List<QuestObjective> objectives = new();

        public QuestInstance CreateInstance()
        {
            List<QuestObjective> runtimeObjectives = objectives
                .Select(obj => obj.Clone())
                .ToList();

            return new QuestInstance(this, runtimeObjectives);
        }
    }
}
