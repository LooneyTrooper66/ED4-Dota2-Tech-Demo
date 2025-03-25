using UnityEngine;

public class NeutralCreepsCanAttack : MonoBehaviour
{
    private LaneCreepCombat creepCombat;
    private HeroCombat heroCombat;
    public bool hostile = false;


    private void Start()
    {
        if (TryGetComponent<LaneCreepCombat>(out LaneCreepCombat laneAI))
        {
            creepCombat = laneAI.GetComponent<LaneCreepCombat>();
        }

        if (TryGetComponent<HeroCombat>(out HeroCombat heroAI))
        {
            heroCombat = heroAI.GetComponent<HeroCombat>();
        }
    }

    private void Update()
    {
        if (heroCombat != null)
        {
            if (heroCombat.state == HeroCombat.currentState.attackingOther)
            {
                hostile = true;
            }
            else
            {
                hostile = false;
            }
        }

        if (creepCombat != null)
        {
            if (creepCombat.state == LaneCreepCombat.currentState.attackingOther)
            {
                hostile = true;
            }
            else
            {
                hostile = false;
            }
        }
    }
}
