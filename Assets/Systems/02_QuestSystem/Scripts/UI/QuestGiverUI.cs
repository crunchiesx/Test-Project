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
                ClosePanel();
            }

            acceptButton.onClick.AddListener(() =>
            {
                currentGiver.GiveQuest();
                ClosePanel();
            });
            declineButton.onClick.AddListener(() => ClosePanel());
        }

        private void OnEnable() => QuestEvents.OnQuestOffered += OnQuestOffered;
        private void OnDisable() => QuestEvents.OnQuestOffered -= OnQuestOffered;

        private void OnQuestOffered(QuestGiver giver)
        {
            currentGiver = giver;
            SetUpUI(currentGiver.QuestAsset);
            OpenPanel();
        }

        private void SetUpUI(QuestSO quest)
        {
            titleText.text = quest.QuestName;
            descriptionText.text = quest.Description;
        }
    }
}
