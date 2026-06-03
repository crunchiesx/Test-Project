// ============================================================
// Script: "Quest.cs"
// ============================================================
// ScriptableObject that holds quest metadata and objective templates.
// One asset per quest definition.
// Create via Assets > Create > Scriptable Objects > Quests > New Quest 
// ============================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Crunchies.ScriptableObjects;
using UnityEngine;

namespace Crunchies.QuestSystem
{
    [Serializable]
    public struct ItemReward
    {
        [SerializeField] private ItemDataSO item;

        [Min(1)]
        [SerializeField] private int quantity;

        public readonly ItemDataSO Item => item;

        public int Quantity
        {
            readonly get => quantity;
            set => quantity = Mathf.Max(1, value);
        }

        public ItemReward(ItemDataSO item, int quantity)
        {
            this.item = item;
            this.quantity = Mathf.Max(1, quantity);
        }
    }

    [CreateAssetMenu(fileName = "QuestSO", menuName = "Scriptable Objects/Quests/New Quest")]
    public class QuestSO : ScriptableObject
    {
        [Header("Identity")]
        public string QuestId;
        public string QuestName = "Unnamed Quest";

        [TextArea]
        public string Description = "";

        [Header("Rewards")]
        public int ExpReward = 0;
        public int GoldReward = 0;

        [SerializeField] private List<ItemReward> itemRewards = new();

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

#if UNITY_EDITOR
        private void OnValidate()
        {
            foreach (QuestObjective obj in objectives)
            {
                obj.Validate();
            }
        }
#endif
    }
}