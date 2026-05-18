using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyDataEditor", menuName = "ScriptableObjects/Editor")]
public class EnemyDataEditorSO : ScriptableObject
{
    public enum EnemyFaction
    {
        Beasts,
        Undead,
        Machine,
        Human,
        Alien
    }

    public enum DamageType
    {
        Physical,
        Fire,
        Ice,
        Poison,
        Electric,
        Arcane
    }

    [Serializable]
    public class ResistanceEntry
    {
        // -1 means vulnerable, 0 means neutral, 1 means fully resistant.
        public DamageType damageType;
        [Range(-1f, 1f)] public float multiplier;
    }

    [Serializable]
    public class LootDrop
    {
        public string itemId;
        [Range(0f, 1f)] public float dropChance = 0.25f;
        public int minAmount = 1;
        public int maxAmount = 1;
    }

    [Serializable]
    public class AbilityData
    {
        public string abilityName;
        public float cooldown = 5f;
        public bool startsUnlocked = true;
        public AnimationCurve powerOverLevel = AnimationCurve.Linear(1f, 1f, 50f, 2f);
    }

    [Header("Identity")]
    public string enemyId = "enemy_001";
    public string enemyName = "New Enemy";
    [TextArea(3, 6)] public string description;
    public EnemyFaction faction;
    [Min(1)] public int level = 1;
    public bool isBoss;
    public bool canRespawn = true;
    [Min(0f)] public float respawnDelay = 12f;

    [Header("Combat Stats")]
    [Min(1f)] public float health = 100f;
    [Min(0f)] public float damage = 12f;
    [Min(0f)] public float armor = 4f;
    [Min(0f)] public float speed = 3f;
    [Range(0f, 100f)] public float accuracy = 80f;
    [Range(0f, 100f)] public float evasion = 10f;
    [Range(0f, 100f)] public float criticalChance = 5f;
    [Min(1f)] public float criticalDamage = 1.5f;

    [Header("Behavior / Visual")]
    public Color editorTint = Color.red;
    public Vector2 patrolDistanceRange = new Vector2(2f, 12f);
    public AnimationCurve aggressionByHealth = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

    [Header("Collections")]
    public List<string> tags = new List<string>();
    public List<ResistanceEntry> resistances = new List<ResistanceEntry>();
    public List<LootDrop> lootTable = new List<LootDrop>();
    public List<AbilityData> abilities = new List<AbilityData>();
}
