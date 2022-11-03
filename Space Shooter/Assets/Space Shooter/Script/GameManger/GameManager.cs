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
using System.Linq;

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

}

[System.Serializable]
public class Warp
{
    public CameraShaker cameraShaker;
}

[System.Serializable]
public class UpgradeGun
{
    [Header("Price Value")]
    public int PriceGunUpgarde1;
    public int PriceGunUpgarde2;

    [Space(10)]
    [Header("Price Text")]
    public TextMeshProUGUI PriceGunUp1Text;
    public TextMeshProUGUI PriceGunUp2Text;

    [Space(10)]
    [Header("Button Upgradde")]
    public Button PriceGunUp1Button;
    public Button PriceGunUp2Button;

    [Space(10)]
    [Header("Upagrade Gun")]
    public bool[] UpagradeBaseShip;
    public bool[] GunUpgardeShip1;
    public bool[] GunUpgardeShip2;
    public bool[] GunUpgardeShip3;
    public bool[] GunUpgardeShip4;


}

[System.Serializable]
public class ShipUnlock
{
    [Header("Price Value")]
    public int PriceGunUpgarde1;
    public int PriceGunUpgarde2;

    [Space(10)]
    [Header("Bools")]
    public bool BaseShip = true;
    public bool Ship2;
    public bool Ship3;
    public bool Ship4;
    public bool Ship5;

    [Space(10)]
    [Header("Price Text")]
    public TextMeshProUGUI SpecShip1TextLife;
    public TextMeshProUGUI SpecShip1TextMoveSpeed;
    public TextMeshProUGUI SpecShip1TextFireRate;
    public TextMeshProUGUI SpecShip1TextPrice;

    [Space(10)]
    public TextMeshProUGUI SpecShip2TextLife;
    public TextMeshProUGUI SpecShip2TextMoveSpeed;
    public TextMeshProUGUI SpecShip2TextFireRate;
    public TextMeshProUGUI SpecShip2TextPrice;

    [Space(10)]
    public TextMeshProUGUI SpecShip3TextLife;
    public TextMeshProUGUI SpecShip3TextMoveSpeed;
    public TextMeshProUGUI SpecShip3TextFireRate;
    public TextMeshProUGUI SpecShip3TextPrice;

    [Space(10)]
    public TextMeshProUGUI SpecShip4TextLife;
    public TextMeshProUGUI SpecShip4TextMoveSpeed;
    public TextMeshProUGUI SpecShip4TextFireRate;
    public TextMeshProUGUI SpecShip4TextPrice;

    [Space(10)]
    public TextMeshProUGUI SpecShip5TextLife;
    public TextMeshProUGUI SpecShip5TextMoveSpeed;
    public TextMeshProUGUI SpecShip5TextFireRate;
    public TextMeshProUGUI SpecShip5TextPrice;

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

    public ChangeShip changeShip;
    public Game game;
    public ShipUnlock shipUnlock;
    public UpgradeGun upgrade;
    public Warp warp;
    public Upgrade upgradeScript;

    [Header("Float")]
    public float MaxHealthAllShip;
    public float MaxSpeedAllShip;
    public float MaxFireRateAllShip;


