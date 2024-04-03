using Mirror;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
	public int health = 100;

	private int damageScore = 5;
	private int deathMoney = 10;
	private float speedMultiplier = 1.0f;
	public EnemyData enemyData;

	private void Start()
	{
		// Set the health to the health of the enemy data
		if (enemyData != null)
		{
			Debug.Log("EnemyData found: " + enemyData.health);
			health = enemyData.health;
			damageScore = enemyData.damageScore;
			deathMoney = enemyData.deathMoney;
			speedMultiplier = enemyData.speedMultiplier;
		}
		FollowWaypoints waypoints = GetComponent<FollowWaypoints>();
		if (waypoints != null)
		{
			waypoints.moveSpeed *= speedMultiplier;
			waypoints.rotSpeed *= speedMultiplier;
		}
		Debug.Log(health);
	}

	public void TakeDamage(int damage)
	{
		health -= damage;
		if (health <= 0)
		{
			Die();
		}
	}

	void Die()
	{
		// give money to player
		FindObjectOfType<ShopManager>().AddMoney(deathMoney);
		// destroy enemy
		Destroy(gameObject);
	}

	public void CompletedPath()
	{
		GameObject.Find("HealthManager").GetComponent<HealthManager>().ScoreTakeDamage(damageScore);
		Destroy(gameObject);
	}
}