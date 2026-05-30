using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using System;
using System.Linq;
using Unity.VisualScripting;

namespace Crunchies.QuestSystem
{
    [CustomEditor(typeof(QuestSO))]
    public class QuestEditor : Editor
    {
        private static readonly List<Type> ObjectiveTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsSubclassOf(typeof(QuestObjective)) && !t.IsAbstract)
            .ToList();

        private SerializedProperty itemRewardsProp;
        private ReorderableList itemRewardsList;

        private bool showItemRewards = true;

        private void OnEnable()
        {
            itemRewardsProp = serializedObject.FindProperty("itemRewards");
            BuildItemRewardsList();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "objectives", "itemRewards");

            EditorGUILayout.Space();
            showItemRewards = EditorGUILayout.Foldout(showItemRewards, "Item Rewards", true);
            if (showItemRewards)
            {
                EditorGUI.indentLevel++;
                itemRewardsList.DoLayoutList();
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Objectives", EditorStyles.boldLabel);

            QuestSO quest = (QuestSO)target;

            for (int i = 0; i < quest.objectives.Count; i++)
            {
                QuestObjective obj = quest.objectives[i];

                EditorGUILayout.BeginVertical(GUI.skin.box);

                // Header row: type name + remove button
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(obj.GetType().Name, EditorStyles.boldLabel);
                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    quest.objectives.RemoveAt(i);
                    EditorUtility.SetDirty(target);
                    break;
                }
                EditorGUILayout.EndHorizontal();

                // Draw all public fields on the objective using SerializedObject so Undo/Redo and dirty-marking work correctly
                SerializedObject so = new(quest);
                SerializedProperty prop = so.FindProperty("objectives").GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(prop, includeChildren: true);
                so.ApplyModifiedProperties();

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(4);
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Add Objective");
            if (EditorGUILayout.DropdownButton(new GUIContent("Select Type"), FocusType.Keyboard))
            {
                GenericMenu menu = new();
                foreach (Type type in ObjectiveTypes)
                {
                    Type captured = type;
                    menu.AddItem(new GUIContent(type.Name), false, () =>
                    {
                        quest.objectives.Add((QuestObjective)Activator.CreateInstance(captured));
                        EditorUtility.SetDirty(target);
                    });
                }
                menu.ShowAsContext();
            }
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        private void BuildItemRewardsList()
        {
            itemRewardsList = new ReorderableList(serializedObject, itemRewardsProp, true, true, true, true)
            {
                drawHeaderCallback = rect =>
                {
                    EditorGUI.LabelField(rect, "Rewards List");
                },

                elementHeight = (EditorGUIUtility.singleLineHeight * 2f) + 10f,

                drawElementCallback = (rect, index, active, focused) =>
                {
                    SerializedProperty element = itemRewardsProp.GetArrayElementAtIndex(index);
                    SerializedProperty itemProp = element.FindPropertyRelative("item");
                    SerializedProperty quantityProp = element.FindPropertyRelative("quantity");

                    rect.y += 2f;

                    Rect itemRect = new(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
                    Rect qtyRect = new(rect.x, rect.y + EditorGUIUtility.singleLineHeight + 2f, rect.width, EditorGUIUtility.singleLineHeight);

                    EditorGUI.PropertyField(itemRect, itemProp, new GUIContent("Item"));
                    EditorGUI.PropertyField(qtyRect, quantityProp, new GUIContent("Quantity"));
                },

                onAddCallback = list =>
                {
                    list.serializedProperty.arraySize++;
                    int newIndex = list.serializedProperty.arraySize - 1;
                    SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(newIndex);

                    element.FindPropertyRelative("quantity").intValue = 1;
                    list.index = newIndex;

                    serializedObject.ApplyModifiedProperties();
                }
            };
        }
    }
}