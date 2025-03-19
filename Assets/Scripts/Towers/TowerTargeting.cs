using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class TowerTargeting : MonoBehaviour
{
    public string tagToTarget;
    private List<GameObject> potentialTargtets = new (); // ask tom about optimisation later



    private void Update()
    {
        // if current target die

        // if current target leaves range

        // if another potential target starts attacking ally hero
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == tagToTarget)
        {
            potentialTargtets.Add(other.gameObject);
        }
    }

    void CheckTargetingOrder()
    {
        foreach (GameObject target in potentialTargtets)
        {
            // 
        }

        // Closest enemy unit attacking an ally hero
        // Closest enemy unit attacking the tower
        // Closest enemy unit attacking an ally unit
        // Closest enemy unit
        // Siege creep

    }
}
