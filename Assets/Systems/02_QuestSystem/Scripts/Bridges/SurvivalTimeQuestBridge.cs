// ============================================================
// Script: "SurvivalTimeQuestBridge.cs"
// ============================================================
// Attach to your GameManager or a dedicated survival-mode
// controller. Toggle isRunning from your game state logic
// to control when the survival clock is active.
// ============================================================
// Example GameObject layout:
//   GameManager
//   ├── GameStateManager.cs       ← controls waves, game over
//   └── SurvivalTimeQuestBridge.cs  ← this file
// ============================================================
using UnityEngine;

namespace Crunchies.QuestSystem
{
    public class SurvivalTimeQuestBridge : MonoBehaviour
    {
        [Tooltip("Set to true when the survival challenge is active")]
        public bool isRunning = false;

        private void Update()
        {
            if (isRunning)
            {
                QuestEvents.TimeSurvived(Time.deltaTime);
            }
        }
    }
}
