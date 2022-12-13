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
    public TextMeshProUGUI ProgressHightScoreText;
    public TextMeshProUGUI ProgressText;

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
    [Header("Price Upgrade Value Base Ship")]
    public int PriceGunUpgarde1BaseShip;
    public int PriceGunUpgarde2BaseShip;

    [Space(10)]
    [Header("Price Upgrade Value Ship 1 ")]
    public int PriceGunUpgarde1Ship1;
    public int PriceGunUpgarde2Ship1;

    [Space(10)]
    [Header("Text Upagrade Already Bought")]
    public string AlreadyBoughtText;

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
    public bool GreekMenu;
    public bool TutoMenu;
    public bool Game;
    public bool Tuto;
    public bool WarpMode;

    [Header("Value")]
    public int money;
    private int CurrentMoney;

    public Animator transition;
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
    public float TransitionTime;

    [Space(10)]
    [Header("Progress Value")]
    public int ProgressPhase1;
    public int ProgressPhase2;
    public int ProgressPhase3;
    public int BossFight;

    [Space(5)]
    public bool ValidatePhase1;
    public bool ValidatePhase2;
    public bool ValidatePhase3;
    public bool ValidateBossFight;

    [Header("Ennemis Phase")]
    public int NumbresEnnemisPhase1;
    public int NumbresEnnemisPhase2;
    public int NumbresEnnemisPhase3;
    public int NumbresBoss;





    public void Awake()
    {
        transition.SetTrigger("Start");
        if (TutoMenu)
        {
            return;
        }
        //Load bools ShipUnlock
        shipUnlock.Ship2 = PlayerPrefs.GetInt("shipUnlock.Ship2") == 1 ? true : false;
        shipUnlock.Ship3 = PlayerPrefs.GetInt("shipUnlock.Ship3") == 1 ? true : false;
        shipUnlock.Ship4 = PlayerPrefs.GetInt("shipUnlock.Ship4") == 1 ? true : false;
        shipUnlock.Ship5 = PlayerPrefs.GetInt("shipUnlock.Ship5") == 1 ? true : false;

        //Load Bools Upagrades
        //BaseShip
        upgrade.UpagradeBaseShip[0] = true;
        upgrade.UpagradeBaseShip[1] = PlayerPrefs.GetInt("UpagradeGunBaseShip 1") == 1 ? true : false;
        upgrade.UpagradeBaseShip[2] = PlayerPrefs.GetInt("UpagradeGunBaseShip 2") == 1 ? true : false;

        //Gun ship 1
        upgrade.GunUpgardeShip1[0] = true;
        upgrade.GunUpgardeShip1[1] = PlayerPrefs.GetInt("UpagradeShip1 Gun1") == 1 ? true : false;
        upgrade.GunUpgardeShip1[2] = PlayerPrefs.GetInt("UpagradeShip1 Gun2") == 1 ? true : false;

        //Load Money Player
        money = PlayerPrefs.GetInt("Money");


        shipUnlock.BaseShip = true;


        //Si Game est Activer
        if (Game == true)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            game.ship_Controller = GameObject.FindObjectOfType<Ship_Controller>();
            game.UILose.SetActive(false);
            game.UIInGame.SetActive(true);
            CurrentMoney = PlayerPrefs.GetInt("Money", money);
            game.asteroid_ = GameObject.FindObjectOfType<Asteroid_Field>();
            Debug.Log("Game Activer");

        }
        if (Tuto)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (WarpMode)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            warp.cameraShaker = GameObject.FindObjectOfType<CameraShaker>();

        }
        if (GreekMenu)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }

    async void Start()
    {

        if (TutoMenu)
        {
            return;
        }

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
        if (GreekMenu == true)
        {
            if(changeShip.CurrentSpaceShipSelect == 0)
            {
                upgrade.PriceGunUp1Text.text = upgrade.PriceGunUpgarde1BaseShip.ToString();
                upgrade.PriceGunUp2Text.text = upgrade.PriceGunUpgarde2BaseShip.ToString();

                if (upgrade.UpagradeBaseShip[1] == true)
                {
                    upgrade.PriceGunUp1Text.text = upgrade.AlreadyBoughtText.ToString();
                    upgrade.PriceGunUp1Button.enabled = false;
                }
                if (upgrade.UpagradeBaseShip[2] == true)
                {
                    upgrade.PriceGunUp2Button.enabled = false;
                    upgrade.PriceGunUp2Text.text = upgrade.AlreadyBoughtText.ToString();
                }
            }

            if (changeShip.CurrentSpaceShipSelect == 1)
            {
                upgrade.PriceGunUp1Text.text = upgrade.PriceGunUpgarde1Ship1.ToString();
                upgrade.PriceGunUp2Text.text = upgrade.PriceGunUpgarde2Ship1.ToString();

                if (upgrade.GunUpgardeShip1[1] == true)
                {
                    upgrade.PriceGunUp1Button.enabled = false;
                    upgrade.PriceGunUp1Text.text = upgrade.AlreadyBoughtText.ToString();
                    Debug.Log(upgrade.PriceGunUp1Text.text);
                }
                if (upgrade.GunUpgardeShip1[2] == true)
                {
                    upgrade.PriceGunUp2Button.enabled = false;
                    upgrade.PriceGunUp2Text.text = upgrade.AlreadyBoughtText.ToString();
                    Debug.Log(upgrade.PriceGunUp2Text.text);
                }
            }

            Debug.Log("Menu Activer");
        }

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


        //Faire spawn le vaisseaux séléctionné
        if (changeShip.CurrentSpaceShipSelect == 0)
        {
            changeShip.SpaceShip[0].SetActive(true);
            changeShip.SpaceShip[1].SetActive(false);
            changeShip.SpaceShip[2].SetActive(false);
            changeShip.SpaceShip[3].SetActive(false);
            changeShip.SpaceShip[4].SetActive(false);
        }
        else if (changeShip.CurrentSpaceShipSelect == 1)
        {
            changeShip.SpaceShip[0].SetActive(false);
            changeShip.SpaceShip[1].SetActive(true);
            changeShip.SpaceShip[2].SetActive(false);
            changeShip.SpaceShip[3].SetActive(false);
            changeShip.SpaceShip[4].SetActive(false);
        }
        else if (changeShip.CurrentSpaceShipSelect == 2)
        {
            changeShip.SpaceShip[0].SetActive(false);
            changeShip.SpaceShip[1].SetActive(false);
            changeShip.SpaceShip[2].SetActive(true);
            changeShip.SpaceShip[3].SetActive(false);
            changeShip.SpaceShip[4].SetActive(false);
        }
        else if (changeShip.CurrentSpaceShipSelect == 3)
        {
            changeShip.SpaceShip[0].SetActive(false);
            changeShip.SpaceShip[1].SetActive(false);
            changeShip.SpaceShip[2].SetActive(false);
            changeShip.SpaceShip[3].SetActive(true);
            changeShip.SpaceShip[4].SetActive(false);
        }
        else if (changeShip.CurrentSpaceShipSelect == 4)
        {
            changeShip.SpaceShip[0].SetActive(false);
            changeShip.SpaceShip[1].SetActive(false);
            changeShip.SpaceShip[2].SetActive(false);
            changeShip.SpaceShip[4].SetActive(true);
        }
    }

    void Update()
    {
        if (TutoMenu)
        {
            return;
        }

        if (GreekMenu == true)
        {
            //Changer les prix en fonction du vaissau selectionner et Activer ou desactiver
            if (changeShip.CurrentSpaceShipSelect == 0)
            {
                upgrade.PriceGunUp1Text.text = upgrade.PriceGunUpgarde1BaseShip.ToString();
                upgrade.PriceGunUp2Text.text = upgrade.PriceGunUpgarde2BaseShip.ToString();
                upgrade.PriceGunUp1Button.enabled = true;
                upgrade.PriceGunUp2Button.enabled = true;

                if (upgrade.UpagradeBaseShip[1] == true)
                {
                    upgrade.PriceGunUp1Button.enabled = false;
                    upgrade.PriceGunUp1Text.text = upgrade.AlreadyBoughtText.ToString();
                }
                if (upgrade.UpagradeBaseShip[2] == true)
                {
                    upgrade.PriceGunUp2Button.enabled = false;
                    upgrade.PriceGunUp2Text.text = upgrade.AlreadyBoughtText.ToString();
                }
            }

            if (changeShip.CurrentSpaceShipSelect == 1)
            {
                upgrade.PriceGunUp1Text.text = upgrade.PriceGunUpgarde1Ship1.ToString();
                upgrade.PriceGunUp2Text.text = upgrade.PriceGunUpgarde2Ship1.ToString();
                upgrade.PriceGunUp1Button.enabled = true;
                upgrade.PriceGunUp2Button.enabled = true;

                if (upgrade.GunUpgardeShip1[1] == true)
                {
                    upgrade.PriceGunUp1Button.enabled = false;
                    upgrade.PriceGunUp1Text.text = upgrade.AlreadyBoughtText.ToString();             
                }
                if (upgrade.GunUpgardeShip1[2] == true)
                {
                    upgrade.PriceGunUp2Button.enabled = false;
                    upgrade.PriceGunUp2Text.text = upgrade.AlreadyBoughtText.ToString();
                }
            }
       }
        //Si Game est Activer
        if(Game == true)
        {

            game.ProgressHightScore = PlayerPrefs.GetFloat("Progress", game.Progress);
            if (game.Progress <= ProgressPhase1)
            {
                ValidatePhase1 = true;
                ValidatePhase2 = false;
                ValidatePhase3 = false;

            }
            else if (game.Progress >= ProgressPhase1)
            {
                ValidatePhase1 = false;
                ValidatePhase2 = true;
                ValidatePhase3 = false;

            }

            if (game.Progress >= ProgressPhase3)
            {
                if (game.Progress >= BossFight)
                {
                    ValidatePhase1 = false;
                    ValidatePhase2 = false;
                    ValidatePhase3 = false;
                    ValidateBossFight = true;
                }
                else
                {
                    ValidatePhase1 = false;
                    ValidatePhase2 = false;
                    ValidatePhase3 = true;
                    ValidateBossFight = false;
                }
            }


            if (ValidateBossFight)
            {
                game.Progress = 90;
                Debug.Log("Boss fight");
            }

            else if (game.IsDead == false)
            {
                game.Progress += Time.deltaTime / game.TimeSpeedDivide;
            }

            game.moneyText.text = "" + money;

            if (game.ship_Controller.shipStats.CurrentHealth <= 0)
            {
                game.IsDead = true;
                game.UILose.SetActive(true);
                game.UIInGame.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                PlayerPrefs.SetInt("Money", money);
                if (game.ProgressHightScore < game.Progress)
                {
                    PlayerPrefs.SetFloat("Progress", game.Progress);
                    game.ProgressHightScore = PlayerPrefs.GetFloat("Progress", game.Progress);
                }
                int ProgressText = (int) game.Progress;
                int ProgressHightScoreText = (int)game.ProgressHightScore;
                game.ProgressText.text = ProgressText.ToString() + "%";
                game.ProgressHightScoreText.text = ProgressHightScoreText.ToString() + "%";
            }

        }

        //Si WarpMode est Activer
        if (WarpMode == true)
        {
            Debug.Log(WarpMode);
            warp.cameraShaker.ShakeOnce(0.7f, 0.2f, 0.2f, 0.7f);
        }


        //Afficher et Changer les emplacement des upgrades par rapport au vaisau au start  
        if (changeShip.CurrentSpaceShipSelect == 0)
        {
            if (Game)
            {
                if (game.ship_Controller.Isdead)
                {
                    return;
                }
            }

            Debug.Log("Vaiseaux de Base Activer");
            upgradeScript.UpagrdeBaseShip[0].SetActive(true);
            //A Ameliorer 
            upgradeScript.UpagrdeShip1[0].SetActive(false);
            upgradeScript.UpagrdeShip1[1].SetActive(false);
            upgradeScript.UpagrdeShip1[2].SetActive(false);
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
            if (Game)
            {
                if (game.ship_Controller.Isdead)
                {
                    return;
                }
            }

            upgradeScript.UpagrdeShip1[0].SetActive(true);
            //A Ameliorer 
            upgradeScript.UpagrdeBaseShip[0].SetActive(false);
            upgradeScript.UpagrdeBaseShip[1].SetActive(false);
            upgradeScript.UpagrdeBaseShip[2].SetActive(false);
            Debug.Log("Vaiseaux 2 Activer");
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


    /// <summary>
    /// Button Upgrade/Achat avec l'argent que le joueur a gagnier durant la partie
    /// </summary>
    //Button Upagrade Gun 1
    public void OnClickUpagrade2()
    {
        if(money >= upgrade.PriceGunUpgarde1BaseShip)
        {
            if(changeShip.CurrentSpaceShipSelect == 0)
            {
                upgrade.UpagradeBaseShip[1] = true;
                PlayerPrefs.SetInt("UpagradeGunBaseShip 1", upgrade.UpagradeBaseShip[1] ? 1 : 0);
                PlayerPrefs.SetInt("Money", money);
                upgrade.PriceGunUp1Button.enabled = false;
                money -= upgrade.PriceGunUpgarde1BaseShip;
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

        if (money >= upgrade.PriceGunUpgarde1BaseShip && upgrade.GunUpgardeShip1[0] == true)
        {
            if (changeShip.CurrentSpaceShipSelect == 0)
            {
                upgrade.GunUpgardeShip1[2] = true;
                PlayerPrefs.SetInt("UpagradeShip1 Gun1", upgrade.GunUpgardeShip1[1] ? 1 : 0);
                PlayerPrefs.SetInt("Money", money);
                upgrade.PriceGunUp2Button.enabled = false;
                upgrade.PriceGunUp1Button.enabled = false;
                money -= upgrade.PriceGunUpgarde1Ship1;
                PlayerPrefs.SetInt("Money", money);
                Dictionary<string, object> UpgardeGunShip = new Dictionary<string, object>()
        {
            { "UpgradeGun1Ship1",  upgrade.GunUpgardeShip1[2] },
        };

                // The ‘myEvent’ event will get queued up and sent every minute
                AnalyticsService.Instance.CustomData("Upgarde", UpgardeGunShip);

                // Optional - You can call Events.Flush() to send the event immediately
                AnalyticsService.Instance.Flush();

                Debug.Log("analitics Résult : " + UpgardeGunShip);
            }

        }


    }

    //Button Upagrade Gun 2
    public void OnClickUpagrade3()
    {
        if(money >= upgrade.PriceGunUpgarde2BaseShip && upgrade.UpagradeBaseShip[1] == true)
        {
            if (changeShip.CurrentSpaceShipSelect == 0)
            {
                upgrade.UpagradeBaseShip[2] = true;
                PlayerPrefs.SetInt("UpagradeGunBaseShip 2", upgrade.UpagradeBaseShip[2] ? 1 : 0);
                PlayerPrefs.SetInt("Money", money);
                upgrade.PriceGunUp2Button.enabled = false;
                upgrade.PriceGunUp1Button.enabled = false;
                money -= upgrade.PriceGunUpgarde2BaseShip;
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

        if (money >= upgrade.PriceGunUpgarde2BaseShip && upgrade.GunUpgardeShip1[1] == true)
        {
            if (changeShip.CurrentSpaceShipSelect == 1)
            {
                upgrade.GunUpgardeShip1[2] = true;
                PlayerPrefs.SetInt("UpagradeShip1 Gun2", upgrade.GunUpgardeShip1[2] ? 1 : 0);
                PlayerPrefs.SetInt("Money", money);
                upgrade.PriceGunUp2Button.enabled = false;
                upgrade.PriceGunUp1Button.enabled = false;
                money -= upgrade.PriceGunUpgarde2BaseShip;
                PlayerPrefs.SetInt("Money", money);
                Dictionary<string, object> UpgardeGunShip = new Dictionary<string, object>()
        {
            { "UpgradeGun2Ship1",  upgrade.GunUpgardeShip1[2] },
        };

                // The ‘myEvent’ event will get queued up and sent every minute
                AnalyticsService.Instance.CustomData("Upgarde", UpgardeGunShip);

                // Optional - You can call Events.Flush() to send the event immediately
                AnalyticsService.Instance.Flush();

                Debug.Log("analitics Résult : " + UpgardeGunShip);
            }

        }

    }


    void SendEnvent()
    {
        
    }

    /// <summary>
    /// Menu de Start avec le button Quitter, Start , Option
    /// </summary>
    /// 

    public void Play()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }


    public void Retry()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }
    public void QuitGame()
    {
        Application.Quit();
        PlayerPrefs.SetInt("Money", money);
    }

    IEnumerator LoadLevel(int LevelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(TransitionTime);

        SceneManager.LoadScene(LevelIndex);
    }


}
