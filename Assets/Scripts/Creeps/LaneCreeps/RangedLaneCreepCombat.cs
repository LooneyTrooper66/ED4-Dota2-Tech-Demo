using UnityEngine;

public class RangedLaneCreepCombat : MonoBehaviour
{
    private float attackRange = 1f;
    private float attackDamage = 20;
    public float attackInterval;
    private float lastAttackTime;

    public enum currentState { attackingHero, attackingOther, travelling };
    public currentState state = currentState.travelling;
    private LaneCreepAI laneAI;
    private Stats stats;


    private void Start()
    {
        stats = GetComponent<Stats>();
        laneAI = GetComponent<LaneCreepAI>();

        state = currentState.travelling;
        attackRange = stats.attackRange;
        attackDamage = stats.damage;
    }
}
