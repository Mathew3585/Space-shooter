using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using EZCameraShake;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class GameManager : MonoBehaviour
{
    [Header("Mode")]
    public bool Menu;
    public bool Game;
    public bool WarpMode;

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

    public bool UltimateActive;

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
    public int PriceGunUp2;
    public int PriceGunUp3;
    [Header("Price Text")]
    public TextMeshProUGUI PriceGunUp2Text;
    public TextMeshProUGUI PriceGunUp3Text;
    [Header("Button Upgradde")]
    public Button PriceGunUp2Button;
    public Button PriceGunUp3Button;

    public CameraShaker cameraShaker;

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

    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
        }
        catch (ConsentCheckException e)
        {
            // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately.
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
        if (WarpMode == true)
        {
            Debug.Log(WarpMode);
            cameraShaker.ShakeOnce(0.7f, 0.2f, 0.2f, 0.7f);
        }

    }


    /// <summary>
    /// Button Upgrade/Achat avec l'argent que le joueur a gagnier durant la partie
    /// </summary>
    //Button Upagrade Gun 1
    public void OnClickUpagrade2()
    {
        if(money >= PriceGunUp2)
        {
            Gunup2 = true;
            PlayerPrefs.SetInt("Money", money);
            PriceGunUp2Button.enabled = false;
            money -= PriceGunUp2;
            PlayerPrefs.SetInt("Money", money);
            Dictionary<string, object> UpgardeGunShip = new Dictionary<string, object>()
        {
            { "Upgrade Gun 1",  Gunup2 },
        };

            // The ‘myEvent’ event will get queued up and sent every minute
            AnalyticsService.Instance.CustomData("Upgarde", UpgardeGunShip);

            // Optional - You can call Events.Flush() to send the event immediately
            AnalyticsService.Instance.Flush();

            Debug.Log("analitics Résult : " + UpgardeGunShip);
        }
        else
        {
            Debug.Log("Argent Manquant");
        }
    }

    //Button Upagrade Gun 2
    public void OnClickUpagrade3()
    {
        if(money >= PriceGunUp3)
        {
            Gunup2 = true;
            Gunup3 = true;
            PlayerPrefs.SetInt("Money", money);
            PriceGunUp3Button.enabled = false;
            PriceGunUp2Button.enabled = false;
            money -= PriceGunUp3;
            PlayerPrefs.SetInt("Money", money);
            Dictionary<string, object> UpgardeGunShip = new Dictionary<string, object>()
        {
            { "Upgrade Gun 2",  Gunup3 },
        };

            // The ‘myEvent’ event will get queued up and sent every minute
            AnalyticsService.Instance.CustomData("Upgarde", UpgardeGunShip);

            // Optional - You can call Events.Flush() to send the event immediately
            AnalyticsService.Instance.Flush();

            Debug.Log("analitics Résult : " + UpgardeGunShip);
        }
        else
        {
            Debug.Log("Argent Manquant");
        }

    }


    void SendEnvent()
    {
        
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
