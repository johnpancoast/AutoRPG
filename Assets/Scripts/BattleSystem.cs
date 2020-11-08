using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public enum BattleState { START, COMBAT, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;

    public BattleState state;

    public List<Unit> playerUnits = new List<Unit>();
    public List<Unit> enemyUnits = new List<Unit>();
    public List<Unit> speedList = new List<Unit>();

    public GameObject endTurnButton;

    static System.Random rnd = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        SortBySpeed();
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        yield return new WaitForSeconds(2f);

        state = BattleState.COMBAT;
        StartCoroutine(ProcessCombat());
    }

    IEnumerator ProcessCombat()
    {
        Debug.Log("Processing Combat");
        yield return new WaitForSeconds(2f);

        while (state == BattleState.COMBAT)
        {
            yield return new WaitForSeconds(.2f);
            for (int i = 0; i < speedList.Count; i++)
            {
                yield return new WaitForSeconds(1f);
                if (speedList[i] != null)
                {
                    if (playerUnits.Contains(speedList[i]) && enemyUnits.Count > 0)
                    {
                        int defenderIndex = rnd.Next(enemyUnits.Count - 1);
                        Unit attacker = speedList[i];
                        Unit defender = enemyUnits[defenderIndex];
                        int damage = DamageHandler.ProcessCombat(attacker, defender);
                        bool isDead = defender.TakeDamage(damage);

                        if (isDead)
                        {
                            enemyUnits.Remove(enemyUnits[defenderIndex]);

                            if (enemyUnits.Count == 0)
                            {
                                state = BattleState.WON;
                                EndBattle();
                            }
                        }
                    }
                    else if (enemyUnits.Contains(speedList[i]) && playerUnits.Count > 0)
                    {
                        int defenderIndex = rnd.Next(playerUnits.Count - 1);
                        Unit attacker = speedList[i];
                        Unit defender = playerUnits[defenderIndex];
                        int damage = DamageHandler.ProcessCombat(attacker, defender);
                        bool isDead = defender.TakeDamage(damage);

                        if (isDead)
                        {
                            playerUnits.Remove(playerUnits[defenderIndex]);

                            if (playerUnits.Count == 0)
                            {
                                state = BattleState.LOST;
                                EndBattle();
                            }
                        }
                    }
                }
            }
        }
    }

    List<Unit> SortBySpeed()
    {
        speedList = new List<Unit>();
        for (int i = 0; i < playerUnits.Count; i++)
        {
            Unit unit = playerUnits[i];
            if (unit.speed == Unit.Speed.FAST)
            {
                speedList.Add(unit);
            }
        }

        for (int i = 0; i < enemyUnits.Count; i++)
        {
            Unit unit = enemyUnits[i];
            if (unit.speed == Unit.Speed.FAST)
            {
                speedList.Add(unit);
            }
        }

        for (int i = 0; i < playerUnits.Count; i++)
        {
            Unit unit = playerUnits[i];
            if (unit.speed == Unit.Speed.NORMAL)
            {
                speedList.Add(unit);
            }
        }

        for (int i = 0; i < enemyUnits.Count; i++)
        {
            Unit unit = enemyUnits[i];
            if (unit.speed == Unit.Speed.NORMAL)
            {
                speedList.Add(unit);
            }
        }

        for (int i = 0; i < playerUnits.Count; i++)
        {
            Unit unit = playerUnits[i];
            if (unit.speed == Unit.Speed.SLOW)
            {
                speedList.Add(unit);
            }
        }

        for (int i = 0; i < enemyUnits.Count; i++)
        {
            Unit unit = enemyUnits[i];
            if (unit.speed == Unit.Speed.SLOW)
            {
                speedList.Add(unit);
            }
        }
        return speedList;
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated";
        }
    }

    public void ToggleEndOfTurnButton()
    {
        if (endTurnButton.activeSelf)
        {
            endTurnButton.SetActive(false);
        }
        else
        {
            endTurnButton.SetActive(true);
        }
    }
}
