using System;
using System.Collections.Generic;
using System.Linq;
using Crunchies.Interfaces;
using UnityEngine;

namespace Crunchies.Components
{
    public class Interactable : MonoBehaviour
    {
        public event Action OnInteract;

        [Header("Interaction Settings")]
        [SerializeField] private string defaultPrompt = "Press 'E' to Interact";

        private List<IInteractionOption> _interactionOptions = new();

        public void Awake()
        {
            _interactionOptions = GetComponents<MonoBehaviour>()
                .OfType<IInteractionOption>()
                .OrderByDescending(option => option.InteractionPriority)
                .ToList();
        }

        public void Interact()
        {
            IInteractionOption option = GetBestAvailableOption();
            if (option == null) return;

            option.ExecuteInteraction();
            OnInteract?.Invoke();
        }

        public bool IsInteractable()
        {
            return GetBestAvailableOption() != null;
        }

        public string GetCurrentInteractionPrompt()
        {
            IInteractionOption option = GetBestAvailableOption();
            if (option == null)
            {
                return defaultPrompt;
            }

            string prompt = option.GetInteractionPrompt();
            return string.IsNullOrWhiteSpace(prompt) ? defaultPrompt : prompt;
        }

        private IInteractionOption GetBestAvailableOption()
        {
            foreach (IInteractionOption option in _interactionOptions)
            {
                if (option.CanInteract())
                {
                    return option;
                }
            }

            return null;
        }
    }
}