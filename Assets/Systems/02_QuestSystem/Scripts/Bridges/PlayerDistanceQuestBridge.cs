// ============================================================
// Script: "PlayerDistanceQuestBridge.cs"
// ============================================================
// Attach alongside your existing PlayerController on the player
// GameObject. Measures how far the player moves each frame and
// forwards the delta to the quest system.
// ============================================================
// Does NOT touch movement logic — it only observes position.
// ============================================================
// Example GameObject layout:
//   Player
//   ├── PlayerController.cs        ← movement, input, animation
//   ├── PlayerStats.cs             ← health, stamina
//   └── PlayerDistanceQuestBridge.cs  ← this file
// ============================================================
using UnityEngine;

namespace Crunchies.QuestSystem
{
    public class PlayerDistanceQuestBridge : MonoBehaviour
    {
        private Vector3 _lastPosition;

        private void Start()
        {
            _lastPosition = transform.position;
        }

        private void Update()
        {
            Vector3 currentPosition = transform.position;
            Vector3 planarDelta = currentPosition - _lastPosition;
            planarDelta.y = 0f;

            float sqrDistance = planarDelta.sqrMagnitude;
            if (sqrDistance > 0.000001f)
            {
                QuestEvents.DistanceTraveled(Mathf.Sqrt(sqrDistance));
            }

            _lastPosition = currentPosition;
        }
    }
}
