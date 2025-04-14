using UnityEngine;

public class CurrentAttackingState : MonoBehaviour // set attackers currenttarget to null when you destroy the gameobject in attack script
{
    public enum currentState { attackingHero, attackingOther, notInCombat };
    public currentState state = currentState.notInCombat;
    public bool isAttacking;
    public bool isAttackingHero;
    public string currentlyAttacking;

    private LaneCreepCombat laneMelee;
    private RangedLaneCreepCombat laneRanged;
    private SiegeCombat siegeCombat;
    private NeutralMeleeCombat neutralMelee;
    private NeutralRangedCombat neutralRanged;
    private HeroCombat heroCombat;
    private Movement heroMovement;
    private LaneCreepAI laneAI;
    private SiegeCreepTargetingAI siegeAi;
    private NeutralCreepAI neutralAi;


    // i am genuinely stupid i shouldve just set this shit in the scripts ive had to reference but im too tired and lazy to fix that rn so suffer with me scott at least i figured out a better way to do it ;-;
    // i also potentially couldve used interfaces for this, but idk if you can make a return class or not?
    // meh probably wouldnt need one anyway

    void Start()
    {
        if (TryGetComponent<LaneCreepCombat>(out LaneCreepCombat laneCom))
        {
            laneMelee = laneCom.GetComponent<LaneCreepCombat>();
            laneAI = GetComponent<LaneCreepAI>();
        }

        if (TryGetComponent<RangedLaneCreepCombat>(out RangedLaneCreepCombat laneRCom))
        {
            laneRanged = laneRCom.GetComponent<RangedLaneCreepCombat>();
            laneAI = GetComponent<LaneCreepAI>();
        }

        if (TryGetComponent<SiegeCombat>(out SiegeCombat laneSCom))
        {
            siegeCombat = laneSCom.GetComponent<SiegeCombat>();
            siegeAi = GetComponent<SiegeCreepTargetingAI>();
        }

        if (TryGetComponent<NeutralMeleeCombat>(out NeutralMeleeCombat neutralCom))
        {
            neutralMelee = neutralCom.GetComponent<NeutralMeleeCombat>();
            neutralAi = GetComponent<NeutralCreepAI>();
        }

        if (TryGetComponent<NeutralRangedCombat>(out NeutralRangedCombat neutralRCom))
        {
            neutralRanged = neutralRCom.GetComponent<NeutralRangedCombat>();
            neutralAi = GetComponent<NeutralCreepAI>();
        }

        if (TryGetComponent<HeroCombat>(out HeroCombat heroCom))
        {
            heroCombat = heroCom.GetComponent<HeroCombat>();
            heroMovement = GetComponent<Movement>();
        }
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

        if (siegeCombat != null && siegeAi != null && siegeAi.currentTarget != null)
        {
            currentlyAttacking = siegeAi.currentTarget.name;

            if (siegeCombat.state == SiegeCombat.currentState.attackingHero)
            {
                state = currentState.attackingHero;
                isAttacking = true;
                isAttackingHero = true;
            }
            else if (siegeCombat.state == SiegeCombat.currentState.attackingOther)
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

        if (heroCombat != null && heroMovement.currentTarget != null)
        {
            currentlyAttacking = heroMovement.currentTarget.name;

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
