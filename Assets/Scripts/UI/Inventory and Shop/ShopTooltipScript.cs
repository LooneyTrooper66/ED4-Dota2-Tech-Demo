using UnityEngine;
using UnityEngine.EventSystems;


public class ShopTooltipScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject panel;


    void Start()
    {
        panel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        panel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false);
    }
}
