using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHeroStats", menuName = "Scriptable Objects/PlayerHeroStats")]
public class PlayerHeroStats : ScriptableObject
{
    public int level;
    public int health;
    public int mana;
    public int strength;
    public int agility;
    public int intelligence;
    public float damage;
    public float movementSpeed;
    public float attackSpeed;
}
