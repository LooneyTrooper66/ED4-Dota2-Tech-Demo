using UnityEngine;

public class HeroStats : MonoBehaviour
{
    public float health;
    public float mana;
    public float damage;
    public float attackSpeed;


    public void DealDamage(GameObject target, float damage)
    {
        target.GetComponent<Stats>().health -= damage;
        if (target.GetComponent<Stats>().health <= 0)
        {
            // use switch statement to determine how much xp to gain, award gold for last hit if death caused by this scripts function
            // if die to the 'Stats' script damage function, only award xp
            
            /*string tag = target.tag;
            switch (tag)
            {
                case "EnemyCreep":
                    Debug.Log("y amount of xp, if lh x amount of gold");
                    return;
            }*/
        }
    }
}
