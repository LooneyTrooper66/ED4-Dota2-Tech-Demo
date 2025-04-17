using UnityEngine;

public class NeutralCreepsCanAttack : MonoBehaviour
{
    private LaneCreepCombat laneMelee;
    private RangedLaneCreepCombat laneRanged;
    private SiegeCombat siegeCombat;
    private NeutralMeleeCombat neutralMelee;
    private NeutralRangedCombat neutralRanged;
    private HeroCombat heroCombat;
    public bool hostile = false;


    private void Start()
    {
        if (TryGetComponent<LaneCreepCombat>(out LaneCreepCombat laneCom))
        {
            laneMelee = laneCom.GetComponent<LaneCreepCombat>();
        }

        if (TryGetComponent<RangedLaneCreepCombat>(out RangedLaneCreepCombat laneRCom))
        {
            laneRanged = laneRCom.GetComponent<RangedLaneCreepCombat>();
        }

        if (TryGetComponent<SiegeCombat>(out SiegeCombat laneSCom))
        {
            siegeCombat = laneSCom.GetComponent<SiegeCombat>();
        }

        if (TryGetComponent<NeutralMeleeCombat>(out NeutralMeleeCombat neutralCom))
        {
            neutralMelee = neutralCom.GetComponent<NeutralMeleeCombat>();
        }

        if (TryGetComponent<NeutralRangedCombat>(out NeutralRangedCombat neutralRCom))
        {
            neutralRanged = neutralRCom.GetComponent<NeutralRangedCombat>();
        }

        if (TryGetComponent<HeroCombat>(out HeroCombat heroCom))
        {
            heroCombat = heroCom.GetComponent<HeroCombat>();
        }
    }

    // set whether is hostile
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

        if (laneMelee != null)
        {
            if (laneMelee.state == LaneCreepCombat.currentState.attackingOther)
            {
                hostile = true;
            }
            else
            {
                hostile = false;
            }
        }

        if (laneRanged != null)
        {
            if (laneRanged.state == RangedLaneCreepCombat.currentState.attackingOther)
            {
                hostile = true;
            }
            else
            {
                hostile = false;
            }
        }

        if (siegeCombat != null)
        {
            if (siegeCombat.state == SiegeCombat.currentState.attackingOther)
            {
                hostile = true;
            }
            else
            {
                hostile = false;
            }
        }

        if (neutralMelee != null)
        {
            if (neutralMelee.state == NeutralMeleeCombat.currentState.attackingOther)
            {
                hostile = true;
            }
            else
            {
                hostile = false;
            }
        }
        
        if (neutralRanged != null)
        {
            if (neutralRanged.state == NeutralRangedCombat.currentState.attackingOther)
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
