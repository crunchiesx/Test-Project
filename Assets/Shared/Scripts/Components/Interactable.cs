using System;
using Crunchies.Interfaces;
using UnityEngine;

namespace Crunchies.Components
{
    public class Interactable : MonoBehaviour, IInteractable
    {
        public event Action OnInteract;

        public void Interact()
        {
            throw new NotImplementedException();
        }

        public bool IsInteractable()
        {
            throw new NotImplementedException();
        }
    }
}