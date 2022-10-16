using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Menu or Game")]
    public bool Menu;
    public bool Game;

    [Header("Value")]
    public int money;
    private int CurrentMoney;
    [Header("Progresse Paramerter")]
    public float TimeSpeedDivide;
    public float Progress;
    public float ProgressHightScore;

    [Header("Text")]
    public TextMeshProUGUI moneyText;

    [Header("Player Is Alive?")]
    public bool IsDead;

    [Header("GameObject")]
    public GameObject UIInGame;
    public GameObject UILose;

    [Header("Script")][Tooltip("Il trouve le player tout seul")]
    public Ship_Controller ship_Controller;

    [Header("")]
    public Asteroid_Field asteroid_;

    [Header("Upagrade Gun")]
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
        money = PlayerPrefs.GetInt("Money");
        Gunup1 = true;

        //Si Menu est activer
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

        //Si Game est Activer
        if (Game == true)
        {
            ship_Controller = GameObject.FindObjectOfType<Ship_Controller>();
            UILose.SetActive(false);
            UIInGame.SetActive(true);
            CurrentMoney = PlayerPrefs.GetInt("Money", money);
            asteroid_ = GameObject.FindObjectOfType<Asteroid_Field>();
            Debug.Log("Game Activer");
        }

    }
    void Update()
    {
        //Si Menu est activer
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

        //Si Game est Activer
        if(Game == true)
        {
            
            ProgressHightScore = PlayerPrefs.GetFloat("Progress", Progress);
            if (IsDead == false)
            {
                Progress += Time.deltaTime / TimeSpeedDivide;
            }

            moneyText.text = "" + money;

            if (ship_Controller.stats.CurrentHealth <= 0)
            {
                IsDead = true;
                UILose.SetActive(true);
                UIInGame.SetActive(false);
                PlayerPrefs.SetInt("Money", money);
                if (ProgressHightScore < Progress)
                {
                    PlayerPrefs.SetFloat("Progress", Progress);
                    ProgressHightScore = PlayerPrefs.GetFloat("Progress", Progress);
                }
            }

            if (Progress >= 70)
            {
                asteroid_.enabled = false;
                Debug.Log("Boss fight");
            }

        }

    }


    /// <summary>
    /// Button Upgrade/Achat avec l'argent que le joueur a gagnier durant la partie
    /// </summary>
    public void OnClickUpagrade2()
    {
        if(money == PriceGunUp2)
        {
            Gunup2 = true;
            PlayerPrefs.SetInt("Money", money);
            PriceGunUp2Button.enabled = false;
        }
        else
        {
            Debug.Log("Argent Manquant");
        }
    }
    public void OnClickUpagrade3()
    {
        if(money == PriceGunUp3)
        {
            Gunup2 = true;
            Gunup3 = true;
            PlayerPrefs.SetInt("Money", money);
            PriceGunUp3Button.enabled = false;
            PriceGunUp2Button.enabled = false;
        }
        else
        {
            Debug.Log("Argent Manquant");
        }

    }


    /// <summary>
    /// Menu de Start avec le button Quitter, Start , Option
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
        PlayerPrefs.SetInt("Money", money);
    }


    public void LoadScene()
    {
        SceneManager.LoadScene(1);
    }

}
