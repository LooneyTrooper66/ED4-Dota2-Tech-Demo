using UnityEngine;
using UnityEngine.UI;

public class ShowHideShop : MonoBehaviour
{
    public Button button;
    public Canvas shopCanvas;


    void Start()
    {
        shopCanvas.enabled = false;
        button.onClick.AddListener(ShowHide);
    }

    void ShowHide()
    {
        if (shopCanvas.enabled == false) shopCanvas.enabled = true;
        else shopCanvas.enabled = false;
    }
}
