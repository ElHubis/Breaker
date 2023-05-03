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

    public void SetHP(PlayerBattle player)
    {
        HPNumber.text = player.PlayerCurrentHP.ToString() + "/" + player.PlayerMaxHP.ToString();
        HPslider.maxValue = player.PlayerMaxHP;
        HPslider.value = player.PlayerCurrentHP;
    }

    public void UpdateHP(int HP, PlayerBattle player)
    {
        HPNumber.text = HP.ToString() + "/" + player.PlayerMaxHP.ToString();
        HPslider.value = HP;
    }

    public void SetActionText(Bosses Boss)
    {
        ActionText.text = " " + Boss.RadamanName + " wants to fight";
    }

}