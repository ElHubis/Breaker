using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    public string Name = "Heyn";
    public int PlayerDamage = 15;
    public int PlayerHeal = 15;
    public int PlayerDefence = 1;
    public int PlayerActiveDefence = 1;

    public int PlayerMaxHP = 70;
    public int PlayerCurrentHP = 70;

    public int PlayerMaxMP = 50;
    public int PlayerCurrentMP = 50;

    public int FireBlastCost = 20;
    public int HealCost = 15;

    public void PlayerTakeDamage(int damage)
    {
        PlayerCurrentHP -= damage;
    }

    public void PlayerHealDamage()
    {
        PlayerCurrentHP += PlayerHeal;
        if (PlayerCurrentHP > PlayerMaxHP)
        {
            PlayerCurrentHP = PlayerMaxHP;
        }
    }
}