using UnityEditorInternal;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyDataEditorSO))]
public class EnemyDataEditor : Editor
{
    private SerializedProperty enemyIdProp;
    private SerializedProperty enemyNameProp;
    private SerializedProperty descriptionProp;
    private SerializedProperty factionProp;
    private SerializedProperty levelProp;
    private SerializedProperty isBossProp;
    private SerializedProperty canRespawnProp;
    private SerializedProperty respawnDelayProp;
    private SerializedProperty healthProp;
    private SerializedProperty damageProp;
    private SerializedProperty armorProp;
    private SerializedProperty speedProp;
    private SerializedProperty accuracyProp;
    private SerializedProperty evasionProp;
    private SerializedProperty criticalChanceProp;
    private SerializedProperty criticalDamageProp;
    private SerializedProperty editorTintProp;
    private SerializedProperty patrolRangeProp;
    private SerializedProperty aggressionCurveProp;
    private SerializedProperty tagsProp;
    private SerializedProperty resistancesProp;
    private SerializedProperty lootTableProp;
    private SerializedProperty abilitiesProp;

    private ReorderableList tagsList;
    private ReorderableList resistanceList;
    private ReorderableList lootList;
    private ReorderableList abilitiesList;

    private bool showIdentity = true;
    private bool showCombat = true;
    private bool showBehavior = true;
    private bool showCollections = true;
    private bool useDecimalRandomize = true;

    private void OnEnable()
    {
        // Cache SerializedProperty references once so OnInspectorGUI stays focused on drawing.
        enemyIdProp = serializedObject.FindProperty("enemyId");
        enemyNameProp = serializedObject.FindProperty("enemyName");
        descriptionProp = serializedObject.FindProperty("description");
        factionProp = serializedObject.FindProperty("faction");
        levelProp = serializedObject.FindProperty("level");
        isBossProp = serializedObject.FindProperty("isBoss");
        canRespawnProp = serializedObject.FindProperty("canRespawn");
        respawnDelayProp = serializedObject.FindProperty("respawnDelay");
        healthProp = serializedObject.FindProperty("health");
        damageProp = serializedObject.FindProperty("damage");
        armorProp = serializedObject.FindProperty("armor");
        speedProp = serializedObject.FindProperty("speed");
        accuracyProp = serializedObject.FindProperty("accuracy");
        evasionProp = serializedObject.FindProperty("evasion");
        criticalChanceProp = serializedObject.FindProperty("criticalChance");
        criticalDamageProp = serializedObject.FindProperty("criticalDamage");
        editorTintProp = serializedObject.FindProperty("editorTint");
        patrolRangeProp = serializedObject.FindProperty("patrolDistanceRange");
        aggressionCurveProp = serializedObject.FindProperty("aggressionByHealth");
        tagsProp = serializedObject.FindProperty("tags");
        resistancesProp = serializedObject.FindProperty("resistances");
        lootTableProp = serializedObject.FindProperty("lootTable");
        abilitiesProp = serializedObject.FindProperty("abilities");

        BuildTagList();
        BuildResistanceList();
        BuildLootList();
        BuildAbilitiesList();
    }

