using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveLeaf", menuName = "Game/WaveLeaf")]
public class WaveLeaf : WaveBase
{
	public GameObject selectedEnemyPrefab;
}

[CustomEditor(typeof(WaveLeaf))]
public class WaveLeafDataEditor : Editor
{
	private SerializedProperty selectedEnemyPrefabProp;

	private void OnEnable()
	{
		selectedEnemyPrefabProp = serializedObject.FindProperty("selectedEnemyPrefab");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		GameObject[] enemyPrefabs = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>().Where(g => g.CompareTag("Enemy")).ToArray();
		string[] enemyPrefabNames = enemyPrefabs.Select(prefab => prefab.name).ToArray();

		int selectedIndex = EditorGUILayout.Popup("Select Enemy Prefab", System.Array.IndexOf(enemyPrefabs, selectedEnemyPrefabProp.objectReferenceValue as GameObject), enemyPrefabNames);

		selectedEnemyPrefabProp.objectReferenceValue = enemyPrefabs[selectedIndex];

		EditorGUILayout.PropertyField(serializedObject.FindProperty("endDelay"));

		serializedObject.ApplyModifiedProperties();
	}
}