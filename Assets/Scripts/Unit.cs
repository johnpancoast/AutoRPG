using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private string unitName;
    private int damage;
    private int hitChanceBonus;     //Bonus to default hit chance
    private float hitChance;        //Percentage chance to hit target
    private float dodgeChance;      //Percentage to dodge attacks
    private float critChance;       //Percentage change to score a critical hit
    private float critDamageBonus;  //Damage bonus for critical damage
    private float armorRating;      //Percentage damage reduction
    private float magicResistance;  //Percentage magical damage reduction

    public string UnitName { get => unitName; set => unitName = value; }
    public int Damage { get => damage; set => damage = value; }
    public int HitChanceBonus { get => hitChanceBonus; set => hitChanceBonus = value; }
    public float HitChance { get => hitChance; set => hitChance = value; }
    public float DodgeChance { get => dodgeChance; set => dodgeChance = value; }
    public float CritChance { get => critChance; set => critChance = value; }
    public float CritDamageBonus { get => critDamageBonus; set => critDamageBonus = value; }
    public float ArmorRating { get => armorRating; set => armorRating = value; }
    public float MagicResistance { get => magicResistance; set => magicResistance = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool TakeDamage(int damage)
    {
        return true;
    }
}
