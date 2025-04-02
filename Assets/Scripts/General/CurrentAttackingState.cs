using UnityEngine;

public class CurrentAttackingState : MonoBehaviour // set attackers currenttarget to null when you destroy the gameobject in attack script
{
    public enum currentState { attackingHero, attackingOther, notInCombat };
    public currentState state;
    public bool isAttacking;
    public bool isAttackingHero;
    public string currentlyAttacking;

    private LaneCreepCombat laneMelee;
    private RangedLaneCreepCombat laneRanged;
    private NeutralMeleeCombat neutralMelee;
    private NeutralRangedCombat neutralRanged;
    private HeroCombat heroCombat;
    private LaneCreepAI laneAI;
    private NeutralCreepAI neutralAi;
    //private HeroAI heroCombat;


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


        if (TryGetComponent<LaneCreepAI>(out LaneCreepAI laneAAA))
        {
            laneAI = laneAAA.GetComponent<LaneCreepAI>();
        }

        if (TryGetComponent<NeutralCreepAI>(out NeutralCreepAI neutralAAA))
        {
            neutralAi = neutralAAA.GetComponent<NeutralCreepAI>();
        }

        // add hero one when made
    }

    // set easily accessed state bools depending on current scripts enum value
    private void Update()
    {
        if (laneMelee != null && laneAI != null && laneAI.currentTarget != null)
        {
            currentlyAttacking = laneAI.currentTarget.name;

            if (laneMelee.state == LaneCreepCombat.currentState.attackingHero)
            {
                state = currentState.attackingHero;
                isAttacking = true;
                isAttackingHero = true;
            }
            else if (laneMelee.state == LaneCreepCombat.currentState.attackingOther)
            {
                state = currentState.attackingOther;
                isAttacking = true;
                isAttackingHero = false;
            }
            else
            {
                state = currentState.notInCombat;
                isAttacking = false;
                isAttackingHero = false;
            }
        }

        if (laneRanged != null && laneAI != null && laneAI.currentTarget != null)
        {
            currentlyAttacking = laneAI.currentTarget.name;

            if (laneRanged.state == RangedLaneCreepCombat.currentState.attackingHero)
            {
                state = currentState.attackingHero;
                isAttacking = true;
                isAttackingHero = true;
            }
            else if (laneRanged.state == RangedLaneCreepCombat.currentState.attackingOther)
            {
                state = currentState.attackingOther;
                isAttacking = true;
                isAttackingHero = false;
            }
            else
            {
                state = currentState.notInCombat;
                isAttacking = false;
                isAttackingHero = false;
            }
        }

        if (neutralMelee != null && neutralAi != null && neutralAi.currentTarget != null)
        {
            currentlyAttacking = neutralAi.currentTarget.name;

            if (neutralMelee.state == NeutralMeleeCombat.currentState.attackingHero)
            {
                state = currentState.attackingHero;
                isAttacking = true;
                isAttackingHero = true;
            }
            else if (neutralMelee.state == NeutralMeleeCombat.currentState.attackingOther)
            {
                state = currentState.attackingOther;
                isAttacking = true;
                isAttackingHero = false;
            }
            else
            {
                state = currentState.notInCombat;
                isAttacking = false;
                isAttackingHero = false;
            }
        }

        if (neutralRanged != null && neutralAi != null && neutralAi.currentTarget != null)
        {
            currentlyAttacking = neutralAi.currentTarget.name;

            if (neutralRanged.state == NeutralRangedCombat.currentState.attackingHero)
            {
                state = currentState.attackingHero;
                isAttacking = true;
                isAttackingHero = true;
            }
            else if (neutralRanged.state == NeutralRangedCombat.currentState.attackingOther)
            {
                state = currentState.attackingOther;
                isAttacking = true;
                isAttackingHero = false;
            }
            else
            {
                state = currentState.notInCombat;
                isAttacking = false;
                isAttackingHero = false;
            }
        }

        if (heroCombat != null /*&& heroAI != null && heroAI.currentTarget != null*/)
        {
            //currentlyAttacking = heroAI.currentTarget.name;

            if (heroCombat.state == HeroCombat.currentState.attackingHero)
            {
                state = currentState.attackingHero;
                isAttacking = true;
                isAttackingHero = true;
            }
            else if (heroCombat.state == HeroCombat.currentState.attackingOther)
            {
                state = currentState.attackingOther;
                isAttacking = true;
                isAttackingHero = false;
            }
            else
            {
                state = currentState.notInCombat;
                isAttacking = false;
                isAttackingHero = false;
            }
        }
    }
}
