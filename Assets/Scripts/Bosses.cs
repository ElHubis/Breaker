using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bosses : MonoBehaviour
{
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

    public bool RadamanTakeDamage(int damage)
    {
        RadamanCurrentHP -= damage;

        if (RadamanCurrentHP <= 0)
            return true;
        else
            return false;
    }

    public void RadamanHealing()
    {
        RadamanCurrentHP += RadamanHeal;
    }

    public bool MoodTakeDamage(int damage)
    {
        MoodCurrentHP -= damage;

        if (MoodCurrentHP <= 0) 
            return true; 
        else
            return false;
    }

    public void MoodHealing()
    {
        MoodCurrentHP += MoodHeal;
    }
    
}