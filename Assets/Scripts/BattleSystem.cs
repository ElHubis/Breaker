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

    public Transform PlayerSpawn;
    public Transform EnemySpawn;

    public BattleState State;

    public bool PlayerDead;

    public BattleUI UI;
 
    PlayerBattle Player;
    Bosses Boss;

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

        GameObject RadamanObject = Instantiate(RadamanPrefab, EnemySpawn);
        Boss = RadamanObject.GetComponent<Bosses>();

        UI.SetHP(Player);

        UI.SetActionText(Boss);

        State = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        bool RadamanDead = Boss.RadamanTakeDamage(Player.PlayerDamage);

        UI.ActionText.text = " You Attacked " + Boss.RadamanName;

        yield return new WaitForSeconds(2f);

        if (RadamanDead)
        {
            State = BattleState.WIN;

            UI.ActionText.text = " You Defeated " + Boss.RadamanName;

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

    IEnumerator PlayerFireBlast()
    {
        bool RadamanDead = Boss.RadamanTakeDamage(Player.PlayerDamage*2);

        UI.ActionText.text = " You Used Fire Blast ";

        yield return new WaitForSeconds(2f);

        if (RadamanDead)
        {
            State = BattleState.WIN;

            UI.ActionText.text = " You Defeated " + Boss.RadamanName;

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
        Player.PlayerHealDamage();

        UI.UpdateHP(Player.PlayerCurrentHP, Player);

        UI.ActionText.text = " You Healed";

        yield return new WaitForSeconds(2f);

        State = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        UI.ActionText.text = Boss.RadamanName + " Is Acting ";

        yield return new WaitForSeconds(1f);

        if (Boss.RadamanCurrentHP <= Boss.RadamanMaxHP / 5)
        {
            Boss.RadamanHealing();

            UI.ActionText.text = "Radaman Healed";

            yield return new WaitForSeconds(1f);
        }

        else if (Player.PlayerCurrentHP > Player.PlayerMaxHP / 1.5)
        {
            Player.PlayerTakeDamage(Boss.RadamanDamage * 2);

            UI.ActionText.text = Boss.RadamanName + " Used Super Slash";

            yield return new WaitForSeconds(1f);
        }

        else
        {
            Player.PlayerTakeDamage(Boss.RadamanDamage);

            UI.ActionText.text = Boss.RadamanName + " Attacked";

            yield return new WaitForSeconds(1f);
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

            Player.PlayerCurrentHP = Player.PlayerMaxHP;
        }
        else
            State = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        UI.ActionText.text = " Choose Your Action";
    }

    public void AttackButton()
    {
        if (State != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void FireBlastButton()
    {
        if (State != BattleState.PLAYERTURN)
            return;
    
        StartCoroutine(PlayerFireBlast());
    }

    public void HealButton()
    {
        if (State != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerHeal());
    }
}