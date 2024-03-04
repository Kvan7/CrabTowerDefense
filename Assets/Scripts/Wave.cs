using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public abstract class WaveBase : ScriptableObject
{
    public float endDelay;
}

[CreateAssetMenu(fileName = "Wave", menuName = "Game/Wave")]
public class Wave : ScriptableObject
{
    public List<WaveBase> waveElements;

    public WaveBase selectedWaveElements;

    public float endDelay;
}

[CustomEditor(typeof(Wave))]
public class WaveDataEditor : Editor
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

        serializedObject.ApplyModifiedProperties();
    }
}