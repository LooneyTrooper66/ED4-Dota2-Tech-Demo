using UnityEngine;

public class HeroCombat : MonoBehaviour
{    public enum currentState { attackingHero, attackingOther, travelling };
    public currentState state = currentState.travelling;
}
