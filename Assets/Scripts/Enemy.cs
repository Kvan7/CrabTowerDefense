using Mirror;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
	public int health = 100;

	public int damageScore = 5;
	
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
		FindObjectOfType<ShopManager>().AddMoney(10);
		// destroy enemy
		Destroy(gameObject);
	}

	public void CompletedPath()
	{
		GameObject.Find("HealthManager").GetComponent<HealthManager>().ScoreTakeDamage(damageScore);
		Destroy(gameObject);
	}
}