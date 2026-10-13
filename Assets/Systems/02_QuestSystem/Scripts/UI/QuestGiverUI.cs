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
        [SerializeField] private Button closeButton;

        private QuestGiver currentGiver;

        protected override void Awake()
        {
            base.Awake();

            acceptButton.onClick.AddListener(OnAcceptButtonPressed);
            declineButton.onClick.AddListener(OnDeclineButtonPressed);
            closeButton.onClick.AddListener(OnCloseButtonPressed);
        }

        private void OnEnable() => QuestEvents.OnQuestOffered += OnQuestOffered;
        private void OnDisable() => QuestEvents.OnQuestOffered -= OnQuestOffered;

        private void OnAcceptButtonPressed()
        {
            currentGiver.GiveQuest();
            ClosePanel();
        }
        private void OnDeclineButtonPressed() => ClosePanel();
        private void OnCloseButtonPressed() => ClosePanel();

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
