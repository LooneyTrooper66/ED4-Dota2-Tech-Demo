using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SellButton : MonoBehaviour
{
    [SerializeField] private Button sellButton;
    [SerializeField] private StaticInventoryDisplay sellScr;
    private InventorySlot_UI slotUI;


    private void Start()
    {
        sellButton.onClick.AddListener(DoThing);
        slotUI = GetComponent<InventorySlot_UI>();
    }

    void DoThing()
    {
        sellScr.SellCurrentSlot(slotUI);
    }
}
