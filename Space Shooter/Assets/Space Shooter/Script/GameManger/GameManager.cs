using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    [Header("Menu or Game")]
    public bool Menu;
    public bool Game;
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

    [Header("Gun")]
    public bool Gunup1;
    public bool Gunup2;
    public bool Gunup3;
    [Header("Gun GameObject")]
    public GameObject Gun1;
    public GameObject Gun2;
    public GameObject Gun3;
    [Header("Price Value")]
    public float PriceGunUp2;
    public float PriceGunUp3;
    [Header("Price Text")]
    public TextMeshProUGUI PriceGunUp2Text;
    public TextMeshProUGUI PriceGunUp3Text;
    [Header("Button Upgradde")]
    public Button PriceGunUp2Button;
    public Button PriceGunUp3Button;

    public void Awake()
    {
        Gunup1 = true;
        if (Menu == true)
        {
            if (Gunup1 == true)
            {
                Gun1.SetActive(true);
                Gun2.SetActive(false);
                Gun3.SetActive(false);
            }
            PriceGunUp2Text.text = PriceGunUp2.ToString();
            PriceGunUp3Text.text = PriceGunUp3.ToString();

            Debug.Log("Menu Activer");
        }
        if(Game == true)
        {
            ship_Controller = GameObject.FindObjectOfType<Ship_Controller>();
            UILose.SetActive(false);
            UIInGame.SetActive(true);
            ProgessSlider = gameObject.GetComponent<ProgerssionSlider>();
            asteroid_ = GameObject.FindObjectOfType<Asteroid_Field>();
            Debug.Log("Game Activer");
        }

    }
    void Update()
    {
        if (Menu == true)
        {
            if (Gunup2 == true)
            {
                Gun1.SetActive(true);
                Gun2.SetActive(true);
            }

            if (Gunup3 == true)
            {
                Gun1.SetActive(true);
                Gun2.SetActive(true);
                Gun3.SetActive(true);
            }
        }

        if(Game == true)
        {
            moneyText.text = "" + money;
            //PlayerPrefs.SetInt("SpaceShootherMoney", money);

            if (ship_Controller.stats.CurrentHealth <= 0)
            {
                UILose.SetActive(true);
                UIInGame.SetActive(false);
            }
            if (ProgessSlider.Progress >= 70)
            {
                asteroid_.enabled = false;
                Debug.Log("Boss fight");
            }
        }

    }

    public void OnClickUpagrade2()
    {
        Gunup2 = true;
        PriceGunUp2Button.enabled = false;
    }
    public void OnClickUpagrade3()
    {
        if(Gunup3 == true)
        {
            PriceGunUp2Button.enabled = false;
        }
        Gunup3 = true;
        PriceGunUp3Button.enabled = false;
    }


}
