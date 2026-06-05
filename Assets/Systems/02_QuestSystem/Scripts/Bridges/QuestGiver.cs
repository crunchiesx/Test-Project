// ============================================================
// Script: "QuestGiver.cs"
// ============================================================
// Attach to an NPC alongside your dialogue or interaction component. 
// Hands a quest to QuestManager when triggered.
// Wire questAsset in the Inspector, or leave it empty to use the built-in factory presets for quick testing.
// ============================================================
// Example GameObject layout:
//   NPC_Villager
//   ├── DialogueController.cs  ← talks to player, triggers GiveQuest()
//   └── QuestGiver.cs          ← this file
// ============================================================
using System;
using Crunchies.Interfaces;
using Crunchies.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace Crunchies.QuestSystem
{
    public class QuestGiver : MonoBehaviour, IInteractable
    {
        public event Action OnInteract;

        [Tooltip("Drag a Quest ScriptableObject here. If empty, questType is used instead.")]
        [SerializeField] private QuestSO questAsset;

        [Header("Fallback - used when no asset is assigned")]
        [SerializeField] private QuestType questType = QuestType.Gather;

        [Header("Events")]
        [SerializeField] private UnityEvent OnQuestGiven = new();
        [SerializeField] private UnityEvent OnQuestEnded = new();

        public QuestSO QuestAsset => questAsset;

        private bool _isGiven;

        private void Awake()
        {
            if (questAsset == null)
            {
                questAsset = BuildFromType();

                if (questAsset == null)
                {
                    Log.Warning("[QuestGiver] No quest configured.", this);
                    return;
                }
            }
        }

        private void OnEnable()
        {
            QuestEvents.OnQuestStarted += QuestGiven;
            QuestEvents.OnQuestCompleted += QuestEnded;
            QuestEvents.OnQuestFailed += QuestEnded;
        }

        private void OnDisable()
        {
            QuestEvents.OnQuestStarted -= QuestGiven;
            QuestEvents.OnQuestCompleted -= QuestEnded;
            QuestEvents.OnQuestFailed -= QuestEnded;
        }

        private void QuestGiven(QuestInstance instance)
        {
            if (instance.QuestId == questAsset.QuestId)
            {
                Log.Info("[QuestGiver] Quest Given: " + questAsset.QuestName);
                OnQuestGiven?.Invoke();
            }
        }

        private void QuestEnded(QuestInstance instance)
        {
            if (instance.QuestId == questAsset.QuestId)
            {
                Log.Info("[QuestGiver] Quest Updated: " + questAsset.QuestName);
                OnQuestEnded?.Invoke();
            }
        }

        // Called by your dialogue system, or auto-triggered on player proximity.
        public void GiveQuest()
        {
            if (_isGiven) return;

            if (QuestManager.Instance.AcceptQuest(questAsset))
            {
                _isGiven = true;
            }
        }

        public void Interact()
        {
            if (!_isGiven)
            {
                QuestEvents.QuestOffered(this);
                return;
            }

            OnInteract?.Invoke();
        }

        public bool IsInteractable() => !_isGiven;

        public string GetInteractionPrompt() => "Press 'E' to Accept Quest";

        private QuestSO BuildFromType() => questType switch
        {
            QuestType.Gather => QuestFactory.CreateGatherQuest(Guid.NewGuid().ToString(), "wood", "Wood", 10),
            QuestType.Kill => QuestFactory.CreateKillQuest(Guid.NewGuid().ToString(), "wolf", "Wolves", 5),
            QuestType.Walk => QuestFactory.CreateWalkQuest(Guid.NewGuid().ToString(), 200f),
            QuestType.Escort => QuestFactory.CreateEscortQuest(Guid.NewGuid().ToString(), "npc_merchant", "Merchant", "outpost_south", "Southern Outpost"),
            QuestType.Survival => QuestFactory.CreateSurvivalQuest(Guid.NewGuid().ToString(), 60f),
            QuestType.Craft => QuestFactory.CreateCraftQuest(Guid.NewGuid().ToString(), "iron_sword", "Iron Sword", 1),
            QuestType.Compound => QuestFactory.CreateRangersErrand(),
            _ => null
        };
    }
}