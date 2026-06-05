using System;
using UnityEngine;

namespace Crunchies.Interfaces
{
    public interface IInteractable
    {
        public event Action OnInteract;

        public void Interact();
        public bool IsInteractable();
        public string GetInteractionPrompt() => "Press 'E' to interact.";
    }
}
