using Mirror;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
	[SyncVar(hook = nameof(OnCurrentHealthChanged))]
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
		int oldHealth = health;
		health -= damage;
		OnCurrentHealthChanged(oldHealth, health);
	}

	private void OnCurrentHealthChanged(int oldHealth, int newHealth)
	{
		if (isServer)
		{
			RpcUpdateHealth(newHealth);
			if (newHealth <= 0)
			{
				Die();
			}
		}
	}

	[ClientRpc]
	private void RpcUpdateHealth(int newHealth)
	{
		health = newHealth;
	}

	void Die()
	{
		if (isServer)
		{
			// give money to player
			FindObjectOfType<ShopManager>().AddMoney(deathMoney);
			// destroy enemy
			NetworkServer.Destroy(gameObject);
		}
	}

	public void CompletedPath()
	{
		GameObject.Find("HealthManager").GetComponent<HealthManager>().ScoreTakeDamage(damageScore);
		NetworkServer.Destroy(gameObject);
	}
}