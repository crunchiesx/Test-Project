using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameSettingsEditorWindow", menuName = "ScriptableObjects/EditorWindow", order = 2)]
public class GameSettingsEditorWindowSO : ScriptableObject
{
    public enum DifficultyPreset
    {
        Story,
        Normal,
        Hard,
        Nightmare
    }

    public enum GraphicsPreset
    {
        Low,
        Medium,
        High,
        Ultra
    }

    [Serializable]
    public class AccessibilitySettings
    {
        public bool subtitles = true;
        public bool colorBlindMode;
        [Range(0.75f, 2f)] public float uiScale = 1f;
    }

    [Serializable]
    public class KeyBinding
    {
        // Two slots allow alternate keyboard mappings.
        public string actionName;
        public KeyCode primary = KeyCode.None;
        public KeyCode secondary = KeyCode.None;
    }

    [Serializable]
    public class SpawnRule
    {
        public string enemyId;
        [Min(0)] public int weight = 1;
        public bool enabled = true;
    }

    [Header("Profile")]
    public string profileName = "Default";
    [TextArea(2, 4)] public string welcomeMessage = "Welcome to the project.";
    public int highScore;
    [Min(1)] public int maxLives = 3;
    public DifficultyPreset difficulty = DifficultyPreset.Normal;

    [Header("Display / Quality")]
    public GraphicsPreset graphicsPreset = GraphicsPreset.High;
    public bool fullscreen = true;
    public bool vSync = true;
    [Range(30, 240)] public int targetFrameRate = 60;
    public Vector2Int targetResolution = new Vector2Int(1920, 1080);
    public Color uiAccentColor = new Color(0.25f, 0.85f, 1f, 1f);

    [Header("Audio")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 0.8f;
    [Range(0f, 1f)] public float sfxVolume = 0.8f;

    [Header("Progression")]
    public AnimationCurve experienceCurve = AnimationCurve.EaseInOut(1f, 100f, 50f, 5000f);

    [Header("Accessibility")]
    public AccessibilitySettings accessibility = new AccessibilitySettings();

    [Header("Collections")]
    public List<string> enabledMods = new List<string>();
    public List<KeyBinding> keyBindings = new List<KeyBinding>();
    public List<SpawnRule> bonusSpawnRules = new List<SpawnRule>();
}
