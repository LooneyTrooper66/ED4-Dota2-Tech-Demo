using UnityEngine;

public class HeroCombat : MonoBehaviour
{
    public Transform currentTarget;
    public enum currentState { attackingHero, attackingOther, travelling };
    public currentState state = currentState.travelling;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // send raycast to find if attackable (have neutral aggro thing)
            // use layermask so cant hit player
            // set anything hit that has the scr to the current target
        }
    }
}
