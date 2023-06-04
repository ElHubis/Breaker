using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleState{ STARTBATTLE, PLAYERTURN, ENEMYTURN, WIN, LOSE }

public class BattleSystem : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject RadamanPrefab;
    public GameObject MoodPrefab;

    public Transform PlayerSpawn;
    public Transform EnemySpawn;

    public static string CurrentBoss = "";

    public BattleState State;

    public bool PlayerDead;
    public bool RadamanDead;
    public bool MoodDead;

    public BattleUI UI;
 
    PlayerBattle Player;
    Bosses Boss;

    //De delar av koden som är bort kommenterade är mitt försök att lägga till en extra boss men det fungerade inte riktigt

    // Start is called before the first frame update
    void Start()
    {
        State = BattleState.STARTBATTLE;
        StartBattle();
    }

    void StartBattle()
    {
        GameObject PlayerObject = Instantiate(PlayerPrefab, PlayerSpawn);
        Player = PlayerObject.GetComponent<PlayerBattle>();

        //if (CurrentBoss == "Radaman") 
        //{
            GameObject BossObject = Instantiate(RadamanPrefab, EnemySpawn);
            Boss = BossObject.GetComponent<Bosses>();
        //}

        //else
        //{
        //    GameObject BossObject = Instantiate(MoodPrefab, EnemySpawn);
        //    Boss = BossObject.GetComponent<Bosses>();   
        //}

        UI.SetHP(Player);

        UI.SetMP(Player);

        UI.SetActionText();

        State = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        Player.PlayerCurrentMP += 5;

        if (Player.PlayerCurrentMP > Player.PlayerMaxMP)
        {
            Player.PlayerCurrentMP = Player.PlayerMaxMP;
        } 
        
        UI.UpdateMP(Player.PlayerCurrentMP, Player);

        bool RadamanDead = Boss.RadamanTakeDamage(Player.PlayerDamage);
        //bool MoodDead = Boss.MoodTakeDamage(Player.PlayerDamage);
        UI.ActionText.text = " You Attacked " + CurrentBoss;
        
        yield return new WaitForSeconds(2f);

        if ( RadamanDead == true) //|| MoodDead == true)
        {
            State = BattleState.WIN;

            UI.ActionText.text = " You Defeated " + CurrentBoss;

            yield return new WaitForSeconds(2f);

            SceneManager.LoadScene("OverWorld");

            Player.PlayerCurrentHP = Player.PlayerMaxHP;
            Player.PlayerCurrentMP = Player.PlayerMaxMP;
        } 
        else
        {
            State = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerDefend()
    {
        Player.PlayerCurrentMP += 10;

        if (Player.PlayerCurrentMP > Player.PlayerMaxMP)
        {
            Player.PlayerCurrentMP = Player.PlayerMaxMP;
        }

        UI.UpdateMP(Player.PlayerCurrentMP, Player);

        Player.PlayerActiveDefence = Player.PlayerDefence * 2;

        UI.ActionText.text = " You defend ";

        yield return new WaitForSeconds(2f);

        State = BattleState.ENEMYTURN;
        StartCoroutine (EnemyTurn());
    }

    IEnumerator PlayerFireBlast()
    {
        Player.PlayerCurrentMP -= Player.FireBlastCost;

        UI.UpdateMP(Player.PlayerCurrentMP, Player);

        bool RadamanDead = Boss.RadamanTakeDamage(Player.PlayerDamage * 2);
        //bool MoodDead = Boss.MoodTakeDamage(Player.PlayerDamage * 2);

        UI.ActionText.text = " You Used Fire Blast ";

        yield return new WaitForSeconds(2f);

        if (RadamanDead == true )//|| MoodDead == true)
        {
            State = BattleState.WIN;

            UI.ActionText.text = " You Defeated " + CurrentBoss;

            yield return new WaitForSeconds(2f);

            SceneManager.LoadScene("OverWorld");

            Player.PlayerCurrentHP = Player.PlayerMaxHP;
        }
        else
        {
            State = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerHeal()
    {
        Player.PlayerCurrentMP -= Player.HealCost;

        UI.UpdateMP(Player.PlayerCurrentMP, Player);

        Player.PlayerHealDamage();

        UI.UpdateHP(Player.PlayerCurrentHP, Player);

        UI.ActionText.text = " You Healed";

        yield return new WaitForSeconds(2f);

        State = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        UI.ActionText.text = CurrentBoss + " Is Acting ";

        yield return new WaitForSeconds(1f);

        //if (CurrentBoss == "Radaman")
        //{
            if (Boss.RadamanCurrentHP <= Boss.RadamanMaxHP / 5)
            {
                Boss.RadamanHealing();

                UI.ActionText.text = CurrentBoss + " Healed";

                yield return new WaitForSeconds(1f);
            }

            else if (Player.PlayerCurrentHP > Player.PlayerMaxHP / 1.5)
            {
                Player.PlayerTakeDamage(Boss.RadamanDamage * 2 / Player.PlayerActiveDefence);

                UI.ActionText.text = Boss.RadamanName + " Used Super Slash";

                yield return new WaitForSeconds(1f);
            }

            else
            {
                Player.PlayerTakeDamage(Boss.RadamanDamage / Player.PlayerActiveDefence);

                UI.ActionText.text = CurrentBoss + "Attacked";

                yield return new WaitForSeconds(1f);
            }
        //}

        //if (CurrentBoss == "Mood")
        //{
        //    if (Boss.MoodCurrentHP <= Boss.MoodMaxHP / 5)
        //    {
        //        Boss.MoodHealing();

        //        UI.ActionText.text = CurrentBoss + "Healed";

        //        yield return new WaitForSeconds(1f);
        //    }

        //    else if (Player.PlayerCurrentHP > Player.PlayerMaxHP / 1.5)
        //    {
        //        Player.PlayerTakeDamage(Boss.MoodDamage * 2 / Player.PlayerActiveDefence);

        //        UI.ActionText.text = CurrentBoss + " Used Grass Slash";

        //        yield return new WaitForSeconds(1f);
        //    }

        //    else
        //    {
        //        Player.PlayerTakeDamage(Boss.MoodDamage / Player.PlayerActiveDefence);

        //        UI.ActionText.text = CurrentBoss + "Attacked";

        //        yield return new WaitForSeconds(1f);
        //    }
        //}


        UI.UpdateHP(Player.PlayerCurrentHP, Player);

        if (Player.PlayerCurrentHP <= 0)
        {
            PlayerDead = true;
        }

        else
        {
            PlayerDead = false;
        }

        if (PlayerDead)
        {
            State = BattleState.LOSE;

            UI.ActionText.text = " You Lost ";

            yield return new WaitForSeconds(2f);

            SceneManager.LoadScene("OverWorld");

            CurrentBoss = "";

            Player.PlayerCurrentHP = Player.PlayerMaxHP;

        }
        else
            State = BattleState.PLAYERTURN;
            PlayerTurn();
    }

    void PlayerTurn()
    {
        Player.PlayerActiveDefence = Player.PlayerDefence;

        UI.ActionText.text = " Choose Your Action";
    }

    public void AttackButton()
    {
        if (State != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void DefendButton()
    {
        if (State != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerDefend());
    }

    public void FireBlastButton()
    {
        if (State != BattleState.PLAYERTURN || Player.PlayerCurrentMP < Player.FireBlastCost )
            return;
    
        StartCoroutine(PlayerFireBlast());
    }

    public void HealButton()
    {
        if (State != BattleState.PLAYERTURN || Player.PlayerCurrentMP < Player.HealCost)
            return;

        StartCoroutine(PlayerHeal());
    }
}