using UnityEngine;

[CreateAssetMenu(fileName = "New ShopItem", menuName = "Shop/Item")]
public class ShopItem : ScriptableObject
{
	public string itemName;
	public int cost;
	public GameObject prefab;
}
