using System;
using UnityEngine;

namespace Crunchies.Interfaces
{
    public interface IInteractable
    {
        public event Action OnInteract;

        public void Interact();
    }
}
