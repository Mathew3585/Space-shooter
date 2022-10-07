using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public int money;

    void Update()
    {
        moneyText.text = "" + money;    
    }
}
