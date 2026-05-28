// ============================================================
// Script: "EnemyDeathQuestBridge.cs"
// ============================================================
// Attach alongside your existing Health (or equivalent)
// component on an enemy GameObject. Listens for that script's
// death event and forwards it to the quest system.
// ============================================================
// Your EnemyHealth.cs stays clean — it knows nothing about quests.
// This bridge is the only thing that knows about both.
// ============================================================
// Example GameObject layout:
//   Enemy
//   ├── EnemyAI.cs
//   ├── EnemyHealth.cs       ← has an OnDied event
//   ├── EnemyDrops.cs
//   └── EnemyDeathQuestBridge.cs  ← this file
// ============================================================
using Crunchies.Components;
using Crunchies.ScriptableObjects;
using Crunchies.Utility;
using UnityEngine;

namespace Crunchies.QuestSystem
{
    [RequireComponent(typeof(CharacterHealth))]
    public class EnemyDeathQuestBridge : MonoBehaviour
    {
        [Tooltip("Must match the EnemyDataSO set in KillObjective.")]
        [SerializeField] private EnemyDataSO enemyData;

        private CharacterHealth _health;

        private void Awake()
        {
            if (!TryGetComponent(out _health))
            {
                Log.MissingComponent<CharacterHealth>(this);
            }
        }

        private void OnEnable() => _health.OnDied += HandleDeath;
        private void OnDisable() => _health.OnDied -= HandleDeath;

        private void HandleDeath() => QuestEvents.EnemyKilled(enemyData.characterId);
    }
}