using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : NetworkBehaviour
{

	[SyncVar(hook = nameof(OnMoneyChanged))]
	public int money = 100; // Start with 100 money
	public WaveSpawner waveSpawner;
	[SerializeField] private TMP_Text moneyText;
	public Transform spawnPoint; // Position to spawn towers

	public AudioSource audioSource;
	public AudioClip buySuccessClip;
	public AudioClip buyFailClip;

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

	private void OnMoneyChanged(int oldMoney, int newMoney)
	{
		if (isServer)
		{
			RpcUpdateAmount(newMoney);
		}
	}

	[ClientRpc]
	private void RpcUpdateAmount(int newMoney)
	{
		money = newMoney;
		UpdateMoneyUI();
	}

	public void AddMoney(int amount)
	{
		int oldMoney = money;
		money += amount;
		OnMoneyChanged(oldMoney, money);
		UpdateMoneyUI();
	}

	public bool SpendMoney(int amount)
	{
		if (money >= amount)
		{
			int oldMoney = money;
			money -= amount;
			OnMoneyChanged(oldMoney, money);
			UpdateMoneyUI();
			return true; // Purchase successful
		}
		return false; // Not enough money
	}

	public void PurchaseItem(ShopItem item)
	{
		if (SpendMoney(item.cost))
		{
			PurchaseItemSuccessNoise();
			// Instantiate tower or apply upgrade
			GameObject tower = Instantiate(item.prefab, spawnPoint.position, Quaternion.identity);
			NetworkServer.Spawn(tower);
		}
		else
		{
			// Show feedback: Not enough money
			PurchaseItemFailNoise();
		}
	}

	[ClientRpc]
	private void PurchaseItemSuccessNoise()
	{
		audioSource.PlayOneShot(buySuccessClip);
	}

	[ClientRpc]
	private void PurchaseItemFailNoise()
	{
		audioSource.PlayOneShot(buyFailClip);
	}
}