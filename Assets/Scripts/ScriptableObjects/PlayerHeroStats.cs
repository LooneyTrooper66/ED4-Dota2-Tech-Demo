using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHeroStats", menuName = "Scriptable Objects/PlayerHeroStats")]
public class PlayerHeroStats : ScriptableObject
{
    // all stats here are base stats and will change as the hero levels up, will be changed in hero stats script, not this object
    [Header("Primary Stats")]
    public float level;
    public float health;
    public float healthRegen;
    public float mana;
    public float manaRegen;
    public float damage;
    public float movementSpeed;
    public float secondsBetweenAttacks;
    public float attackRange;
    public float acquisitionRange;

    [Header("Secondary Stats")]
    public float strength;
    public float agility;
    public float intelligence;
    public int visionDay;
    public int visionNight;

    [Header("Resorces")]
    public int reliableGold;
    public int unreliableGold;
    public int xp;
}
