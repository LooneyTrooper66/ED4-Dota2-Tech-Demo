using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public abstract class InventoryDisplay : MonoBehaviour
{
    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlot_UI, InventorySlots> slotsDictionary;

    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, InventorySlots> SlotsDictionary => slotsDictionary;


    protected virtual void Start()
    {

    }

    public abstract void AssignSlot(InventorySystem invToDisplay);

    protected virtual void UpdateSlot(InventorySlots updatedSlot)
    {
        GameObject playerInv = GameObject.Find("playerInventory");
        StaticInventoryDisplay playerInvDisplay = playerInv.GetComponent<StaticInventoryDisplay>();
        foreach (var slot in playerInvDisplay.SlotsDictionary)
        {
            if (slot.Value == updatedSlot) // slot value - under the hood slot
            {
                slot.Key.UpdateUISlot(updatedSlot); // slot key - ui representation of value
            }
        }
    }

    public abstract void SlotClicked(InventorySlot_UI clickedSlot);
}
