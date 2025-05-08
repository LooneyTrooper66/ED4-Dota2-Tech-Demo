using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemData", menuName = "Scriptable Objects/InventoryItemData")]
public class InventoryItemData : ScriptableObject
{
    public string itemName;
    public int itemCost;
    public Sprite icon;
    public int maxStackSize;
}
