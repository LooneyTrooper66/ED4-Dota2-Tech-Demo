using UnityEngine;

[CreateAssetMenu(fileName = "CreepStatDefaults", menuName = "Scriptable Objects/CreepStatDefaults")]
public class CreepStatDefaults : ScriptableObject
{
    [Header("Base Stats")]
    public float health;
    public float healthRegen;
    public float mana;
    public float manaRegen;
    public float damage;
    public float secondsBetweenAttacks;
    public float attackRange;
    public float acquisitionRange;
    public int goldWorth;
    public int xpWorth;
    public int visionDay;
    public int visionNight;
}
