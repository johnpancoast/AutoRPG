using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public enum BattleState { START, COMBAT, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    private float waitTime = 0.3f;
    private bool isAutoBattle = true;

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
        

        /*if (isAutoBattle)
        {
            state = BattleState.PLAYERTURN;
            StartCoroutine(ProcessPlayerTurn());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }*/
    }

    IEnumerator ProcessPlayerTurn()
    {
        
        if (state == BattleState.PLAYERTURN)
        {
            for (int i = 0; i < playerUnits.Count; i++)
            {
                if (enemyUnits.Count > 0)
                {
                    int attackTargetIndex = UnityEngine.Random.Range(0, enemyUnits.Count);

                    int damage = DamageHandler.ProcessCombat(playerUnits[i], enemyUnits[attackTargetIndex]);

                    dialogueText.text =
                        playerUnits[i].UnitName + " attacks " + enemyUnits[attackTargetIndex].UnitName + " for " + damage;

                    bool isDead = enemyUnits[attackTargetIndex].TakeDamage(damage);

                    yield return new WaitForSeconds(waitTime);

                    if (isDead)
                    {
                        enemyUnits.Remove(enemyUnits[attackTargetIndex]);

                        if (enemyUnits.Count == 0)
                        {
                            state = BattleState.WON;
                            EndBattle();
                            
                        }
                    }
                }
            }
        }

        yield return new WaitForSeconds(waitTime);

        state = BattleState.ENEMYTURN;
        StartCoroutine(ProcessEnemyTurn());
    }

    IEnumerator ProcessEnemyTurn()
    {
        if (state == BattleState.ENEMYTURN)
        {
            for (int i = 0; i < enemyUnits.Count; i++)
            {
                if (playerUnits.Count > 0)
                {
                    int attackTargetIndex = UnityEngine.Random.Range(0, playerUnits.Count);

                    int damageDealt = DamageHandler.DealDamage(enemyUnits[i].Damage);

                    dialogueText.text =
                        enemyUnits[i].UnitName + " attacks " + playerUnits[attackTargetIndex].UnitName + " for " + damageDealt;

                    bool isDead = playerUnits[attackTargetIndex].TakeDamage(damageDealt);

                    yield return new WaitForSeconds(waitTime);

                    if (isDead)
                    {
                        playerUnits.Remove(playerUnits[attackTargetIndex]);

                        if (playerUnits.Count == 0)
                        {
                            state = BattleState.LOST;
                            EndBattle();
                        }
                    }
                }
            }
        }

        yield return new WaitForSeconds(waitTime);

        if (isAutoBattle)
        {
            state = BattleState.PLAYERTURN;
            StartCoroutine(ProcessPlayerTurn());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            ToggleEndOfTurnButton();
            PlayerTurn();
        }
    }

    IEnumerator ProcessCombat()
    {
        Debug.Log("Processing Combat");
        yield return new WaitForSeconds(2f);

        while (state == BattleState.COMBAT)
        {
            yield return new WaitForSeconds(2f);
            for (int i = 0; i < speedList.Count; i++)
            {
                if (speedList[i] != null)
                {
                    Debug.Log("Error check: speedList null check");
                    if (playerUnits.Contains(speedList[i]))
                    {
                        int defenderIndex = rnd.Next(enemyUnits.Count - 1);
                        Unit defender = enemyUnits[defenderIndex];
                        int damage = DamageHandler.ProcessCombat(speedList[i], defender);
                        Debug.Log(speedList[i].UnitName + " deals " + damage);
                        bool isDead = defender.TakeDamage(damage);
                        Debug.Log("combat check");

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
                    else if (enemyUnits.Contains(speedList[i]))
                    {
                        int defenderIndex = rnd.Next(playerUnits.Count - 1);
                        Unit defender = playerUnits[defenderIndex];
                        int damage = DamageHandler.ProcessCombat(speedList[i], defender);
                        Debug.Log(speedList[i].UnitName +" deals " +damage);
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

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";
    }

    public void OnEndTurnButton()
    {
        if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(ProcessPlayerTurn());
            ToggleEndOfTurnButton();
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
