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
using UnityEngine;

namespace Crunchies.QuestSystem
{
    public class LocationReachedQuestBridge : MonoBehaviour
    {
        [Tooltip("Must match the locationId set in ReachedLocationObjective")]
        [SerializeField] private string locationId = "village_gate";
        [SerializeField] private bool triggerOnce = true;

        private bool _triggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (_triggered || !other.transform.TryGetComponent<Player>(out _)) return;
            if (triggerOnce) _triggered = true;
            QuestEvents.LocationReached(locationId);
        }
    }
}