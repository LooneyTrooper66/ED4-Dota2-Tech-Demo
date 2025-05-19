using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private InventorySlot_UI[] slots;
    [SerializeField] private ItemFunctionsHolder itemFunctionsHolder;
    [SerializeField] private HeroStats stats;
    bool itemArmed = false;
    InventorySlot_UI clickedSlotHolder;



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

            clickedSlotHolder = clickedSlot;
            itemArmed = true;
        }
    }

    private void Update()
    {
        if (itemArmed)
        {
            if (Input.GetMouseButtonDown(1))
            {
                itemArmed = false;
            }
            if (Input.GetMouseButtonDown(0))
            {
                // activate item cooldown or remove from inv if placed
                if (clickedSlotHolder.AssignedInvSlot.ItemData.itemUseCooldown > 0) StartCoroutine(ItemCooldown(clickedSlotHolder, clickedSlotHolder.AssignedInvSlot.ItemData.itemUseCooldown));
                else inventorySystem.RemoveFromInventory(clickedSlotHolder.AssignedInvSlot, 1);
                clickedSlotHolder = null;
                itemArmed = false;
            }
        }
    }

    IEnumerator ItemCooldown(InventorySlot_UI clickedSlot, float cooldownTime)
    {
        clickedSlot.button.enabled = false;
        clickedSlot.itemSprite.color = Color.gray;
        yield return new WaitForSecondsRealtime(cooldownTime);
        clickedSlot.button.enabled = true;
        clickedSlot.itemSprite.color = Color.white;
    }

    public void SellCurrentSlot(InventorySlot_UI clickedSlot)
    {
        if (clickedSlot.AssignedInvSlot.ItemData != null)
        {
            stats.unreliableGold += (clickedSlot.AssignedInvSlot.ItemData.itemCost / 2);

            if (clickedSlot.AssignedInvSlot.ItemData.itemSellFunctionName != null)
            {
                string functionName = clickedSlot.AssignedInvSlot.ItemData.itemSellFunctionName;
                itemFunctionsHolder.Invoke(functionName, 0);
            }

            inventorySystem.RemoveFromInventory(clickedSlot.AssignedInvSlot, 1);
        }
    }
}
