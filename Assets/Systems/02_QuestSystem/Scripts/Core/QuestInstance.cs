using System.Collections.Generic;

namespace Crunchies.QuestSystem
{
    public enum QuestStatus
    {
        NotStarted,
        Active,
        Completed,
        Failed
    }

    public class QuestInstance
    {
        public QuestSO Definition { get; private set; }
        public List<QuestObjective> Objectives { get; private set; }
        public QuestStatus Status { get; private set; } = QuestStatus.NotStarted;

        public string QuestId => Definition.QuestId;
        public string QuestName => Definition.QuestName;
        public string Description => Definition.Description;
        public int XpReward => Definition.ExpReward;
        public int GoldReward => Definition.GoldReward;

        public bool IsActive => Status == QuestStatus.Active;
        public bool IsCompleted => Status == QuestStatus.Completed;
        public bool IsFailed => Status == QuestStatus.Failed;

        public QuestInstance(QuestSO definition, List<QuestObjective> objectives)
        {
            Definition = definition;
            Objectives = objectives;
        }

        public void Begin()
        {
            if (Status != QuestStatus.NotStarted) return;

            Status = QuestStatus.Active;
            RegisterObjectiveListeners();

            QuestEvents.QuestStarted(this);
        }

        public void Tick()
        {
            if (Status != QuestStatus.Active) return;

            foreach (QuestObjective obj in Objectives)
            {
                if (obj.IsFailed)
                {
                    Fail();
                    return;
                }
            }

            bool allDone = true;
            foreach (QuestObjective obj in Objectives)
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
            Status = QuestStatus.Completed;

            UnregisterObjectiveListeners();

            QuestEvents.QuestCompleted(this);
        }

        public void Fail()
        {
            Status = QuestStatus.Failed;

            foreach (QuestObjective obj in Objectives)
            {
                obj.Fail();
            }

            QuestEvents.QuestFailed(this);
        }

        public void ResetRuntime()
        {
            UnregisterObjectiveListeners();
            Status = QuestStatus.NotStarted;

            foreach (QuestObjective obj in Objectives)
            {
                obj.Reset();
            }
        }

        public void RegisterObjectiveListeners()
        {
            foreach (QuestObjective obj in Objectives)
            {
                obj.RegisterListeners();
            }
        }

        public void UnregisterObjectiveListeners()
        {
            foreach (QuestObjective obj in Objectives)
            {
                obj.UnregisterListeners();
            }
        }
    }
}
