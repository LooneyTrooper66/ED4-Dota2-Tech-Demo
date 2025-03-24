using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHeroStats", menuName = "Scriptable Objects/PlayerHeroStats")]
public class PlayerHeroStats : ScriptableObject
{
    [Header("Base Primary Stats")]
    public int health;
    public int mana;
    public float damage;
    public float movementSpeed;
    public float attackSpeed;

    [Header("Base Secondary Stats")]
    public int strength;
    public int agility;
    public int intelligence;
}
