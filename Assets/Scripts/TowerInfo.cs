using UnityEngine;

[CreateAssetMenu(fileName = "TowerInfo", menuName = "Game/Tower Data")]
public class TowerInfo : ScriptableObject
{
	public float fireRate = 1f;
	public float attackDamage = 50f;
	public float projectileSpeed = 10f;
	public float projectileLifetime = 5f;
	public float attackRange = 10f;
	public float rotationSpeed = 1f;
	public bool instantRotation = false;
}