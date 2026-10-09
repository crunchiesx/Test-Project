using Crunchies.InputActions;
using Crunchies.UI;
using Crunchies.Utility;
using UnityEngine;
using Crunchies.QuestSystem;

namespace Crunchies.Managers
{
    public class UIManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private QuestListUI questUI;

        private void OnEnable()
        {
            if (PlayerInputHandler.Instance != null)
            {
                PlayerInputHandler.Instance.OnUIQuestAction += OnUIQuestAction;
                PlayerInputHandler.Instance.OnUIEscapeAction += OnUIEscapeAction;
            }
        }

        private void OnDisable()
        {
            if (PlayerInputHandler.Instance != null)
            {
                PlayerInputHandler.Instance.OnUIQuestAction -= OnUIQuestAction;
                PlayerInputHandler.Instance.OnUIEscapeAction -= OnUIEscapeAction;
            }
        }

        private void OnUIQuestAction()
        {
            if (questUI == null)
            {
                Log.MissingReference<QuestListUI>(this);
            }

            if (questUI.IsOpen)
            {
                questUI.ClosePanel();
            }
            else
            {
                questUI.OpenPanel();
            }
        }

        private void OnUIEscapeAction()
        {
            bool panelWasClosed = UIPanel.CloseRecentActivePanel();

            if (!panelWasClosed)
            {
                GameManager.Instance.TogglePause();
            }
        }
    }
}