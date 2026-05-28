// ============================================================
// Script: "InteractObjective.cs"
// ============================================================
// Complete when the player interacts with N objects by id.
// Driven by: QuestEvents.ObjectInteracted(objectId)
// Bridge: InteractQuestBridge.cs
// ============================================================
using System;
using UnityEngine;
using Crunchies.ScriptableObjects;

namespace Crunchies.QuestSystem
{
    [Serializable]
    public class InteractObjective : QuestObjective
    {
        [Header("Interact Objective")]
        [SerializeField] private ObjectDataSO objectData;

        /// <summary>
        /// If true, the same object only counts once even if used multiple times.
        /// Useful for "pull 3 different levers".
        /// </summary>
        public bool uniqueOnly = false;

        [NonSerialized] private int _uniqueCount;

        public InteractObjective() { }

        public InteractObjective(ObjectDataSO objectData, int count = 1, bool uniqueOnly = false)
        {
            this.objectData = objectData;
            this.uniqueOnly = uniqueOnly;
            requiredAmount = count;
            description = $"Interact with {objectData.objectName} ({count}x)";
        }

        protected override void OnRegisterListeners() => QuestEvents.OnObjectInteracted += OnObjectInteracted;
        protected override void OnUnregisterListeners() => QuestEvents.OnObjectInteracted -= OnObjectInteracted;

        private void OnObjectInteracted(string id)
        {
            if (id != objectData.objectId) return;
            if (uniqueOnly && _uniqueCount >= 1) return;
            _uniqueCount++;
            AddProgress(1);
        }

        protected override void OnReset() => _uniqueCount = 0;

        public override string GetProgressText() => $"{objectData.objectName}: {currentAmount:0} / {requiredAmount:0}";


#if UNITY_EDITOR
        public override void Validate()
        {
            if (objectData == null)
            {
                base.Validate();
                return;
            }

            description = $"Interact with {objectData.objectName} ({requiredAmount}x)";
        }
#endif
    }
}
