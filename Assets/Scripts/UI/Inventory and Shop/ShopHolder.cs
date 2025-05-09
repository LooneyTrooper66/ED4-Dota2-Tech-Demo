using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ShopHolder : MonoBehaviour
{
    [SerializeField] private int inventorySize;
    [SerializeField] protected InventorySystem inventorySystem;

    public InventorySystem InventorySystem => inventorySystem;
    public static UnityAction<InventorySystem> OnDynamicInventoryDisplayRequest;


    public void Awake()
    {
        inventorySystem = new InventorySystem(inventorySize);
    }
}
