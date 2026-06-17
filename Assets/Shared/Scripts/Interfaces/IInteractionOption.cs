using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Crunchies.Interfaces
{
    public interface IInteractionOption
    {
        // Add [field: SerializeField] to allow setting priority in inspector for each implementation
        public abstract int InteractionPriority { get; set; }

        public void ExecuteInteraction();
        public bool CanInteract();
        public string GetInteractionPrompt();
    }
}