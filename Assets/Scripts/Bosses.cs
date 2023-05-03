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

    public void RadamanStrongAttack()
    {

    }
}