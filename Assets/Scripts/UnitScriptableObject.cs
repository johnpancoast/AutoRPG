using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "ScriptableObjects/UnitScriptableObject", order = 1)]
public class UnitScriptableObject : ScriptableObject
{
    public string unitName;
    public int damage;
    public int health;
    public int speed;
    public float hitChanceBonus;
    public float hitChance;
    public float dodgeChance;
    public float critChance;
    public float critDamageBonus;
    public float armorRating;
    public float magicResistance;
    public Sprite art;
}
