using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
	public ShopItem item;
	public TMP_Text labelText;

	// Start is called before the first frame update
	void Start()
	{
		labelText.text = item.itemName + '\n' + item.cost.ToString();
	}

	public void Buy()
	{
		FindObjectOfType<ShopManager>().PurchaseItem(item);
	}

}