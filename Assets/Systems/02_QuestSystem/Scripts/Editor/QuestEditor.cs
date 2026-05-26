using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;

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

        public override void OnInspectorGUI()
        {
            DrawPropertiesExcluding(serializedObject, "objectives");

            serializedObject.Update();

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
    }
}
