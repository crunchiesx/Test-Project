// ============================================================
// Script: "LocationReachedQuestBridge.cs"
// ============================================================
// Attach to a trigger volume GameObject alongside any other
// area logic (fog of war reveal, music change, etc).
// Fires QuestEvents.LocationReached when the player enters.
// ============================================================
// Example GameObject layout:
//   TriggerZone_NorthOutpost
//   ├── FogOfWarRevealer.cs
//   ├── AmbientMusicChanger.cs
//   └── LocationReachedQuestBridge.cs  ← this file
// ============================================================
using Crunchies.PlayerSystem;
using Crunchies.ScriptableObjects;
using Crunchies.AI;
using UnityEngine;

namespace Crunchies.QuestSystem
{
    public class PlayerLocationTrigger : MonoBehaviour
    {
        [Tooltip("Must match the LocationDataSO set in ReachedLocationObjective")]
        [SerializeField] private LocationDataSO locationData;
        [SerializeField] private bool triggerOnceForPlayer = true;

        private bool _hasBeenTriggeredByPlayer = false;

        private void OnTriggerEnter(Collider other)
        {
            if (_hasBeenTriggeredByPlayer || !other.transform.TryGetComponent<Player>(out _)) return;
            if (triggerOnceForPlayer) _hasBeenTriggeredByPlayer = true;
            QuestEvents.LocationReached(locationData.locationId);
        }
    }
}