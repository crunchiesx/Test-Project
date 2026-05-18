# Modular Unity Quest System

## Folder Structure

```
QuestSystem/
├── Core/
│   ├── QuestEvents.cs          ← Static event bus
│   ├── QuestObjective.cs       ← Abstract base class
│   ├── Quest.cs                ← ScriptableObject per quest
│   └── QuestManager.cs         ← Singleton manager
├── Objectives/
│   ├── GatherObjective.cs      ← Collect N items
│   ├── KillObjective.cs        ← Kill N enemies
│   ├── WalkDistanceObjective.cs← Travel N units
│   ├── InteractObjective.cs    ← Interact with N objects
│   ├── ReachLocationObjective.cs← Reach a named area
│   ├── EscortObjective.cs      ← Escort NPC to destination
│   ├── SurviveObjective.cs     ← Survive for N seconds
│   └── CraftObjective.cs       ← Craft N items (bonus example)
├── Factory/
│   └── QuestFactory.cs         ← Code-side quest builders
├── Bridges/
│   ├── QuestGiver.cs           ← NPC quest hand-off
│   ├── Enemy.cs                ← Fires EnemyKilled on death
│   ├── Collectible.cs          ← Fires ItemCollected on pickup
│   ├── LocationTrigger.cs      ← Fires LocationReached on enter
│   ├── Interactable.cs         ← Fires ObjectInteracted on use
│   ├── PlayerMovementTracker.cs← Fires DistanceTravelled each frame
│   └── SurvivalTimer.cs        ← Fires TimeSurvived each frame
└── UI/
    ├── QuestUI.cs              ← Quest log panel
    └── QuestEntryUI.cs         ← Single quest entry prefab script
```

---

## Architecture

```
Gameplay Object             QuestEvents (static bus)          Objective
───────────────             ────────────────────────          ─────────
Enemy.Die()          ──►   EnemyKilled(id)          ──►   KillObjective
Collectible.Pickup() ──►   ItemCollected(id, n)     ──►   GatherObjective
LocationTrigger      ──►   LocationReached(id)      ──►   ReachLocationObjective
PlayerTracker        ──►   DistanceTravelled(d)     ──►   WalkDistanceObjective
Interactable.E key   ──►   ObjectInteracted(id)     ──►   InteractObjective
SurvivalTimer        ──►   TimeSurvived(dt)         ──►   SurviveObjective
NPC arrives          ──►   NpcReachedDestination(id)──►   EscortObjective
CraftingSystem       ──►   ItemCrafted(id, n)       ──►   CraftObjective
                                                            ▼
                                                      Quest.Tick()
                                                            ▼
                                                      QuestManager
                                                    (active → completed)
```

The event bus is the key design choice. Gameplay objects know nothing
about quests. Quests know nothing about gameplay objects. Both sides
only know about QuestEvents.

---

## Scene Setup Checklist

- [ ] Create an empty GameObject → add **QuestManager**
- [ ] Add **PlayerMovementTracker** to your player
- [ ] Add **SurvivalTimer** to your GameManager (set `isRunning` in code)
- [ ] Add **LocationTrigger** to trigger volumes (set matching `locationId`)
- [ ] Add **Collectible** to pickup prefabs (set matching `itemId`)
- [ ] Add **Enemy** to enemy prefabs (set matching `enemyId`)
- [ ] Add **Interactable** to interactive props (set matching `objectId`)
- [ ] Add **QuestGiver** to NPC prefabs (drag a Quest SO or set `questType`)
- [ ] Wire **QuestUI** panel in your HUD canvas

---

## Quick Start — give a quest from code

```csharp
// Single-objective kill quest
Quest q = QuestFactory.CreateKillQuest("k_wolves_01", "wolf", "Wolves", 5);
QuestManager.Instance.AcceptQuest(q);

// Multi-objective compound quest
Quest errand = QuestFactory.CreateRangersErrand();
QuestManager.Instance.AcceptQuest(errand);
```

---

## How to Add a New Objective Type

**Only two files need to change: your new file, and QuestEvents.cs.**

### Step 1 — Add the event to QuestEvents.cs

```csharp
public static event Action<string, int> OnItemCrafted;
public static void ItemCrafted(string recipeId, int amount = 1)
    => OnItemCrafted?.Invoke(recipeId, amount);
```

### Step 2 — Create Objectives/YourObjective.cs

```csharp
[Serializable]
public class YourObjective : QuestObjective
{
    public string targetId = "some_id";

    public YourObjective(string targetId, int count)
    {
        this.targetId  = targetId;
        requiredAmount = count;
        description    = $"Do the thing {count} times";
    }

    public override void RegisterListeners()
        => QuestEvents.OnSomeEvent += OnSomeEvent;

    public override void UnregisterListeners()
        => QuestEvents.OnSomeEvent -= OnSomeEvent;

    private void OnSomeEvent(string id, int n)
    {
        if (id == targetId) AddProgress(n);
    }

    public override string GetProgressText()
        => $"Thing done: {currentAmount:0} / {requiredAmount:0}";
}
```

### Step 3 — Fire the event from your gameplay system

```csharp
QuestEvents.SomeEvent("some_id", 1);
```

**QuestObjective, Quest, QuestManager, QuestUI — zero changes needed.**

---

## Built-in Objective Reference

| Class | Event that drives it | Key config fields |
|---|---|---|
| `GatherObjective` | `ItemCollected(id, n)` | `itemId`, `requiredAmount` |
| `KillObjective` | `EnemyKilled(id)` | `enemyId`, `requiredAmount` |
| `WalkDistanceObjective` | `DistanceTravelled(delta)` | `requiredAmount`, `unitLabel` |
| `InteractObjective` | `ObjectInteracted(id)` | `objectId`, `uniqueOnly` |
| `ReachLocationObjective` | `LocationReached(id)` | `locationId` |
| `EscortObjective` | `NpcReachedDestination(id)` | `npcId` |
| `SurviveObjective` | `TimeSurvived(delta)` | `requiredAmount` (seconds) |
| `CraftObjective` | `ItemCrafted(id, n)` | `recipeId`, `requiredAmount` |

---

## Notes

- Quest ScriptableObjects store **no runtime state** in the asset itself —
  all state is in `[NonSerialized]` fields, reset via `Quest.ResetRuntime()`.
  This means the same SO can be re-accepted in the same play session.

- The UI uses **TextMeshPro**. If you are on legacy Text, swap
  `TextMeshProUGUI` for `Text` in QuestUI.cs and QuestEntryUI.cs.

- Objectives are not serializable in the Inspector because Unity cannot
  serialize abstract types out of the box. For a fully Inspector-driven
  workflow, wrap each objective in its own ScriptableObject, or use
  Odin Inspector's polymorphic list support.
