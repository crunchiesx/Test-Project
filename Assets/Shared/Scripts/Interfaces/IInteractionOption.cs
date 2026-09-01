using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Crunchies.Interfaces
{
    public enum PriorityOrder
    {
        None,
        First,
        Second,
        Third,
        Fourth,
        Fifth
    }

    public interface IInteractionOption
    {
        // Add [field: SerializeField] to allow setting priority in inspector for each implementation
        public abstract string InteractionPrompt { get; set; }
        public abstract PriorityOrder InteractionPriority { get; set; }

        public void ExecuteInteraction();
        public bool CanInteract();
        public string GetInteractionPrompt() => InteractionPrompt;
    }
}