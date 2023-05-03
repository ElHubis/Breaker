using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    public string Name = "Heyn";
    public int PlayerDamage = 15;
    public int PlayerHeal = 15;
    public int PlayerDefence = 1;
    public int PlayerMaxHP = 70;
    public int PlayerCurrentHP = 70;

    public void PlayerTakeDamage(int damage)
    {
        PlayerCurrentHP -= damage;
    }

    public void PlayerHealDamage()
    {
        PlayerCurrentHP += PlayerHeal;
    }

}