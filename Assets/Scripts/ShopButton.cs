using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Mirror;

public class ShopButton : NetworkBehaviour
{
	public ShopItem item;
	public TMP_Text labelText;

	// Start is called before the first frame update
	void Start()
	{
		labelText.text = item.itemName + '\n' + item.cost.ToString();
	}

	[Command(requiresAuthority = false)]
	public void Buy()
	{
		FindObjectOfType<ShopManager>().PurchaseItem(item);
	}

}