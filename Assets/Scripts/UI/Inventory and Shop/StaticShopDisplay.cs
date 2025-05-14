using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class StaticShopDisplay : InventoryDisplay
{
    [SerializeField] private ShopHolder shopHolder;
    [SerializeField] private InventorySlot_UI[] slots;
    [SerializeField] private NewDict newDict;
    [SerializeField] private List<InventorySlot_UI> playerInvSlots;
    Dictionary<InventoryItemData, int> shopItemsDict;
    public HeroStats stats;
    [SerializeField] private ItemFunctionsHolder itemFunctionsHolder;


    protected override void Start()
    {
        base.Start();

        shopItemsDict = newDict.ToDictionary();

        if (shopHolder != null)
        {
            inventorySystem = shopHolder.InventorySystem;
            inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }
        else Debug.LogWarning("no inventory assigned");

        AssignSlot(inventorySystem);
    }

    public override void AssignSlot(InventorySystem invToDisplay)
    {
        slotsDictionary = new Dictionary<InventorySlot_UI, InventorySlots>();

        if (slots.Length != inventorySystem.InventorySize) Debug.Log("inventory slots out of sync");

        for (int i = 0; i < inventorySystem.InventorySize; i++)
        {
            slotsDictionary.Add(slots[i], inventorySystem.pInventorySlots[i]);
            inventorySystem.pInventorySlots[i].UpdateInventorySlot(shopItemsDict.ElementAt(i).Key, shopItemsDict.ElementAt(i).Value);
            slots[i].Init(inventorySystem.pInventorySlots[i]);
        }
    }

    public override void SlotClicked(InventorySlot_UI clickedSlot)
    {
        // if cant afford dont bother with other stuff
        if (stats.totalGold < clickedSlot.AssignedInvSlot.ItemData.itemCost) return;

        for (int i = 0; i < playerInvSlots.Count; i++)
        {
            // check theres space in inventory
            if (inventorySystem.AddToInventory(clickedSlot.AssignedInvSlot.ItemData, 1))
            {
                // need to make sure cant go into negatives lmao
                // must use unreliable first
                float itemCosts = clickedSlot.AssignedInvSlot.ItemData.itemCost;
                if (stats.unreliableGold > 0)
                {
                    if (stats.unreliableGold >= clickedSlot.AssignedInvSlot.ItemData.itemCost)
                    {
                        stats.unreliableGold -= clickedSlot.AssignedInvSlot.ItemData.itemCost;
                        itemCosts = 0;
                    }
                    else
                    {
                        itemCosts -= stats.unreliableGold;
                        stats.unreliableGold = 0;
                    }
                }

                if (itemCosts > 0)
                {
                    stats.reliableGold -= itemCosts;
                }

                if (clickedSlot.AssignedInvSlot.ItemData.canBeUsed == false)
                {
                    string functionName = clickedSlot.AssignedInvSlot.ItemData.itemFunctionName;
                    itemFunctionsHolder.Invoke(functionName, 0);
                }

                return;
            }
        }
    }
}

[System.Serializable]
public class NewDict
{
    [SerializeField] NewDictionary[] shopItemsDict;

    public Dictionary<InventoryItemData, int> ToDictionary()
    {
        Dictionary<InventoryItemData, int> newDict = new Dictionary<InventoryItemData, int>();
        foreach (var item in shopItemsDict)
        {
            newDict.Add(item.invData, item.numInSlot);
        }
        return newDict;
    }
}

[System.Serializable]
public class NewDictionary
{
    [SerializeField] public InventoryItemData invData;
    [SerializeField] public int numInSlot;
}
