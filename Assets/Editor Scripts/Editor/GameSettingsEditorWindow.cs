using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class GameSettingsEditorWindow : EditorWindow
{
    private GameSettingsEditorWindowSO settings;
    private SerializedObject serializedSettings;
    private Vector2 scrollPosition;

    private SerializedProperty profileNameProp;
    private SerializedProperty welcomeMessageProp;
    private SerializedProperty highScoreProp;
    private SerializedProperty maxLivesProp;
    private SerializedProperty difficultyProp;
    private SerializedProperty graphicsPresetProp;
    private SerializedProperty fullscreenProp;
    private SerializedProperty vSyncProp;
    private SerializedProperty targetFrameRateProp;
    private SerializedProperty targetResolutionProp;
    private SerializedProperty uiAccentColorProp;
    private SerializedProperty masterVolumeProp;
    private SerializedProperty musicVolumeProp;
    private SerializedProperty sfxVolumeProp;
    private SerializedProperty experienceCurveProp;
    private SerializedProperty accessibilityProp;
    private SerializedProperty enabledModsProp;
    private SerializedProperty keyBindingsProp;
    private SerializedProperty bonusSpawnRulesProp;

    private ReorderableList modsList;
    private ReorderableList keyBindingsList;
    private ReorderableList spawnRulesList;

    private bool showGeneral = true;
    private bool showDisplay = true;
    private bool showAudio = true;
    private bool showProgression = true;
    private bool showCollections = true;

    [MenuItem("Window/Game Settings Editor")]
    public static void ShowWindow()
    {
        GetWindow<GameSettingsEditorWindow>("Game Settings Editor");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Game Settings (Editor Window)", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("This window demonstrates EditorWindow workflows with SerializedObject, foldouts and ReorderableList controls.", MessageType.Info);

        GameSettingsEditorWindowSO selected = EditorGUILayout.ObjectField("Settings Asset", settings, typeof(GameSettingsEditorWindowSO), false) as GameSettingsEditorWindowSO;
        if (selected != settings)
        {
            settings = selected;
            BindSettings();
        }

        if (settings == null)
        {
            EditorGUILayout.HelpBox("Assign a GameSettingsEditorWindowSO asset to begin editing.", MessageType.Warning);
            return;
        }

        EditorGUILayout.Space();

        if (serializedSettings == null || serializedSettings.targetObject != settings)
        {
            // Rebind when switching assets or after domain reload.
            BindSettings();
        }

        serializedSettings.Update();
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        DrawGeneralSection();

        EditorGUILayout.Space();

        DrawDisplaySection();

        EditorGUILayout.Space();

        DrawAudioSection();

        EditorGUILayout.Space();

        DrawProgressionSection();

        EditorGUILayout.Space();

        DrawCollectionsSection();

        EditorGUILayout.Space();

        DrawActions();

        EditorGUILayout.EndScrollView();
        serializedSettings.ApplyModifiedProperties();
    }

    private void BindSettings()
    {
        if (settings == null)
        {
            serializedSettings = null;
            return;
        }

        // EditorWindow needs its own SerializedObject because we are not inside a CustomEditor.
        serializedSettings = new SerializedObject(settings);

        profileNameProp = serializedSettings.FindProperty("profileName");
        welcomeMessageProp = serializedSettings.FindProperty("welcomeMessage");
        highScoreProp = serializedSettings.FindProperty("highScore");
        maxLivesProp = serializedSettings.FindProperty("maxLives");
        difficultyProp = serializedSettings.FindProperty("difficulty");
        graphicsPresetProp = serializedSettings.FindProperty("graphicsPreset");
        fullscreenProp = serializedSettings.FindProperty("fullscreen");
        vSyncProp = serializedSettings.FindProperty("vSync");
        targetFrameRateProp = serializedSettings.FindProperty("targetFrameRate");
        targetResolutionProp = serializedSettings.FindProperty("targetResolution");
        uiAccentColorProp = serializedSettings.FindProperty("uiAccentColor");
        masterVolumeProp = serializedSettings.FindProperty("masterVolume");
        musicVolumeProp = serializedSettings.FindProperty("musicVolume");
        sfxVolumeProp = serializedSettings.FindProperty("sfxVolume");
        experienceCurveProp = serializedSettings.FindProperty("experienceCurve");
        accessibilityProp = serializedSettings.FindProperty("accessibility");
        enabledModsProp = serializedSettings.FindProperty("enabledMods");
        keyBindingsProp = serializedSettings.FindProperty("keyBindings");
        bonusSpawnRulesProp = serializedSettings.FindProperty("bonusSpawnRules");

        BuildModList();
        BuildKeyBindingList();
        BuildSpawnRuleList();
    }

    private void DrawGeneralSection()
    {
        showGeneral = EditorGUILayout.Foldout(showGeneral, "General", true);
        if (!showGeneral)
        {
            return;
        }

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.PropertyField(profileNameProp);
        EditorGUILayout.PropertyField(welcomeMessageProp);
        EditorGUILayout.PropertyField(difficultyProp);
        maxLivesProp.intValue = EditorGUILayout.IntSlider("Max Lives", maxLivesProp.intValue, 1, 10);
        EditorGUILayout.PropertyField(highScoreProp);
        if (highScoreProp.intValue < 0)
        {
            EditorGUILayout.HelpBox("High score is negative. Usually this should be zero or above.", MessageType.Warning);
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawDisplaySection()
    {
        showDisplay = EditorGUILayout.Foldout(showDisplay, "Display / Quality", true);
        if (!showDisplay)
        {
            return;
        }

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.PropertyField(graphicsPresetProp);
        EditorGUILayout.PropertyField(fullscreenProp);
        if (!fullscreenProp.boolValue)
        {
            EditorGUILayout.PropertyField(targetResolutionProp);
        }
        EditorGUILayout.PropertyField(vSyncProp);
        targetFrameRateProp.intValue = EditorGUILayout.IntSlider("Target Frame Rate", targetFrameRateProp.intValue, 30, 240);
        EditorGUILayout.PropertyField(uiAccentColorProp);
        EditorGUILayout.EndVertical();
    }

    private void DrawAudioSection()
    {
        showAudio = EditorGUILayout.Foldout(showAudio, "Audio", true);
        if (!showAudio)
        {
            return;
        }

        EditorGUILayout.BeginVertical("box");
        masterVolumeProp.floatValue = EditorGUILayout.Slider("Master", masterVolumeProp.floatValue, 0f, 1f);
        musicVolumeProp.floatValue = EditorGUILayout.Slider("Music", musicVolumeProp.floatValue, 0f, 1f);
        sfxVolumeProp.floatValue = EditorGUILayout.Slider("SFX", sfxVolumeProp.floatValue, 0f, 1f);
        if (masterVolumeProp.floatValue <= 0.01f)
        {
            EditorGUILayout.HelpBox("Master volume is almost muted.", MessageType.Info);
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawProgressionSection()
    {
        showProgression = EditorGUILayout.Foldout(showProgression, "Progression / Accessibility", true);
        if (!showProgression)
        {
            return;
        }

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.PropertyField(experienceCurveProp);
        EditorGUILayout.PropertyField(accessibilityProp, true);
        EditorGUILayout.EndVertical();
    }

    private void DrawCollectionsSection()
    {
        showCollections = EditorGUILayout.Foldout(showCollections, "Collections", true);
        if (!showCollections)
        {
            return;
        }

        EditorGUILayout.BeginVertical("box");
        modsList.DoLayoutList();
        keyBindingsList.DoLayoutList();
        spawnRulesList.DoLayoutList();
        EditorGUILayout.EndVertical();
    }

    private void DrawActions()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Increase High Score (+10)"))
        {
            highScoreProp.intValue += 10;
        }

        if (GUILayout.Button("Apply Hard Preset"))
        {
            difficultyProp.enumValueIndex = (int)GameSettingsEditorWindowSO.DifficultyPreset.Hard;
            graphicsPresetProp.enumValueIndex = (int)GameSettingsEditorWindowSO.GraphicsPreset.High;
            masterVolumeProp.floatValue = 0.9f;
            musicVolumeProp.floatValue = 0.7f;
            sfxVolumeProp.floatValue = 1f;
            maxLivesProp.intValue = 3;
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Reset Scores"))
        {
            highScoreProp.intValue = 0;
        }
    }

    private void BuildModList()
    {
        modsList = new ReorderableList(serializedSettings, enabledModsProp, true, true, true, true);
        modsList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Enabled Mods");
        modsList.drawElementCallback = (rect, index, active, focused) =>
        {
            rect.y += 2f;
            EditorGUI.PropertyField(rect, enabledModsProp.GetArrayElementAtIndex(index), GUIContent.none);
        };
    }

    private void BuildKeyBindingList()
    {
        keyBindingsList = new ReorderableList(serializedSettings, keyBindingsProp, true, true, true, true);
        keyBindingsList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Key Bindings");
        keyBindingsList.elementHeight = (EditorGUIUtility.singleLineHeight * 2f) + 8f;
        keyBindingsList.drawElementCallback = (rect, index, active, focused) =>
        {
            SerializedProperty element = keyBindingsProp.GetArrayElementAtIndex(index);
            SerializedProperty actionName = element.FindPropertyRelative("actionName");
            SerializedProperty primary = element.FindPropertyRelative("primary");
            SerializedProperty secondary = element.FindPropertyRelative("secondary");

            rect.y += 2f;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), actionName);
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + 2f, rect.width * 0.5f - 2f, EditorGUIUtility.singleLineHeight), primary);
            EditorGUI.PropertyField(new Rect(rect.x + rect.width * 0.5f + 2f, rect.y + EditorGUIUtility.singleLineHeight + 2f, rect.width * 0.5f - 2f, EditorGUIUtility.singleLineHeight), secondary);
        };
    }

    private void BuildSpawnRuleList()
    {
        spawnRulesList = new ReorderableList(serializedSettings, bonusSpawnRulesProp, true, true, true, true);
        spawnRulesList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Bonus Spawn Rules");
        spawnRulesList.elementHeight = (EditorGUIUtility.singleLineHeight * 2f) + 8f;
        spawnRulesList.drawElementCallback = (rect, index, active, focused) =>
        {
            SerializedProperty element = bonusSpawnRulesProp.GetArrayElementAtIndex(index);
            SerializedProperty enemyId = element.FindPropertyRelative("enemyId");
            SerializedProperty weight = element.FindPropertyRelative("weight");
            SerializedProperty enabled = element.FindPropertyRelative("enabled");

            rect.y += 2f;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), enemyId);
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + 2f, rect.width * 0.5f - 2f, EditorGUIUtility.singleLineHeight), weight);
            EditorGUI.PropertyField(new Rect(rect.x + rect.width * 0.5f + 2f, rect.y + EditorGUIUtility.singleLineHeight + 2f, rect.width * 0.5f - 2f, EditorGUIUtility.singleLineHeight), enabled);
        };
    }
}
