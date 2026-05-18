using Crunchies.InputActions;
using Crunchies.Utility;
using UnityEngine;

namespace Crunchies.QuestSystem
{
    public class GameMenuController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private QuestUI questUI;

        private void OnEnable()
        {
            if (PlayerInputHandler.Instance != null)
            {
                PlayerInputHandler.Instance.OnQuestAction += OnQuestAction;
            }
        }

        private void OnDisable()
        {
            if (PlayerInputHandler.Instance != null)
            {
                PlayerInputHandler.Instance.OnQuestAction += OnQuestAction;
            }
        }

        private void OnQuestAction()
        {
            if (questUI == null)
            {
                Log.MissingReference<QuestUI>(this);
            }

            if (questUI.IsOpen)
            {
                questUI.Close();
            }
            else
            {
                questUI.Open();
            }
        }
    }
}