    public void Awake()
    {
        //Load bools ShipUnlock
        shipUnlock.Ship2 = PlayerPrefs.GetInt("shipUnlock.Ship2") == 1 ? true : false;
        shipUnlock.Ship3 = PlayerPrefs.GetInt("shipUnlock.Ship3") == 1 ? true : false;
        shipUnlock.Ship4 = PlayerPrefs.GetInt("shipUnlock.Ship4") == 1 ? true : false;
        shipUnlock.Ship5 = PlayerPrefs.GetInt("shipUnlock.Ship5") == 1 ? true : false;

        //Load Bools Upagrades
        upgrade.UpagradeBaseShip[1] = PlayerPrefs.GetInt("UpagradeGunBaseShip 1") == 1 ? true : false;
        upgrade.UpagradeBaseShip[2] = PlayerPrefs.GetInt("UpagradeGunBaseShip 2") == 1 ? true : false;

        //Load Money Player
        money = PlayerPrefs.GetInt("Money");


        shipUnlock.BaseShip = true;

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
        if (WarpMode)
        {
            warp.cameraShaker = GameObject.FindObjectOfType<CameraShaker>();
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

        //Si Menu est activer
        if (Menu == true)
        {
            upgrade.PriceGunUp1Text.text = upgrade.PriceGunUpgarde1.ToString();
            upgrade.PriceGunUp2Text.text = upgrade.PriceGunUpgarde2.ToString();
            Debug.Log("Menu Activer");

            //Afficher et Changer les emplacement des upgrades par rapport au vaisau au start  
            if (changeShip.CurrentSpaceShipSelect == 0)
            {
                Debug.Log("Vaiseaux de Base Activer");
                upgradeScript.UpagrdeBaseShip[0].SetActive(true);
                //A retravailler
                foreach (GameObject ship in upgradeScript.UpagrdeShip1) { ship.SetActive(false); }
                //Upagrade Gun
                if (upgrade.UpagradeBaseShip[1] == true)
                {
                    Debug.Log("Upgrade 1 mise a jour");
                    upgradeScript.UpagrdeBaseShip[0].SetActive(true);
                    upgradeScript.UpagrdeBaseShip[1].SetActive(true);
                }

                if (upgrade.UpagradeBaseShip[2] == true)
                {
                    Debug.Log("Upgrade 2 mise a jour");
                    upgradeScript.UpagrdeBaseShip[0].SetActive(true);
                    upgradeScript.UpagrdeBaseShip[1].SetActive(true);
                    upgradeScript.UpagrdeBaseShip[2].SetActive(true);
                }
            }

            if (changeShip.CurrentSpaceShipSelect == 1)
            {
                upgradeScript.UpagrdeShip1[0].SetActive(true);
                foreach (GameObject ship in upgradeScript.UpagrdeBaseShip) { ship.SetActive(false); }
                Debug.Log("Vaiseaux 1");
                //Upagrade Gun
                if (upgrade.GunUpgardeShip1[1] == true)
                {
                    Debug.Log("Upgrade 1 mise a jour");
                    upgradeScript.UpagrdeShip1[0].SetActive(true);
                    upgradeScript.UpagrdeShip1[1].SetActive(true);
                }

                if (upgrade.GunUpgardeShip1[2] == true)
                {
                    Debug.Log("Upgrade 2 mise a jour");
                    upgradeScript.UpagrdeShip1[0].SetActive(true);
                    upgradeScript.UpagrdeShip1[1].SetActive(true);
                    upgradeScript.UpagrdeShip1[2].SetActive(true);
                }
            }

        }


        //Faire spawn le vaisseaux séléctionné
        if (changeShip.CurrentSpaceShipSelect == 0)
        {
            changeShip.SpaceShip[0].SetActive(true);
            changeShip.SpaceShip[1].SetActive(false);
            changeShip.SpaceShip[2].SetActive(false);
            changeShip.SpaceShip[3].SetActive(false);
            changeShip.SpaceShip[4].SetActive(false);
        }
        if (changeShip.CurrentSpaceShipSelect == 1)
        {
            changeShip.SpaceShip[0].SetActive(false);
            changeShip.SpaceShip[1].SetActive(true);
            changeShip.SpaceShip[2].SetActive(false);
            changeShip.SpaceShip[3].SetActive(false);
            changeShip.SpaceShip[4].SetActive(false);
        }
        if (changeShip.CurrentSpaceShipSelect == 2)
        {
            changeShip.SpaceShip[0].SetActive(false);
            changeShip.SpaceShip[1].SetActive(false);
            changeShip.SpaceShip[2].SetActive(true);
            changeShip.SpaceShip[3].SetActive(false);
            changeShip.SpaceShip[4].SetActive(false);
        }
        if (changeShip.CurrentSpaceShipSelect == 3)
        {
            changeShip.SpaceShip[0].SetActive(false);
            changeShip.SpaceShip[1].SetActive(false);
            changeShip.SpaceShip[2].SetActive(false);
            changeShip.SpaceShip[3].SetActive(true);
            changeShip.SpaceShip[4].SetActive(false);
        }
        if (changeShip.CurrentSpaceShipSelect == 4)
        {
            changeShip.SpaceShip[0].SetActive(false);
            changeShip.SpaceShip[1].SetActive(false);
            changeShip.SpaceShip[2].SetActive(false);
            changeShip.SpaceShip[4].SetActive(true);
        }
    }

    void Update()
    {
       if(Menu == true)
        {
            //Afficher et Changer les emplacement des upgrades par rapport au vaisau au start  
            if (changeShip.CurrentSpaceShipSelect == 0)
            {
                Debug.Log("Vaiseaux de Base Activer");
                upgradeScript.UpagrdeBaseShip[0].SetActive(true);
                //Upagrade Gun
                if (upgrade.UpagradeBaseShip[1] == true)
                {
                    Debug.Log("Upgrade 1 mise a jour");
                    upgradeScript.UpagrdeBaseShip[0].SetActive(true);
                    upgradeScript.UpagrdeBaseShip[1].SetActive(true);
                }

                if (upgrade.UpagradeBaseShip[2] == true)
                {
                    Debug.Log("Upgrade 2 mise a jour");
                    upgradeScript.UpagrdeBaseShip[0].SetActive(true);
                    upgradeScript.UpagrdeBaseShip[1].SetActive(true);
                    upgradeScript.UpagrdeBaseShip[2].SetActive(true);
                }
            }

            if (changeShip.CurrentSpaceShipSelect == 1)
            {
                upgradeScript.UpagrdeShip1[0].SetActive(true);
                Debug.Log("Vaiseaux de Base Activer");
                //Upagrade Gun
                if (upgrade.GunUpgardeShip1[1] == true)
                {
                    Debug.Log("Upgrade 1 mise a jour");
                    upgradeScript.UpagrdeShip1[0].SetActive(true);
                    upgradeScript.UpagrdeShip1[1].SetActive(true);
                }

                if (upgrade.GunUpgardeShip1[2] == true)
                {
                    Debug.Log("Upgrade 2 mise a jour");
                    upgradeScript.UpagrdeShip1[0].SetActive(true);
                    upgradeScript.UpagrdeShip1[1].SetActive(true);
                    upgradeScript.UpagrdeShip1[2].SetActive(true);
                }
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

            if (game.ship_Controller.shipStats.CurrentHealth <= 0)
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

        //Si WarpMode est Activer
        if (WarpMode == true)
        {
            Debug.Log(WarpMode);
            warp.cameraShaker.ShakeOnce(0.7f, 0.2f, 0.2f, 0.7f);
        }

    }


    /// <summary>
    /// Button Upgrade/Achat avec l'argent que le joueur a gagnier durant la partie
    /// </summary>
    //Button Upagrade Gun 1
    public void OnClickUpagrade2()
    {
        if(money >= upgrade.PriceGunUpgarde1)
        {
            if(changeShip.CurrentSpaceShipSelect == 0)
            {
                upgrade.UpagradeBaseShip[1] = true;
                PlayerPrefs.SetInt("UpagradeGunBaseShip 1", upgrade.UpagradeBaseShip[1] ? 1 : 0);
                PlayerPrefs.SetInt("Money", money);
                upgrade.PriceGunUp1Button.enabled = false;
                money -= upgrade.PriceGunUpgarde1;
                PlayerPrefs.SetInt("Money", money);
                Dictionary<string, object> UpgardeGunShip = new Dictionary<string, object>()
                {
                    { "UpgradeGun1",  upgrade.UpagradeBaseShip[1] },
                };

                // The ‘myEvent’ event will get queued up and sent every minute
                AnalyticsService.Instance.CustomData("Upgarde", UpgardeGunShip);

                // Optional - You can call Events.Flush() to send the event immediately
                AnalyticsService.Instance.Flush();

                Debug.Log("analitics Résult : " + UpgardeGunShip);
            }
        }
        else
        {
            upgrade.UpagradeBaseShip[0] = false;
            upgrade.UpagradeBaseShip[1] = false;
            Debug.Log("Argent Manquant");
        }
    }

    //Button Upagrade Gun 2
    public void OnClickUpagrade3()
    {
        if(money >= upgrade.PriceGunUpgarde2 && upgrade.UpagradeBaseShip[1] == true)
        {
            if (changeShip.CurrentSpaceShipSelect == 0)
            {
                upgrade.UpagradeBaseShip[2] = true;
                PlayerPrefs.SetInt("UpagradeGunBaseShip 2", upgrade.UpagradeBaseShip[2] ? 1 : 0);
                PlayerPrefs.SetInt("Money", money);
                upgrade.PriceGunUp2Button.enabled = false;
                upgrade.PriceGunUp1Button.enabled = false;
                money -= upgrade.PriceGunUpgarde2;
                PlayerPrefs.SetInt("Money", money);
                Dictionary<string, object> UpgardeGunShip = new Dictionary<string, object>()
        {
            { "UpgradeGun2",  upgrade.UpagradeBaseShip[2] },
        };

                // The ‘myEvent’ event will get queued up and sent every minute
                AnalyticsService.Instance.CustomData("Upgarde", UpgardeGunShip);

                // Optional - You can call Events.Flush() to send the event immediately
                AnalyticsService.Instance.Flush();

                Debug.Log("analitics Résult : " + UpgardeGunShip);
            }
        }
                
        else
        {
            upgrade.UpagradeBaseShip[2] = false;
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
