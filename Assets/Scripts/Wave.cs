using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditorInternal;
using UnityEditor;
#endif

// DO NOT CHANGE!!!! WILL OVERWRITE ALL WAVES IN THE GAME
// MARK AS READ ONLY !!
[CreateAssetMenu(fileName = "Wavey", menuName = "Game/Wavey")]
public class Wave : ScriptableObject
{
	[Serializable]
	public class WaveItem
	{
		public bool isGroup;
		public GameObject prefab; // Used if isGroup == false
		public List<WaveItem> children; // Used if isGroup == true
		public float endDelay;
		public int repeatCount = 1;
	}

	public List<WaveItem> waveItems;
	public float endDelay;
}
#if UNITY_EDITOR

[CustomEditor(typeof(Wave))]
public class WaveEditor : Editor
{
	ReorderableList rootWaveItemsList;

	private void OnEnable()
	{
		rootWaveItemsList = new ReorderableList(serializedObject, serializedObject.FindProperty("waveItems"), true, true, true, true);

		rootWaveItemsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
		{
			SerializedProperty item = rootWaveItemsList.serializedProperty.GetArrayElementAtIndex(index);
			SerializedProperty isGroupProp = item.FindPropertyRelative("isGroup");
			SerializedProperty prefabProp = item.FindPropertyRelative("prefab");
			SerializedProperty childrenProp = item.FindPropertyRelative("children");
			SerializedProperty endDelayProp = item.FindPropertyRelative("endDelay");
			SerializedProperty repeatCountProp = item.FindPropertyRelative("repeatCount");

			rect.y += 2;

			float halfWidth = rect.width / 2;
			Rect isGroupRect = new Rect(rect.x, rect.y, halfWidth - 10, EditorGUIUtility.singleLineHeight);
			EditorGUI.PropertyField(isGroupRect, isGroupProp, new GUIContent("Is Group"));

			Rect endDelayRect = new Rect(rect.x + halfWidth, rect.y, halfWidth - 10, EditorGUIUtility.singleLineHeight);
			EditorGUI.PropertyField(endDelayRect, endDelayProp, new GUIContent("End Delay"));

			rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			// Draw repeat count
			EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), repeatCountProp, new GUIContent("Repeat Count"));
			rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			if (isGroupProp.boolValue)
			{
				// Draw the children list
				EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(childrenProp, true)), childrenProp, new GUIContent("Children"), true);
			}
			else
			{
				EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), prefabProp, new GUIContent("Prefab"));
			}
		};


		rootWaveItemsList.elementHeightCallback = (index) =>
		{
			SerializedProperty item = rootWaveItemsList.serializedProperty.GetArrayElementAtIndex(index);
			SerializedProperty isGroupProp = item.FindPropertyRelative("isGroup");
			SerializedProperty childrenProp = item.FindPropertyRelative("children");

			float baseHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			float propertyHeight = baseHeight;

			propertyHeight += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // repeat height

			// Check if it's a group, add the height of the children property.
			if (isGroupProp.boolValue)
			{
				propertyHeight += EditorGUI.GetPropertyHeight(childrenProp, true) + EditorGUIUtility.standardVerticalSpacing;
			}
			// If not a group, just consider the prefab property.
			else
			{
				SerializedProperty prefabProp = item.FindPropertyRelative("prefab");
				propertyHeight += EditorGUI.GetPropertyHeight(prefabProp, true) + EditorGUIUtility.standardVerticalSpacing;
			}
			return propertyHeight;
		};


		rootWaveItemsList.drawElementBackgroundCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
		{
			if (isActive)
				GUI.Box(rect, "");
		};

		rootWaveItemsList.onAddCallback = (ReorderableList list) =>
		{
			var index = list.serializedProperty.arraySize;
			list.serializedProperty.arraySize++;
			list.index = index;
		};

		rootWaveItemsList.onRemoveCallback = (ReorderableList list) =>
		{
			if (list.index > -1)
				list.serializedProperty.DeleteArrayElementAtIndex(list.index);
			list.index = list.index - 1;
		};
	}


	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		rootWaveItemsList.DoLayoutList();

		serializedObject.ApplyModifiedProperties();
	}
}



#endif
