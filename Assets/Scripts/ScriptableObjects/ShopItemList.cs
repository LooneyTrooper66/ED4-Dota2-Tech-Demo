using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ShopItemList", menuName = "Scriptable Objects/ShopItemList")]
public class ShopItemList : ScriptableObject
{
    [SerializeField] private List<ShopInventoryItem> items;
    [SerializeField] private int goldCosts;
}

[System.Serializable]
public struct ShopInventoryItem
{
    public InventoryItemData itemData;
    public int amount;
}