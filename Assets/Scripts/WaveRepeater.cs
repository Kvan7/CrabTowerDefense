using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveRepeater", menuName = "Game/WaveRepeater")]
public class WaveRepeater : WaveBase
{
    public List<WaveBase> waveElements;
    public WaveBase selectedWaveElements;

    public int repeatCount;
}

[CustomEditor(typeof(WaveRepeater))]
public class WaveRepeaterDataEditor : Editor
{
    private SerializedProperty waveElementsProp;
    private SerializedProperty selectedWaveElementsProp;
    private ReorderableList waveElementsList;

    private void OnEnable()
    {
        waveElementsProp = serializedObject.FindProperty("waveElements");
        selectedWaveElementsProp = serializedObject.FindProperty("selectedWaveElements");

        waveElementsList = new ReorderableList(serializedObject, waveElementsProp, true, true, true, true);

        waveElementsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = waveElementsList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - 30, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
        };

        waveElementsList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Wave Elements");
        };

        waveElementsList.onSelectCallback = (ReorderableList list) =>
        {
            selectedWaveElementsProp.objectReferenceValue = waveElementsProp.GetArrayElementAtIndex(list.index).objectReferenceValue;
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        waveElementsList.DoLayoutList();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("endDelay"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("repeatCount"));

        serializedObject.ApplyModifiedProperties();
    }
}
