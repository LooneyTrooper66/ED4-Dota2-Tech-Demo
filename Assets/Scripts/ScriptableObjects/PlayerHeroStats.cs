using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHeroStats", menuName = "Scriptable Objects/PlayerHeroStats")]
public class PlayerHeroStats : ScriptableObject
{
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
}
