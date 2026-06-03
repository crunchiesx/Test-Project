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
    public class NpcArrivalTrigger : MonoBehaviour
    {
        [Tooltip("Must match the locationData set in EscortObjective.")]
        [SerializeField] private LocationDataSO locationData;

        private void OnTriggerEnter(Collider other)
        {
            Log.Info("[NpcArrivalTrigger] Triggered by " + other.gameObject.name);
            if (!other.transform.TryGetComponent(out AgentController controller)) return;
            HandleArrival(controller.AgentData);
        }

        private void HandleArrival(CharacterDataSO characterData)
        {
            QuestEvents.NpcReachDestination(characterData, locationData);
        }
    }
}
