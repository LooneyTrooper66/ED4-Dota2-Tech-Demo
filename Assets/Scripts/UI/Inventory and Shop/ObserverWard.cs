using System;
using UnityEngine;

public class ObserverWard : MonoBehaviour
{
    ItemFunctionsHolder itemFunctionsHolder;
    float timer = 360;


    private void Awake()
    {
        itemFunctionsHolder = GameObject.Find("EventSystem").GetComponent<ItemFunctionsHolder>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            gameObject.SetActive(false);
            itemFunctionsHolder.ObserverWardTimerOut();
            Destroy(gameObject);
        }
    }
}
