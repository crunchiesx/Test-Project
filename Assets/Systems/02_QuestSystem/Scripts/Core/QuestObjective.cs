// ============================================================
// Script: "QuestObjective.cs"
// ============================================================
// Abstract base class for all objective types.
// Subclass this - one file per objective - to extend the system.
// No other core files need to change when adding new types.
// ============================================================
using System;
using UnityEngine;

namespace Crunchies.QuestSystem
{
    [Serializable]
    public abstract class QuestObjective
    {
        [Header("Display")]
        public string description = "Complete the Objective";

        [Header("Runtime State - do not edit in Inspector")]
        [SerializeField] protected float currentAmount;
        [SerializeField] protected float requiredAmount = 1f;
        [SerializeField] private bool isCompleted;
        [SerializeField] private bool isFailed;

        [NonSerialized] private bool listenersRegistered;
        public bool ListenersRegistered => listenersRegistered;

        public bool IsCompleted => isCompleted;
        public bool IsFailed => isFailed;
        public float Current => currentAmount;
        public float Required => requiredAmount;
        public float Progress => requiredAmount > 0 ? Mathf.Clamp01(currentAmount / requiredAmount) : 0f;

        /// <summary>
        /// Subscribe to QuestEvents here.
        /// </summary>
        public void RegisterListeners()
        {
            if (listenersRegistered) return;

            OnRegisterListeners();
            listenersRegistered = true;
        }

        /// <summary>
        /// Unsubscribe to QuestEvents here.
        /// </summary>
        public void UnregisterListeners()
        {
            if (!listenersRegistered) return;

            OnUnregisterListeners();
            listenersRegistered = false;
        }

        protected abstract void OnRegisterListeners();
        protected abstract void OnUnregisterListeners();

        /// <summary>
        /// Human-readable progress string down in the UI.
        /// </summary>
        public virtual string GetProgressText() => $"{currentAmount:0} / {requiredAmount:0}";

        // ------------------------------------------------------------------
        // Internal helpers
        // ------------------------------------------------------------------

        protected void AddProgress(float amount)
        {
            if (isCompleted || isFailed) return;
            currentAmount = Mathf.Min(currentAmount + amount, requiredAmount);
            QuestEvents.ObjectiveUpdated(this);
            if (currentAmount >= requiredAmount) Complete();
        }

        protected void Complete()
        {
            isCompleted = true;
            UnregisterListeners();
        }

        public void Fail()
        {
            isFailed = true;
            UnregisterListeners();
        }

        public void Reset()
        {
            UnregisterListeners();
            currentAmount = 0;
            isCompleted = false;
            isFailed = false;
            OnReset();
        }

        protected virtual void OnReset() { }
    }
}
