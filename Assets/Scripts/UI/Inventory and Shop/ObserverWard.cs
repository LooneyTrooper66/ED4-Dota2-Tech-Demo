using System;
using UnityEngine;

public class ObserverWard : MonoBehaviour
{
    public ItemFunctionsHolder itemFunctionsHolder;
    float timer = 360;


    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            itemFunctionsHolder.ObserverWardTimerOut();
            Destroy(gameObject);
        }
    }
}
