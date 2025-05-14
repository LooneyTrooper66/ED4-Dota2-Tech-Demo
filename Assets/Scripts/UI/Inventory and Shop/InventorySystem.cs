using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlots> inventorySlots;

    public List<InventorySlots> pInventorySlots => inventorySlots;
    public int InventorySize => pInventorySlots.Count;

    public UnityAction<InventorySlots> OnInventorySlotChanged;


    public InventorySystem(int size)
    {
        inventorySlots = new List<InventorySlots>(size);

        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlots());
        }
    }

    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
    {
        if(ContainsItem(itemToAdd, out List<InventorySlots> invSlots))
        {
            foreach (var slot in invSlots)
            {
                if(slot.RoomLeftInStack(amountToAdd))
                {
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }
        }

        if (HasFreeSlot(out InventorySlots freeSlot))
        {
            freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
            OnInventorySlotChanged?.Invoke(freeSlot);
            return true;
        }

        return false;
    }

    public void RemoveFromInventory(InventorySlots slotToClear, int amountToRemove)
    {
        slotToClear.RemoveFromStack(amountToRemove);
        OnInventorySlotChanged?.Invoke(slotToClear);
    }

    public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlots> invSlot)
    {
        GameObject playerInv = GameObject.Find("playerInventory");
        StaticInventoryDisplay playerInvDisplay = playerInv.GetComponent<StaticInventoryDisplay>();
        invSlot = playerInvDisplay.InventorySystem.pInventorySlots.Where(i => i.ItemData == itemToAdd).ToList();
        return invSlot == null ? false : true;
    }

    public bool HasFreeSlot(out InventorySlots freeSlot)
    {
        GameObject playerInv = GameObject.Find("playerInventory");
        StaticInventoryDisplay playerInvDisplay = playerInv.GetComponent<StaticInventoryDisplay>();
        freeSlot = playerInvDisplay.InventorySystem.pInventorySlots.FirstOrDefault(i => i.ItemData == null);
        return freeSlot == null ? false : true;
    }
}