    public override void OnInspectorGUI()
    {
        // Always sync serialized data before drawing custom controls.
        serializedObject.Update();

        EditorGUILayout.LabelField("Enemy Data (Custom Inspector)", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("This inspector demonstrates serialized fields, foldouts, sliders, enums, curves, color fields and reorderable lists.", MessageType.Info);

        DrawIdentitySection();

        EditorGUILayout.Space();

        DrawCombatSection();

        EditorGUILayout.Space();

        DrawBehaviorSection();

        EditorGUILayout.Space();

        DrawCollectionSection();

        EditorGUILayout.Space();

        DrawActions();

        // Push modified SerializedProperty values back to the target object.
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawIdentitySection()
    {
        showIdentity = EditorGUILayout.Foldout(showIdentity, "Identity", true);
        if (!showIdentity)
        {
            return;
        }

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.PropertyField(enemyIdProp);
        EditorGUILayout.PropertyField(enemyNameProp, new GUIContent("Display Name"));
        EditorGUILayout.PropertyField(descriptionProp);
        EditorGUILayout.PropertyField(factionProp);
        levelProp.intValue = EditorGUILayout.IntSlider("Level", levelProp.intValue, 1, 100);
        EditorGUILayout.PropertyField(isBossProp);
        EditorGUILayout.PropertyField(canRespawnProp);
        if (canRespawnProp.boolValue)
        {
            EditorGUILayout.PropertyField(respawnDelayProp);
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawCombatSection()
    {
        showCombat = EditorGUILayout.Foldout(showCombat, "Combat", true);
        if (!showCombat)
        {
            return;
        }

        EditorGUILayout.BeginVertical("box");
        healthProp.floatValue = EditorGUILayout.Slider("Health", healthProp.floatValue, 1f, 500f);
        damageProp.floatValue = EditorGUILayout.Slider("Damage", damageProp.floatValue, 0f, 200f);
        armorProp.floatValue = EditorGUILayout.Slider("Armor", armorProp.floatValue, 0f, 100f);
        speedProp.floatValue = EditorGUILayout.Slider("Speed", speedProp.floatValue, 0f, 25f);
        accuracyProp.floatValue = EditorGUILayout.Slider("Accuracy", accuracyProp.floatValue, 0f, 100f);
        evasionProp.floatValue = EditorGUILayout.Slider("Evasion", evasionProp.floatValue, 0f, 100f);
        criticalChanceProp.floatValue = EditorGUILayout.Slider("Critical Chance", criticalChanceProp.floatValue, 0f, 100f);
        criticalDamageProp.floatValue = EditorGUILayout.Slider("Critical Damage", criticalDamageProp.floatValue, 1f, 5f);

        if (healthProp.floatValue <= 0f)
        {
            EditorGUILayout.HelpBox("Health should usually be above zero.", MessageType.Warning);
        }

        if (criticalChanceProp.floatValue > 60f && !isBossProp.boolValue)
        {
            EditorGUILayout.HelpBox("Very high critical chance can make non-boss enemies too volatile.", MessageType.Info);
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawBehaviorSection()
    {
        showBehavior = EditorGUILayout.Foldout(showBehavior, "Behavior / Visual", true);
        if (!showBehavior)
        {
            return;
        }

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.PropertyField(editorTintProp);
        EditorGUILayout.PropertyField(aggressionCurveProp);

        Vector2 patrol = patrolRangeProp.vector2Value;
        float min = patrol.x;
        float max = patrol.y;
        EditorGUILayout.MinMaxSlider(new GUIContent("Patrol Distance Range"), ref min, ref max, 0f, 50f);
        patrol.x = min;
        patrol.y = Mathf.Max(min, max);
        patrolRangeProp.vector2Value = patrol;
        patrolRangeProp.vector2Value = EditorGUILayout.Vector2Field("Range (Min/Max)", patrolRangeProp.vector2Value);
        EditorGUILayout.EndVertical();
    }

    private void DrawCollectionSection()
    {
        showCollections = EditorGUILayout.Foldout(showCollections, "Collections", true);
        if (!showCollections)
        {
            return;
        }

        EditorGUILayout.BeginVertical("box");
        tagsList.DoLayoutList();
        resistanceList.DoLayoutList();
        lootList.DoLayoutList();
        abilitiesList.DoLayoutList();
        EditorGUILayout.EndVertical();
    }

    private void DrawActions()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);
        useDecimalRandomize = EditorGUILayout.Toggle("Randomize With Decimals", useDecimalRandomize);

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Randomize Combat Stats"))
        {
            healthProp.floatValue = RandomValue(50f, 250f);
            damageProp.floatValue = RandomValue(5f, 120f);
            armorProp.floatValue = RandomValue(0f, 75f);
            speedProp.floatValue = RandomValue(1f, 12f);
            accuracyProp.floatValue = RandomValue(50f, 100f);
            evasionProp.floatValue = RandomValue(0f, 50f);
            criticalChanceProp.floatValue = RandomValue(1f, 40f);
            criticalDamageProp.floatValue = RandomValue(1f, 3f);
        }

        if (GUILayout.Button("Reset Combat Stats"))
        {
            healthProp.floatValue = 100f;
            damageProp.floatValue = 10f;
            armorProp.floatValue = 5f;
            speedProp.floatValue = 3f;
            accuracyProp.floatValue = 80f;
            evasionProp.floatValue = 10f;
            criticalChanceProp.floatValue = 5f;
            criticalDamageProp.floatValue = 1.5f;
        }
        EditorGUILayout.EndHorizontal();
    }

    private void BuildTagList()
    {
        // ReorderableList is useful for array/list editing with drag + add/remove controls.
        tagsList = new ReorderableList(serializedObject, tagsProp, true, true, true, true);
        tagsList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Tags");
        tagsList.drawElementCallback = (rect, index, active, focused) =>
        {
            rect.y += 2f;
            EditorGUI.PropertyField(rect, tagsProp.GetArrayElementAtIndex(index), GUIContent.none);
        };
    }

    private void BuildResistanceList()
    {
        resistanceList = new ReorderableList(serializedObject, resistancesProp, true, true, true, true);
        resistanceList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Resistances");
        resistanceList.elementHeight = EditorGUIUtility.singleLineHeight + 6f;
        resistanceList.drawElementCallback = (rect, index, active, focused) =>
        {
            SerializedProperty element = resistancesProp.GetArrayElementAtIndex(index);
            SerializedProperty type = element.FindPropertyRelative("damageType");
            SerializedProperty multiplier = element.FindPropertyRelative("multiplier");

            rect.y += 2f;
            float leftWidth = rect.width * 0.5f;
            Rect typeRect = new Rect(rect.x, rect.y, leftWidth - 4f, EditorGUIUtility.singleLineHeight);
            Rect multRect = new Rect(rect.x + leftWidth, rect.y, rect.width - leftWidth, EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(typeRect, type, GUIContent.none);
            EditorGUI.Slider(multRect, multiplier, -1f, 1f, GUIContent.none);
        };
    }

    private void BuildLootList()
    {
        lootList = new ReorderableList(serializedObject, lootTableProp, true, true, true, true);
        lootList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Loot Table");
        lootList.elementHeight = (EditorGUIUtility.singleLineHeight * 4f) + 16f;
        lootList.drawElementCallback = (rect, index, active, focused) =>
        {
            SerializedProperty element = lootTableProp.GetArrayElementAtIndex(index);
            SerializedProperty itemId = element.FindPropertyRelative("itemId");
            SerializedProperty dropChance = element.FindPropertyRelative("dropChance");
            SerializedProperty minAmount = element.FindPropertyRelative("minAmount");
            SerializedProperty maxAmount = element.FindPropertyRelative("maxAmount");

            const float vPad = 2f;
            float line = EditorGUIUtility.singleLineHeight;
            rect.y += vPad;

            Rect itemRect = new Rect(rect.x, rect.y, rect.width, line);
            Rect chanceRect = new Rect(rect.x, rect.y + line + vPad, rect.width, line);
            Rect minRect = new Rect(rect.x, rect.y + (line * 2f) + (vPad * 2f), rect.width, line);
            Rect maxRect = new Rect(rect.x, rect.y + (line * 3f) + (vPad * 3f), rect.width, line);

            EditorGUI.PropertyField(itemRect, itemId, new GUIContent("Item ID"));
            EditorGUI.Slider(chanceRect, dropChance, 0f, 1f, new GUIContent("Drop Chance"));
            EditorGUI.PropertyField(minRect, minAmount, new GUIContent("Min"));
            EditorGUI.PropertyField(maxRect, maxAmount, new GUIContent("Max"));

            // Simple validation example done directly in the editor UI pass.
            if (maxAmount.intValue < minAmount.intValue)
            {
                maxAmount.intValue = minAmount.intValue;
            }
        };
    }

    private void BuildAbilitiesList()
    {
        abilitiesList = new ReorderableList(serializedObject, abilitiesProp, true, true, true, true);
        abilitiesList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Abilities");
        abilitiesList.elementHeight = (EditorGUIUtility.singleLineHeight * 4f) + 14f;
        abilitiesList.drawElementCallback = (rect, index, active, focused) =>
        {
            SerializedProperty element = abilitiesProp.GetArrayElementAtIndex(index);
            rect.y += 2f;

            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("abilityName"));
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + 2f, rect.width * 0.5f - 4f, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("cooldown"));
            EditorGUI.PropertyField(new Rect(rect.x + rect.width * 0.5f, rect.y + EditorGUIUtility.singleLineHeight + 2f, rect.width * 0.5f, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("startsUnlocked"));
            EditorGUI.PropertyField(new Rect(rect.x, rect.y + (EditorGUIUtility.singleLineHeight * 2f) + 4f, rect.width, (EditorGUIUtility.singleLineHeight * 2f) + 4f), element.FindPropertyRelative("powerOverLevel"));
        };
    }

    private float RandomValue(float min, float max)
    {
        float raw = Random.Range(min, max);
        return useDecimalRandomize ? Mathf.Round(raw * 10f) / 10f : Mathf.Round(raw);
    }
}
