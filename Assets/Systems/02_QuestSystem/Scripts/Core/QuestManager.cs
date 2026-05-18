// ============================================================
// Script: "QuestManager.cs"
// ============================================================
// Singleton class.
// Accepts quests, ticks active once every frame, and moves them to completed or failed list when done.
// ============================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Crunchies.Utility;
using UnityEngine;

namespace Crunchies.QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; private set; }

        private readonly List<Quest> _activeQuest = new();
        private readonly List<Quest> _completedQuest = new();
        private readonly List<Quest> _failedQuest = new();

        public IReadOnlyList<Quest> ActiveQuest => _activeQuest;
        public IReadOnlyList<Quest> CompletedQuest => _completedQuest;
        public IReadOnlyList<Quest> FailedQuest => _failedQuest;

        [Header("Debug")]
        [SerializeField] private bool showDebugInfo = true;
        [SerializeField] private List<DebugQuestInfo> debugActiveQuest = new();
        [SerializeField] private List<DebugQuestInfo> debugCompletedQuest = new();
        [SerializeField] private List<DebugQuestInfo> debugFailedQuest = new();

        // ------------------------------------------------------------------
        // Unity Lifecycle
        // ------------------------------------------------------------------

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            QuestEvents.OnQuestCompleted += MoveToCompleted;
            QuestEvents.OnQuestFailed += MoveToFailed;
        }

        private void OnDisable()
        {
            QuestEvents.OnQuestCompleted -= MoveToCompleted;
            QuestEvents.OnQuestFailed -= MoveToFailed;
        }

        private void Update()
        {
            for (int i = _activeQuest.Count - 1; i >= 0; i--)
            {
                _activeQuest[i].Tick();
            }

            SyncDebugInfo();
        }

        // ------------------------------------------------------------------
        // Public API
        // ------------------------------------------------------------------

        /// <summary>
        /// Hand a quest to the manager. Returns false if already active or completed.
        /// </summary>
        public bool AcceptQuest(Quest quest)
        {
            if (quest == null) return false;

            if (_activeQuest.Any(q => q.questId == quest.questId))
            {
                Log.Warning($"[QuestManager] Already active: {quest.questName}");
                return false;
            }

            if (_completedQuest.Any(q => q.questId == quest.questId))
            {
                Log.Warning($"[QuestManager] Already completed: {quest.questName}");
                return false;
            }

            quest.ResetRuntime();
            _activeQuest.Add(quest);
            quest.Begin();
            return true;
        }

        /// <summary>
        /// Force-fail an active quest by id (e.g time limit expired externally).
        /// </summary>
        public void FailQuest(string questId)
        {
            Quest quest = _activeQuest.FirstOrDefault(q => q.questId == questId);
            quest.Fail();
        }

        public bool IsQuestActive(string questId) => _activeQuest.Any(q => q.questId == questId);
        public bool IsQuestCompleted(string questId) => _completedQuest.Any(q => q.questId == questId);
        public Quest GetActiveQuest(string questId) => _activeQuest.FirstOrDefault(q => q.questId == questId);

        // ------------------------------------------------------------------
        // Event handlers
        // ------------------------------------------------------------------

        private void MoveToCompleted(Quest quest)
        {
            _activeQuest.Remove(quest);
            _completedQuest.Add(quest);
        }

        private void MoveToFailed(Quest quest)
        {
            _activeQuest.Remove(quest);
            _failedQuest.Add(quest);
        }

        private void SyncDebugInfo()
        {
            if (!showDebugInfo)
            {
                debugActiveQuest.Clear();
                debugCompletedQuest.Clear();
                debugFailedQuest.Clear();
                return;
            }

            debugActiveQuest.Clear();
            foreach (Quest quest in _activeQuest)
            {
                DebugQuestInfo info = new(quest.questName, quest.questId);
                foreach (QuestObjective obj in quest.objectives)
                {
                    info.objectiveDebugEntries.Add(new DebugObjectiveInfo
                    (
                        obj.GetType().Name,
                        obj.GetProgressText()
                    ));
                }

                debugActiveQuest.Add(info);
            }

            debugCompletedQuest.Clear();
            foreach (Quest quest in _completedQuest)
            {
                DebugQuestInfo info = new(quest.questName, quest.questId);
                foreach (QuestObjective obj in quest.objectives)
                {
                    info.objectiveDebugEntries.Add(new DebugObjectiveInfo
                    (
                        obj.GetType().Name,
                        obj.GetProgressText()
                    ));
                }

                debugCompletedQuest.Add(info);
            }

            debugFailedQuest.Clear();
            foreach (Quest quest in _failedQuest)
            {
                DebugQuestInfo info = new(quest.questName, quest.questId);
                foreach (QuestObjective obj in quest.objectives)
                {
                    info.objectiveDebugEntries.Add(new DebugObjectiveInfo
                    (
                        obj.GetType().Name,
                        obj.GetProgressText()
                    ));
                }

                debugFailedQuest.Add(info);
            }
        }
    }

    [Serializable]
    public class DebugQuestInfo
    {
        public string questName;
        public string questId;
        public List<DebugObjectiveInfo> objectiveDebugEntries = new();

        public DebugQuestInfo(string questName, string questId)
        {
            this.questName = questName;
            this.questId = questId;
        }
    }


    [Serializable]
    public class DebugObjectiveInfo
    {
        public string description;
        public string progress;

        public DebugObjectiveInfo(string description, string progress)
        {
            this.description = description;
            this.progress = progress;
        }
    }
}
