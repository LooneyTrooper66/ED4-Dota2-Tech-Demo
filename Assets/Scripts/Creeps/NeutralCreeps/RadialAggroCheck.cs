using UnityEngine;

public class RadialAggroCheck : MonoBehaviour
{
    NeutralCreepAI creepAI;


    private void Start()
    {
        creepAI = GetComponentInParent<NeutralCreepAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        creepAI.FindAndSetCreepTarget();
    }
}
