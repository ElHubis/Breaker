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
    //Prefabs som ska laddas in n�r det blir fight
    public GameObject PlayerPrefab;
    public GameObject RadamanPrefab;
    public GameObject MoodPrefab;

    //Positioner f�r vart prefabs ska laddas in
    public Transform PlayerSpawn;
    public Transform EnemySpawn;

    //Variabel f�r vilken boss som �r aktiv
    public static string CurrentBoss = "";

    //F�r att best�mma vilken stadie fighten �r i
    public BattleState State;

    //F�r att kolla om bossen eller spelaren d�tt
    public bool PlayerDead;
    public bool RadamanDead;
    public bool MoodDead;

    //Importerar UI kod
    public BattleUI UI;
    
    //Importerar kod f�r spelar karakt�r och bossar
    PlayerBattle Player;
    Bosses Boss;


    // Start is called before the first frame update
    //S�tter ig�ng fighten
    void Start()
    {
        State = BattleState.STARTBATTLE;
        StartBattle();
    }

    //Laddar in prefabs och st�ller in HP och MP f�r spelaren samt skriver starttexten
    void StartBattle()
    {
        GameObject PlayerObject = Instantiate(PlayerPrefab, PlayerSpawn);
        Player = PlayerObject.GetComponent<PlayerBattle>();

        if (CurrentBoss == "Radaman") 
        {
            GameObject BossObject = Instantiate(RadamanPrefab, EnemySpawn);
            Boss = BossObject.GetComponent<Bosses>();
        }

        else
        {
            GameObject BossObject = Instantiate(MoodPrefab, EnemySpawn);
            Boss = BossObject.GetComponent<Bosses>();   
        }

        UI.SetHP(Player);

        UI.SetMP(Player);

        UI.SetActionText();

        State = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    //Funktion f�r spelarens attack
    IEnumerator PlayerAttack()
    {
        Player.PlayerCurrentMP += 5;

        if (Player.PlayerCurrentMP > Player.PlayerMaxMP)
        {
            Player.PlayerCurrentMP = Player.PlayerMaxMP;
        } 
        
        UI.UpdateMP(Player.PlayerCurrentMP, Player);

        if (CurrentBoss == "Radaman")
        {
            Boss.RadamanTakeDamage(Player.PlayerDamage);
        }

        else
        {
            Boss.MoodTakeDamage(Player.PlayerDamage);
        }

        UI.ActionText.text = " You Attacked " + CurrentBoss;
        
        yield return new WaitForSeconds(2f);

        if (Boss.RadamanCurrentHP <= 0)
        {
            RadamanDead = true;
        }

        if (Boss.MoodCurrentHP <= 0)
        {
            MoodDead = true;
        }

        if ( RadamanDead == true || MoodDead == true)
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

    //Funktion f�r n�r spelaren trycker p� defend
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

    //Funktion f�r spelarens extra stark� attack
    IEnumerator PlayerFireBlast()
    {
        Player.PlayerCurrentMP -= Player.FireBlastCost;

        UI.UpdateMP(Player.PlayerCurrentMP, Player);

        if (CurrentBoss == "Radaman")
        {
            Boss.RadamanTakeDamage(Player.PlayerDamage * 2);
        }

        else
        {
            Boss.MoodTakeDamage(Player.PlayerDamage * 2);
        }

        UI.ActionText.text = " You Used Fire Blast" ;

        yield return new WaitForSeconds(2f);

        if (Boss.RadamanCurrentHP <= 0)
        {
            RadamanDead = true;
        }

        if (Boss.MoodCurrentHP <= 0)
        {
            MoodDead = true;
        }

        if (RadamanDead == true || MoodDead == true)
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

    //Funktion f�r att spelaren ska heala
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

    //Funktion som kontrollerar findernas beteende
    IEnumerator EnemyTurn()
    {
        UI.ActionText.text = CurrentBoss + " Is Acting ";

        yield return new WaitForSeconds(1f);

        if (CurrentBoss == "Radaman")
        {
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
        }

        if (CurrentBoss == "Mood")
        {
            if (Boss.MoodCurrentHP <= Boss.MoodMaxHP / 5)
            {
                Boss.MoodHealing();

                UI.ActionText.text = CurrentBoss + "Healed";

                yield return new WaitForSeconds(1f);
            }

            else if (Player.PlayerCurrentHP > Player.PlayerMaxHP / 1.5)
            {
                Player.PlayerTakeDamage(Boss.MoodDamage * 2 / Player.PlayerActiveDefence);

                UI.ActionText.text = CurrentBoss + " Used Grass Slash";

                yield return new WaitForSeconds(1f);
            }

            else
            {
                Player.PlayerTakeDamage(Boss.MoodDamage / Player.PlayerActiveDefence);

                UI.ActionText.text = CurrentBoss + "Attacked";

                yield return new WaitForSeconds(1f);
            }
        }


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

    //Kallas n�r det blir spelarens tur
    void PlayerTurn()
    {
        Player.PlayerActiveDefence = Player.PlayerDefence;

        UI.ActionText.text = " Choose Your Action";
    }

    //Kallas n�r spelaren trycker p� attack
    public void AttackButton()
    {
        if (State != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    //Kallas n�r spelaren trycker p� defend
    public void DefendButton()
    {
        if (State != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerDefend());
    }

    //Kallas n�r spelaren Fire Blast vilket �r den starkare attacken
    public void FireBlastButton()
    {
        if (State != BattleState.PLAYERTURN || Player.PlayerCurrentMP < Player.FireBlastCost )
            return;
    
        StartCoroutine(PlayerFireBlast());
    }

    //Kallas n�r spelaren trycker p� heal
    public void HealButton()
    {
        if (State != BattleState.PLAYERTURN || Player.PlayerCurrentMP < Player.HealCost)
            return;

        StartCoroutine(PlayerHeal());
    }
}