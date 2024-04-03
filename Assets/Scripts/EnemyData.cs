using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/Enemy")]
public class EnemyData : ScriptableObject
{
	public int health = 100;
	public int damageScore = 5;
	public int deathMoney = 10;
	public float speedMultiplier = 1.0f;
	public GameObject prefab;
}