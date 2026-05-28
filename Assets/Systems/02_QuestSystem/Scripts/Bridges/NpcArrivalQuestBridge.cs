// ============================================================
// Script: "NpcArrivalQuestBridge.cs"
// ============================================================
// Attach alongside your NPC controller on the escorted NPC
// GameObject. Listens for the arrival event from your NavMesh
// or NPC movement script and forwards it to the quest system.
// ============================================================
// Example GameObject layout:
//   NPC_Merchant
//   ├── NpcController.cs        ← NavMesh, follow player, dialogue
//   └── NpcArrivalQuestBridge.cs  ← this file
// ============================================================
using Crunchies.AI;
using Crunchies.ScriptableObjects;
using Crunchies.Utility;
using UnityEngine;

namespace Crunchies.QuestSystem
{
    [RequireComponent(typeof(NpcController))]
    public class NpcArrivalQuestBridge : MonoBehaviour
    {
        [Tooltip("Must match the NpcDataSO set in EscortObjective.")]
        [SerializeField] private NpcDataSO npcData;
        private NpcController _controller;

        private void Awake()
        {
            if (!TryGetComponent(out _controller))
            {
                Log.MissingComponent<NpcController>(this);
            }
        }

        private void OnEnable() => _controller.OnDestinationReached += HandleArrival;
        private void OnDisable() => _controller.OnDestinationReached -= HandleArrival;

        private void HandleArrival()
        {
            QuestEvents.NpcReachDestination(npcData.characterId);
        }
    }
}
