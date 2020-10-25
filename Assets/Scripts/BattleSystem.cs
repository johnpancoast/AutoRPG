using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    private float waitTime = 0.3f;
    private bool isAutoBattle = true;

    public TextMeshProUGUI dialogueText;

    public BattleState state;

    public List<Unit> playerUnits = new List<Unit>();
    public List<Unit> enemyUnits = new List<Unit>();

    public GameObject endTurnButton;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());

        if (isAutoBattle)
        {
            ToggleEndOfTurnButton();
        }
    }

    IEnumerator SetupBattle()
    {
        yield return new WaitForSeconds(2f);

        if (isAutoBattle)
        {
            state = BattleState.PLAYERTURN;
            StartCoroutine(ProcessPlayerTurn());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
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

    List<Unit> SortBySpeed()
    {
        List<Unit> speedList = new List<Unit>();
        speedList.AddRange(playerUnits);
        speedList.AddRange(enemyUnits);
        speedList.Sort(SortBySpeed);
        return speedList;
    }

    private int SortBySpeed(Unit x, Unit y)
    {
        return x.CompareTo(y);
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
