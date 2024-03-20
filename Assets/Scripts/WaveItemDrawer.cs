#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

// This attribute specifies that this editor script should be used for WaveItem objects
[CustomPropertyDrawer(typeof(Wave.WaveItem))]
public class WaveItemDrawer : PropertyDrawer
{
	// This will draw the property in the inspector
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		bool isGroup = property.FindPropertyRelative("isGroup").boolValue;

		float singleLineHeight = EditorGUIUtility.singleLineHeight;
		float y = position.y;
		SerializedProperty isGroupProp = property.FindPropertyRelative("isGroup");
		SerializedProperty endDelayProp = property.FindPropertyRelative("endDelay");

		float halfWidth = position.width / 2;
		Rect isGroupRect = new Rect(position.x, position.y, halfWidth - 10, EditorGUIUtility.singleLineHeight);
		EditorGUI.PropertyField(isGroupRect, isGroupProp, new GUIContent("Is Group"));

		Rect endDelayRect = new Rect(position.x + halfWidth, position.y, halfWidth - 10, EditorGUIUtility.singleLineHeight);
		EditorGUI.PropertyField(endDelayRect, endDelayProp, new GUIContent("End Delay"));

		y += singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		// Draw the repeat count
		SerializedProperty repeatCountProp = property.FindPropertyRelative("repeatCount");
		EditorGUI.PropertyField(new Rect(position.x, y, position.width, singleLineHeight),
								repeatCountProp, new GUIContent("Repeat Count"));
		y += singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

		// Depending on whether it is a group, draw either the children list or the prefab field
		if (isGroup)
		{
			// Draw the children list
			EditorGUI.PropertyField(new Rect(position.x, y, position.width, position.height - y),
									property.FindPropertyRelative("children"), true);
		}
		else
		{
			// Draw the prefab field
			EditorGUI.PropertyField(new Rect(position.x, y, position.width, singleLineHeight),
									property.FindPropertyRelative("prefab"));
		}

		EditorGUI.EndProperty();
	}

	// Optionally override this to control the property height
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Height for 'isGroup' toggle
																									 // height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Height for 'endDelay'

		bool isGroup = property.FindPropertyRelative("isGroup").boolValue;
		height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Height for 'repeatCount' field
		if (isGroup)
		{
			SerializedProperty childrenProp = property.FindPropertyRelative("children");

			height += EditorGUI.GetPropertyHeight(childrenProp, true) + EditorGUIUtility.standardVerticalSpacing;
		}
		else
		{
			// Height for 'prefab' field
			height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		}

		return height;
	}



}
#endif