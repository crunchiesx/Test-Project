using System;
using Crunchies.UI;
using Crunchies.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Crunchies.QuestSystem
{
    public class QuestGiverUI : UIPanel
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Button acceptButton;
        [SerializeField] private Button declineButton;

        private QuestGiver currentGiver;

        private void Awake()
        {
            if (IsOpen)
            {
                Close();
            }

            acceptButton.onClick.AddListener(() =>
            {
                currentGiver.GiveQuest();
                Close();
            });
            declineButton.onClick.AddListener(() => Close());
        }

        private void OnEnable() => QuestGiver.OnQuestOffered += OnQuestOffered;
        private void OnDisable() => QuestGiver.OnQuestOffered -= OnQuestOffered;

        private void OnQuestOffered(QuestGiver giver)
        {
            currentGiver = giver;
            SetUpUI(currentGiver.QuestAsset);
            Open();
        }

        private void SetUpUI(QuestSO quest)
        {
            titleText.text = quest.questName;
            descriptionText.text = quest.description;
        }
    }
}
