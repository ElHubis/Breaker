using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public TextMeshProUGUI ActionText;
    public TextMeshProUGUI HPNumber;
    public Slider HPslider;
    public TextMeshProUGUI MPNumber;
    public Slider MPslider;
 
    //Sätter HP-mätare i början av en fight
    public void SetHP(PlayerBattle player)
    {
        HPNumber.text = player.PlayerCurrentHP.ToString() + "/" + player.PlayerMaxHP.ToString();
        HPslider.maxValue = player.PlayerMaxHP;
        HPslider.value = player.PlayerCurrentHP;
    }
    
    //Sätter MP-mätare i början av en fight
    public void SetMP(PlayerBattle player)
    {
        MPNumber.text = player.PlayerCurrentMP.ToString() + "/" + player.PlayerMaxMP.ToString();
        MPslider.maxValue = player.PlayerMaxMP;
        MPslider.value = player.PlayerCurrentMP;
    }

    //Uppdaterar HP under fighten
    public void UpdateHP(int HP, PlayerBattle player)
    {
        HPNumber.text = HP.ToString() + "/" + player.PlayerMaxHP.ToString();
        HPslider.value = HP;
    }

    //Uppdaterar MP under fighten
    public void UpdateMP(int MP, PlayerBattle player)
    {
        MPNumber.text = MP.ToString() + "/" + player.PlayerMaxMP.ToString();
        MPslider.value = MP;
    }

    //Skriver ut den första textraden i början av en fight
    public void SetActionText()
    {
        ActionText.text = " " + BattleSystem.CurrentBoss + " wants to fight";
    }

}