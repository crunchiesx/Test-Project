// ============================================================
// Script: "QuestEntryUI.cs"
// ============================================================
// Attach to the quest entry prefab. Displays one quest's name,
// status, and a live progress line per objective.
// ============================================================
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Crunchies.QuestSystem
{
    public class QuestEntryUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Transform objectivesParent;

        [Tooltip("Prefab with a single TextMeshProUGUI - one per objective line.")]
        [SerializeField] private GameObject objectiveLinePrefab;

        private readonly List<TextMeshProUGUI> _lines = new();

        public void Populate(Quest quest)
        {
            titleText.SetText(quest.questName);
            BuildLines(quest);
            Refresh(quest);
        }

        public void Refresh(Quest quest)
        {
            statusText.SetText
            (
                quest.status switch
                {
                    QuestStatus.Active => "In Progress",
                    QuestStatus.Completed => "^ Completed",
                    QuestStatus.Failed => "! Failed",
                    _ => string.Empty
                }
            );

            for (int i = 0; i < quest.objectives.Count && i < _lines.Count; i++)
            {
                QuestObjective obj = quest.objectives[i];
                TextMeshProUGUI line = _lines[i];

                string prefix = obj.IsCompleted ? "^ " : obj.IsFailed ? "! " : "* ";
                line.text = prefix + obj.GetProgressText();
                line.color = obj.IsCompleted ? Color.green : obj.IsFailed ? Color.red : Color.white;
            }
        }

        private void BuildLines(Quest quest)
        {
            foreach (Transform child in objectivesParent)
            {
                Destroy(child.gameObject);
            }
            _lines.Clear();

            foreach (QuestObjective _ in quest.objectives)
            {
                GameObject go = Instantiate(objectiveLinePrefab, objectivesParent);
                TextMeshProUGUI tmp = go.GetComponentInChildren<TextMeshProUGUI>();
                _lines.Add(tmp);
            }
        }
    }
}
