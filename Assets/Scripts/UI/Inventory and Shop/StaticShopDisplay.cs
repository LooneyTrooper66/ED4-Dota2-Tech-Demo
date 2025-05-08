using UnityEngine;
using System.Collections.Generic;

public class StaticShopDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private InventorySlot_UI[] slots;


    protected override void Start()
    {
        base.Start();

        if (inventoryHolder != null)
        {
            inventorySystem = inventoryHolder.InventorySystem;
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
            slots[i].Init(inventorySystem.pInventorySlots[i]);
        }
    }

    public override void SlotClicked(InventorySlot_UI clickedSlot)
    {
        // if has enough money and enough space in inv
        // take required money and add to player inv
        // set cooldown if has one
    }
}
