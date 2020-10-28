using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  Check to see if hit is successful.              (hitChance * (1 + (accuracy-dodge)))
 *  Check to see if hit is critically successful.   (5% + critChance)
 *  Calculate gross damage.                         (attack + dmgBonus) if critical (2 + critDamageBonus)
 *  Calculate net damage.                           (grossDamage - (grossDamage * armorRating) - (grossDamage * magicResist))
 *  Return net damage.
 */
public class DamageHandler
{

    public static int DealDamage(int damage)
    {
        return -1;
    }

    public static int ProcessCombat(Unit attacker, Unit defender)
    {
        int netDamage = 0;

        float toHit = (attacker.HitChance - (attacker.HitChanceBonus - defender.DodgeChance));
        Debug.Log(toHit);
        if (Random.Range(0.0f, 1.0f) > toHit)
        {
            return 0;
        }

        if (Random.Range(0.0f, 1.0f) <= attacker.CritChance)
        {
            float criticalDamageBonus = 2 + attacker.CritDamageBonus;
            int grossDamage = (int)((attacker.Damage) * criticalDamageBonus);
            netDamage = (int)(grossDamage - (grossDamage * defender.ArmorRating) - (grossDamage * defender.MagicResistance));
        }
        else
        {
            int grossDamage = (attacker.Damage);
            netDamage = (int)(grossDamage - (grossDamage * defender.ArmorRating) - (grossDamage * defender.MagicResistance));
        }

        return netDamage;
    }

}
