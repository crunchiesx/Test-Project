// ============================================================
// Script: "QuestUI.cs"
// ============================================================
// Quest tracker panel. Spawns and refreshes QuestEntryUI
// prefabs in response to quest lifecycle events.
// ============================================================
// Inspector wiring:
//   questLogParent   → ScrollRect content transform
//   questEntryPrefab → prefab with QuestEntryUI component
//   noQuestsLabel    → optional "No active quests" label GameObject
// ============================================================
using System.Collections.Generic;
using Crunchies.UI;
using UnityEngine;

namespace Crunchies.QuestSystem
{
    public class QuestListUI : UIPanel
    {
        [Header("References")]
        [SerializeField] private Transform questLogParent;
        [SerializeField] private GameObject questEntryPrefab;
        [SerializeField] private GameObject noQuestLabel;

        private readonly Dictionary<string, QuestEntryUI> _entries = new();

        private void Awake()
        {
            if (IsOpen)
            {
                ClosePanel();
            }
        }

        private void OnEnable()
        {
            QuestEvents.OnQuestStarted += OnQuestStarted;
            QuestEvents.OnQuestCompleted += OnQuestChanged;
            QuestEvents.OnQuestFailed += OnQuestChanged;
            QuestEvents.OnObjectiveUpdated += OnObjectiveUpdated;
        }

        private void OnDisable()
        {
            QuestEvents.OnQuestStarted -= OnQuestStarted;
            QuestEvents.OnQuestCompleted -= OnQuestChanged;
            QuestEvents.OnQuestFailed -= OnQuestChanged;
            QuestEvents.OnObjectiveUpdated -= OnObjectiveUpdated;
        }

        private void OnQuestStarted(QuestInstance quest)
        {
            if (_entries.ContainsKey(quest.QuestId)) return;

            GameObject go = Instantiate(questEntryPrefab, questLogParent);
            QuestEntryUI entry = go.GetComponent<QuestEntryUI>();
            entry.Populate(quest);
            _entries.Add(quest.QuestId, entry);

            RefreshNoQuestLabel();
        }

        private void OnQuestChanged(QuestInstance quest)
        {
            if (_entries.TryGetValue(quest.QuestId, out var entry))
            {
                entry.Refresh(quest);
            }

            RefreshNoQuestLabel();
        }

        private void OnObjectiveUpdated(QuestObjective objective)
        {
            foreach (QuestInstance quest in QuestManager.Instance.ActiveQuest)
            {
                if (!quest.Objectives.Contains(objective)) continue;

                if (_entries.TryGetValue(quest.QuestId, out var entry))
                {
                    entry.Refresh(quest);
                }

                break;
            }
        }

        private void RefreshNoQuestLabel()
        {
            if (noQuestLabel != null)
            {
                noQuestLabel.SetActive(QuestManager.Instance.TotalQuestCount == 0);
            }
        }

        public override void OpenPanel()
        {
            base.OpenPanel();
            RefreshNoQuestLabel();
        }
    }
}
