using UnityEngine;

public class Enemy : MonoBehaviour
{
	public int health = 100;

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
}