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

[System.Serializable]
public class Game
{
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

    [Header("Script")]
    [Tooltip("Il trouve le player tout seul")]
    public Ship_Controller ship_Controller;

    [Header("Asteroid générator")]
    public Asteroid_Field asteroid_;
    public CameraShaker cameraShaker;
}

[System.Serializable]
public class Menu
{
    [Header("Price Value")]
    public int PriceGunUp2;
    public int PriceGunUp3;
    [Header("Price Text")]
    public TextMeshProUGUI PriceGunUp2Text;
    public TextMeshProUGUI PriceGunUp3Text;
    [Header("Button Upgradde")]
    public Button PriceGunUp2Button;
    public Button PriceGunUp3Button;
}
[System.Serializable]
public class Upgrade
{
    [Header("Upagrade Gun")]
    public bool Gunup1;
    public bool Gunup2;
    public bool Gunup3;
    [Header("Gun GameObject")]
    public GameObject Gun1;
    public GameObject Gun2;
    public GameObject Gun3;

}

public class GameManager : MonoBehaviour
{
    

    [Header("Mode")]
    public bool Menu;
    public bool Game;
    public bool WarpMode;

    [Header("Value")]
    public int money;
    private int CurrentMoney;

    public Game game;
    public Menu menu;
    public Upgrade upgrade;

    public void Awake()
    {
        money = PlayerPrefs.GetInt("Money");
        upgrade.Gunup1 = true;

        //Si Menu est activer
        if (Menu == true)
        {
            if (upgrade.Gunup1 == true)
            {
                upgrade.Gun1.SetActive(true);
                upgrade.Gun2.SetActive(false);
                upgrade.Gun3.SetActive(false);
            }
            menu.PriceGunUp2Text.text = menu.PriceGunUp2.ToString();
            menu.PriceGunUp3Text.text = menu.PriceGunUp3.ToString();

            Debug.Log("Menu Activer");
        }

        //Si Game est Activer
        if (Game == true)
        {
            game.ship_Controller = GameObject.FindObjectOfType<Ship_Controller>();
            game.UILose.SetActive(false);
            game.UIInGame.SetActive(true);
            CurrentMoney = PlayerPrefs.GetInt("Money", money);
            game.asteroid_ = GameObject.FindObjectOfType<Asteroid_Field>();
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
            if (upgrade.Gunup2 == true)
            {
                upgrade.Gun1.SetActive(true);
                upgrade.Gun2.SetActive(true);
            }

            if (upgrade.Gunup3 == true)
            {
                upgrade.Gun1.SetActive(true);
                upgrade.Gun2.SetActive(true);
                upgrade.Gun3.SetActive(true);
            }
        }

        //Si Game est Activer
        if(Game == true)
        {

            game.ProgressHightScore = PlayerPrefs.GetFloat("Progress", game.Progress);
            if (game.IsDead == false)
            {
                game.Progress += Time.deltaTime / game.TimeSpeedDivide;
            }

            game.moneyText.text = "" + money;

            if (game.ship_Controller.stats.CurrentHealth <= 0)
            {
                game.IsDead = true;
                game.UILose.SetActive(true);
                game.UIInGame.SetActive(false);
                PlayerPrefs.SetInt("Money", money);
                if (game.ProgressHightScore < game.Progress)
                {
                    PlayerPrefs.SetFloat("Progress", game.Progress);
                    game.ProgressHightScore = PlayerPrefs.GetFloat("Progress", game.Progress);
                }
            }
            if (game.Progress >= 70)
            {
                game.asteroid_.enabled = false;
                Debug.Log("Boss fight");
            }

        }
        if (WarpMode == true)
        {
            Debug.Log(WarpMode);
            game.cameraShaker.ShakeOnce(0.7f, 0.2f, 0.2f, 0.7f);
        }

    }


    /// <summary>
    /// Button Upgrade/Achat avec l'argent que le joueur a gagnier durant la partie
    /// </summary>
    //Button Upagrade Gun 1
    public void OnClickUpagrade2()
    {
        if(money >= menu.PriceGunUp2)
        {
            upgrade.Gunup2 = true;
            PlayerPrefs.SetInt("Money", money);
            menu.PriceGunUp2Button.enabled = false;
            money -= menu.PriceGunUp2;
            PlayerPrefs.SetInt("Money", money);
            Dictionary<string, object> UpgardeGunShip = new Dictionary<string, object>()
        {
            { "UpgradeGun1",  upgrade.Gunup2 },
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
        if(money >= menu.PriceGunUp3 || upgrade.Gunup2 == true)
        {
            upgrade.Gunup3 = true;
            PlayerPrefs.SetInt("Money", money);
            menu.PriceGunUp3Button.enabled = false;
            menu.PriceGunUp2Button.enabled = false;
            money -= menu.PriceGunUp3;
            PlayerPrefs.SetInt("Money", money);
            Dictionary<string, object> UpgardeGunShip = new Dictionary<string, object>()
        {
            { "UpgradeGun2",  upgrade.Gunup3 },
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
