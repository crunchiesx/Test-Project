using System;
using Crunchies.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace Crunchies.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NpcController : MonoBehaviour
    {
        public event Action OnDestinationReached;

        private NavMeshAgent navAgent;

        private void Awake()
        {
            if (!TryGetComponent(out navAgent))
            {
                Log.MissingComponent<NavMeshAgent>(this);
            }
        }
    }
}
