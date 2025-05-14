using UnityEngine;
using System.Collections.Generic;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private InventorySlot_UI[] slots;
    [SerializeField] private ItemFunctionsHolder itemFunctionsHolder;


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
        if (clickedSlot.AssignedInvSlot.ItemData == null) return;

        // check isnt in cooldown (disable button or smn) pretty much only for blink dagger
        if (clickedSlot.AssignedInvSlot.ItemData.canBeUsed == true)
        {
            string functionName = clickedSlot.AssignedInvSlot.ItemData.itemFunctionName;
            itemFunctionsHolder.Invoke(functionName, 0);

            inventorySystem.RemoveFromInventory(clickedSlot.AssignedInvSlot, 1);

            // activate cooldown if not already
        }
    }
}
