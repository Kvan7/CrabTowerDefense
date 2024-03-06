using UnityEngine;

[CreateAssetMenu(fileName = "New ShopItem", menuName = "Game/ShopItem")]
public class ShopItem : ScriptableObject
{
	public string itemName;
	public int cost;
	public GameObject prefab;
}
