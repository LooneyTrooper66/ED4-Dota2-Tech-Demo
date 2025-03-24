using UnityEngine;

public class HeroStats : MonoBehaviour
{
    public PlayerHeroStats playerStats;  // if adding multiple characters, pass this from the character selection screen so gets correct hero stats

    [Header("Primary Stats")]
    public int level;
    public int health;
    public int mana;
    public float damage;
    public float movementSpeed;
    public float attackSpeed;

    [Header("Secondary Stats")]
    public int strength;
    public int agility;
    public int intelligence;

    [Header("Resorces")]
    public int gold;
    public int xp;


    private void Start()
    {
        #region // set all base stats for game start
        level = 0;
        health = playerStats.health;
        mana = playerStats.mana;
        damage = playerStats.damage;
        movementSpeed = playerStats.movementSpeed;
        attackSpeed = playerStats.attackSpeed;
        strength = playerStats.strength;
        agility = playerStats.agility;
        intelligence = playerStats.intelligence;
        gold = 0;
        xp = 0;
        #endregion
    }

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


            //meow meowmeow meow meow meow meow meow
            }*/
        }
    }
}
