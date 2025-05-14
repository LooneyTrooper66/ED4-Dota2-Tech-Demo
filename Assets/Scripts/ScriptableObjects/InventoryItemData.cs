using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemData", menuName = "Scriptable Objects/InventoryItemData")]
public class InventoryItemData : ScriptableObject
{
    public string itemName;
    public float itemCost;
    public Sprite icon;
    public int maxStackSize;
    public string itemFunctionName;
    public bool canBeUsed;
    public float itemUseCooldown;
}
