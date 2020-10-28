using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Unit : MonoBehaviour
{
    public UnitScriptableObject UnitValues;
    public TextMeshPro CardNameText;
    public GameObject CardArt;
    public TextMeshPro HealthText;

    private string unitName;
    private int damage;
    private int health;
    public enum Speed { FAST, NORMAL, SLOW};
    private float hitChanceBonus;     
    private float hitChance;        
    private float dodgeChance;      
    private float critChance;       
    private float critDamageBonus;  
    private float armorRating;      
    private float magicResistance;
    private Sprite art;
    public Speed speed = Speed.NORMAL;

    public string UnitName { get => unitName; set => unitName = value; }
    public int Damage { get => damage; set => damage = value; }
    public int Health { get => health; set => health = value; }
    public float HitChanceBonus { get => hitChanceBonus; set => hitChanceBonus = value; }
    public float HitChance { get => hitChance; set => hitChance = value; }
    public float DodgeChance { get => dodgeChance; set => dodgeChance = value; }
    public float CritChance { get => critChance; set => critChance = value; }
    public float CritDamageBonus { get => critDamageBonus; set => critDamageBonus = value; }
    public float ArmorRating { get => armorRating; set => armorRating = value; }
    public float MagicResistance { get => magicResistance; set => magicResistance = value; }
    public Sprite Art { get => art; set => art = value; }


    // Start is called before the first frame update
    void Start()
    {
        unitName = UnitValues.unitName;
        CardNameText.text = unitName;
        damage = UnitValues.damage;
        health = UnitValues.health;
        SetSpeed();
        hitChanceBonus = UnitValues.hitChanceBonus;
        hitChance = UnitValues.hitChance;
        dodgeChance = UnitValues.dodgeChance;
        critChance = UnitValues.critChance;
        critDamageBonus = UnitValues.critDamageBonus;
        armorRating = UnitValues.armorRating;
        magicResistance = UnitValues.magicResistance;
        art = UnitValues.art;
        SpriteRenderer spriteRenderer = CardArt.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = art;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            HealthText.text = "Health: 0";
            return true;
        }
        else
        {
            HealthText.text = "Health: " + Health.ToString();
            return false;
        }
    }

    private void SetSpeed()
    {
        if (UnitValues.speed == 1)
        {
            speed = Speed.FAST;
        }
        else if (UnitValues.speed == -1)
        {
            speed = Speed.SLOW;
        }
        else
        {
            speed = Speed.NORMAL;
        }
    }
}
