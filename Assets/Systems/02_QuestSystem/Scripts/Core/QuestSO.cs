// ============================================================
// Script: "Quest.cs"
// ============================================================
// ScriptableObject that holds quest metadata and its list of objectives.
// One asset per quest.
// Create via Assets > Create > Scriptable Objects > Quests > New Quest 
// ============================================================
using System;
using System.Collections.Generic;
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
        // Unity Cannot serialize abstract types in the Inspector.
        // Populate this list in code via QuestFactory or QuestGiver.
        // For Inspector-driven quests, look into a ScriptableObject-per-objective 
        // wrapper pattern or Odin Inspector's polymorphic list.
        [SerializeReference] public List<QuestObjective> objectives = new();


        // ---- Runtime state (not persisted in SO asset) ----
        [NonSerialized] public QuestStatus status = QuestStatus.NotStarted;

        public bool IsActive => status == QuestStatus.Active;
        public bool IsCompleted => status == QuestStatus.Completed;
        public bool IsFailed => status == QuestStatus.Failed;

        // ------------------------------------------------------------------
        // Lifecycle
        // ------------------------------------------------------------------

        public void Begin()
        {
            if (status != QuestStatus.NotStarted) return;
            status = QuestStatus.Active;

            RegisterObjectiveListeners();

            QuestEvents.QuestStarted(this);
        }

        /// <summary>
        /// Called every frame by QuestManager. Checks whether all objectives are done or any have failed.
        /// </summary>
        public void Tick()
        {
            if (status != QuestStatus.Active) return;

            foreach (QuestObjective obj in objectives)
            {
                if (obj.IsFailed)
                {
                    Fail();
                    return;
                }
            }

            bool allDone = true;
            foreach (QuestObjective obj in objectives)
            {
                if (!obj.IsCompleted)
                {
                    allDone = false;
                    break;
                }
            }

            if (allDone) Complete();
        }

        public void Complete()
        {
            status = QuestStatus.Completed;

            UnregisterObjectiveListeners();

            QuestEvents.QuestCompleted(this);
        }

        public void Fail()
        {
            status = QuestStatus.Failed;

            foreach (QuestObjective obj in objectives)
            {
                obj.Fail();
            }

            QuestEvents.QuestFailed(this);
        }

        /// <summary>
        /// Clears all runtime state so this ScriptableObject can be re-accepted during the same play session.
        /// </summary>
        public void ResetRuntime()
        {
            UnregisterObjectiveListeners();
            status = QuestStatus.NotStarted;

            foreach (QuestObjective obj in objectives)
            {
                obj.Reset();
            }
        }

        public void RegisterObjectiveListeners()
        {
            foreach (QuestObjective obj in objectives)
            {
                obj.RegisterListeners();
            }
        }

        public void UnregisterObjectiveListeners()
        {
            foreach (QuestObjective obj in objectives)
            {
                obj.UnregisterListeners();
            }
        }
    }
}
