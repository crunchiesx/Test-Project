using Crunchies.Components;
using Crunchies.InputActions;
using Crunchies.Interfaces;
using Crunchies.PlayerSystem;
using Crunchies.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.Assemblies;

namespace Crunchies.UI
{
    public class InteractUI : UIPanel
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI interactText;

        private bool interactionState = false;

        private void OnEnable()
        {
            OnPanelChange += PanelChange;
            PlayerInteraction.OnInteractionUpdate += InteractionUpdate;
        }
        private void OnDisable()
        {
            OnPanelChange -= PanelChange;
            PlayerInteraction.OnInteractionUpdate -= InteractionUpdate;
        }

        public void Awake()
        {
            if (IsOpen)
            {
                ClosePanel();
            }
        }

        private void PanelChange()
        {
            bool shouldBeOpen = interactionState && !IsAnyPanelActive;

            if (shouldBeOpen && !IsOpen)
            {
                OpenPanel();
            }
            else if (!shouldBeOpen && IsOpen)
            {
                ClosePanel();
            }
        }

        private void InteractionUpdate(bool hasInteraction, Interactable interactable)
        {
            interactionState = hasInteraction;

            if (hasInteraction)
            {
                string context = $"Press '{PlayerInputHandler.Instance.InteractDisplayString()}' to {interactable.GetCurrentInteractionPrompt()}";
                interactText.SetText(context);
                OpenPanel();
            }
            else
            {
                ClosePanel();
            }
        }
    }
}
