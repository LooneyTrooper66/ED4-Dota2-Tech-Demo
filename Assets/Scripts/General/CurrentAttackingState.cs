using UnityEngine;

public class CurrentAttackingState : MonoBehaviour
{
    public enum currentState { attackingHero, attackingOther, notInCombat };
    public currentState state;
    public bool isAttacking;
    public bool isAttackingHero;
    public string currentlyAttacking;

    private LaneCreepCombat laneCombat;
    private NeutralCreepCombat neutralCombat;
    private HeroCombat heroCombat;
    private LaneCreepAI laneAI;
    private NeutralCreepAI neutralAi;
    //private HeroAI heroCombat;


    private void Start()
    {
        if (TryGetComponent<LaneCreepCombat>(out LaneCreepCombat laneCom))
        {
            laneCombat = laneCom.GetComponent<LaneCreepCombat>();
        }

        if (TryGetComponent<NeutralCreepCombat>(out NeutralCreepCombat neutralCom))
        { 
            neutralCombat = neutralCom.GetComponent<NeutralCreepCombat>();
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
        if (laneCombat != null && laneAI != null)
        {
            currentlyAttacking = laneAI.currentTarget.name;

            if (laneCombat.state == LaneCreepCombat.currentState.attackingHero)
            {
                isAttacking = true;
                isAttackingHero = true;
            }
            else if (laneCombat.state == LaneCreepCombat.currentState.attackingOther)
            {
                isAttacking = true;
                isAttackingHero = false;
            }
            else
            {
                isAttacking = false;
                isAttackingHero = false;
            }
        }

        if (neutralCombat != null && neutralAi != null)
        {
            currentlyAttacking = neutralAi.currentTarget.name;

            if (neutralCombat.state == NeutralCreepCombat.currentState.attackingHero)
            {
                isAttacking = true;
                isAttackingHero = true;
            }
            else if (neutralCombat.state == NeutralCreepCombat.currentState.attackingOther)
            {
                isAttacking = true;
                isAttackingHero = false;
            }
            else
            {
                isAttacking = false;
                isAttackingHero = false;
            }
        }

        if (heroCombat != null /*&& heroAI != null*/)
        {
            //currentlyAttacking = heroAI.currentTarget.name;

            if (heroCombat.state == HeroCombat.currentState.attackingHero)
            {
                isAttacking = true;
                isAttackingHero = true;
            }
            else if (heroCombat.state == HeroCombat.currentState.attackingOther)
            {
                isAttacking = true;
                isAttackingHero = false;
            }
            else
            {
                isAttacking = false;
                isAttackingHero = false;
            }
        }
    }
}
