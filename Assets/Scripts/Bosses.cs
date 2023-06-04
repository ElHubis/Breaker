using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bosses : MonoBehaviour
{
    //Bossarnas stats
    public string RadamanName = "Radaman";
    public int RadamanDamage = 10;
    public int RadamanHeal = 15;
    public int RadamanMaxHP = 80;
    public int RadamanCurrentHP = 80;

    public string MoodName = "Mood";
    public int MoodDamage = 20;
    public int MoodHeal = 10;
    public int MoodMaxHP = 100;
    public int MoodCurrentHP = 100;

    //Funktioner för att bossarna ska ta skada och ska kunna heala
    public void RadamanTakeDamage(int damage)
    {
        RadamanCurrentHP -= damage;
    }

    public void RadamanHealing()
    {
        RadamanCurrentHP += RadamanHeal;
    }

    public void MoodTakeDamage(int damage)
    {
        MoodCurrentHP -= damage;
    }

    public void MoodHealing()
    {
        MoodCurrentHP += MoodHeal;
    }
    
}