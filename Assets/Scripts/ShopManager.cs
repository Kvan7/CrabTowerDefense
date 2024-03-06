using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

	public int money = 100; // Start with 100 money
	public WaveSpawner waveSpawner;
	[SerializeField] private TMP_Text moneyText;
	public Transform spawnPoint; // Position to spawn towers

	// Start is called before the first frame update
	void Start()
	{
		if (waveSpawner != null)
		{
			waveSpawner.onWaveComplete.AddListener(WaveCompleted);
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnDestroy()
	{
		// Don't forget to remove the listener when this object is destroyed
		if (waveSpawner != null)
		{
			waveSpawner.onWaveComplete.RemoveListener(WaveCompleted);
		}
	}

	private void WaveCompleted()
	{
		AddMoney(50);
	}

	void UpdateMoneyUI()
	{
		moneyText.text = "Money: " + money.ToString();
	}


	public void AddMoney(int amount)
	{
		money += amount;
		UpdateMoneyUI();
	}

	public bool SpendMoney(int amount)
	{
		if (money >= amount)
		{
			money -= amount;
			UpdateMoneyUI();
			return true; // Purchase successful
		}
		return false; // Not enough money
	}

	public void PurchaseItem(ShopItem item)
	{
		if (SpendMoney(item.cost))
		{
			// Instantiate tower or apply upgrade
			Instantiate(item.prefab, spawnPoint.position, Quaternion.identity);
		}
		else
		{
			// Show feedback: Not enough money
		}
	}
}