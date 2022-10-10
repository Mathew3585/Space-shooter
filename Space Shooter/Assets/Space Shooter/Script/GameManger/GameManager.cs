using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    [Header("Value")]
    public int money;

    [Header("Text")]
    public TextMeshProUGUI moneyText;

    [Header("GameObject")]
    public GameObject UIInGame;
    public GameObject UILose;

    [Header("Script")][Tooltip("Il trouve le player tout seul")]
    public Ship_Controller ship_Controller;
    [Tooltip("Il trouve le script tout seul")]
    public ProgerssionSlider ProgessSlider;

    public Asteroid_Field asteroid_;

    public void Awake()
    {
        ship_Controller = GameObject.FindObjectOfType<Ship_Controller>();
        UILose.SetActive(false);
        UIInGame.SetActive(true);
        ProgessSlider = gameObject.GetComponent<ProgerssionSlider>();
        asteroid_ = GameObject.FindObjectOfType<Asteroid_Field>();
    }
    void Update()
    {
        moneyText.text = "" + money;
        //PlayerPrefs.SetInt("SpaceShootherMoney", money);

        if(ship_Controller.stats.CurrentHealth <= 0)
        {
            UILose.SetActive(true);
            UIInGame.SetActive(false);
        }
        if(ProgessSlider.Progress >= 70)
        {
            asteroid_.enabled = false;
            Debug.Log("Boss fight");
        }
    }



    
}
