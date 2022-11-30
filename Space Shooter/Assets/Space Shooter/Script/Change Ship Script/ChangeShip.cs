using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeShip : MonoBehaviour
{
    public Button rightbutton;
    public Button leftbutton;

    [Space(10)]
    [Header("Button Unlock/ Select")]
    public GameObject Unlockbutton;
    public GameObject SelectShip;

    [Space(10)]
    [Header("List")]
    public GameObject[] SpaceShip;
    public GameObject[] SpaceShipDescription;
    public UnlockShip[] unlockShips;

    [Space(10)]
    [Header("Gameobject")]
    public UnlockShip CurrentUnlockShipsSciprt;
    public GameObject CurrentSpaceShip;
    private GameObject CurrentSpaceShipDescription;
    private GameManager gameManager;

    [Space(10)]
    [Header("Int")]
    public int i;
    public int CurrentSpaceShipSelect;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        CurrentSpaceShipSelect = PlayerPrefs.GetInt("Current_Space_Ship_Select", CurrentSpaceShipSelect);
    }

    private void Start()
    {
        CurrentSpaceShipSelect = PlayerPrefs.GetInt("Current_Space_Ship_Select", CurrentSpaceShipSelect);
        CurrentUnlockShipsSciprt = SpaceShip[CurrentSpaceShipSelect].gameObject.GetComponent<UnlockShip>();

        if (gameManager.GreekMenu)
        {
            i = CurrentSpaceShipSelect;
            CurrentSpaceShipSelect = i;
            CurrentSpaceShip = SpaceShip[i];
            CurrentSpaceShipDescription = SpaceShipDescription[i];

            // Afficher les valeurs des float sur la desctiption des vaissaux
            if (i == 0)
            {
                //SHIP 1 SPEC
                gameManager.shipUnlock.SpecShip1TextLife.text = "Life " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.maxHealth.ToString() + "  /  " + gameManager.MaxHealthAllShip;
                gameManager.shipUnlock.SpecShip1TextMoveSpeed.text = "Speed " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.MoveSpeed.ToString() + "  /  " + gameManager.MaxSpeedAllShip;
                gameManager.shipUnlock.SpecShip1TextFireRate.text = "FireRate " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.fireRate.ToString() + "  /  " + gameManager.MaxFireRateAllShip;

            }

            if (i == 1)
            {
                //SHIP 2 SPEC
                gameManager.shipUnlock.SpecShip2TextLife.text = "Life " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.maxHealth.ToString() + "  /  " + gameManager.MaxHealthAllShip;
                gameManager.shipUnlock.SpecShip2TextMoveSpeed.text = "Speed " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.MoveSpeed.ToString() + "  /  " + gameManager.MaxSpeedAllShip;
                gameManager.shipUnlock.SpecShip2TextFireRate.text = "FireRate " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.fireRate.ToString() + "  /  " + gameManager.MaxFireRateAllShip;
                if (unlockShips[1].IsUnlock == false)
                {
                    gameManager.shipUnlock.SpecShip1TextPrice.text = "Unlock: " + unlockShips[1].specShip.Price.ToString();
                }
            }

            if (i == 2)
            {
                //SHIP 3 SPEC
                gameManager.shipUnlock.SpecShip3TextLife.text = "Life " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.maxHealth.ToString() + "  /  " + gameManager.MaxHealthAllShip;
                gameManager.shipUnlock.SpecShip3TextMoveSpeed.text = "Speed " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.MoveSpeed.ToString() + "  /  " + gameManager.MaxSpeedAllShip;
                gameManager.shipUnlock.SpecShip3TextFireRate.text = "FireRate " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.fireRate.ToString() + "  /  " + gameManager.MaxFireRateAllShip;
                if (unlockShips[2].IsUnlock == false)
                {
                    gameManager.shipUnlock.SpecShip1TextPrice.text = "Unlock: " + unlockShips[2].specShip.Price.ToString();
                }
            }

            if (i == 3)
            {
                //SHIP 4 SPEC
                gameManager.shipUnlock.SpecShip4TextLife.text = "Life " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.maxHealth.ToString() + "  /  " + gameManager.MaxHealthAllShip;
                gameManager.shipUnlock.SpecShip4TextMoveSpeed.text = "Speed " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.MoveSpeed.ToString() + "  /  " + gameManager.MaxSpeedAllShip;
                gameManager.shipUnlock.SpecShip4TextFireRate.text = "FireRate " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.fireRate.ToString() + "  /  " + gameManager.MaxFireRateAllShip;
                if (unlockShips[3].IsUnlock == false)
                {
                    gameManager.shipUnlock.SpecShip1TextPrice.text = "Unlock: " + unlockShips[3].specShip.Price.ToString();
                }
            }

            if (i == 4)
            {
                //SHIP 5 SPEC
                gameManager.shipUnlock.SpecShip5TextLife.text = "Life " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.maxHealth.ToString() + "  /  " + gameManager.MaxHealthAllShip;
                gameManager.shipUnlock.SpecShip5TextMoveSpeed.text = "Speed " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.MoveSpeed.ToString() + "  /  " + gameManager.MaxSpeedAllShip;
                gameManager.shipUnlock.SpecShip5TextFireRate.text = "FireRate " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.fireRate.ToString() + "  /  " + gameManager.MaxFireRateAllShip;
                if (unlockShips[4].IsUnlock == false)
                {
                    gameManager.shipUnlock.SpecShip1TextPrice.text = "Unlock: " + unlockShips[4].specShip.Price.ToString();
                }
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GreekMenu)
        {
            // Activer Les bools dans le game Manager 
            if (gameManager.shipUnlock.Ship2 == true)
            {
                SelectShip.SetActive(true);
                Unlockbutton.SetActive(false);
            }
            if (gameManager.shipUnlock.Ship3 == true)
            {
                SelectShip.SetActive(true);
                Unlockbutton.SetActive(false);
            }
            if (gameManager.shipUnlock.Ship4 == true)
            {
                SelectShip.SetActive(true);
                Unlockbutton.SetActive(false);
            }
            if (gameManager.shipUnlock.Ship5 == true)
            {
                SelectShip.SetActive(true);
                Unlockbutton.SetActive(false);
            }
        }
    }


    public void BuyShip()
    {
        CurrentSpaceShipDescription.gameObject.SetActive(true);
        i = CurrentSpaceShipSelect;
        CurrentSpaceShip = SpaceShip[i];
    }

    public void Onclicknext()
    {
        if (++i >= SpaceShip.Length)
        {
            i = 0;
        }

        CurrentSpaceShip.gameObject.SetActive(false);
        CurrentSpaceShipDescription.gameObject.SetActive(false);
        CurrentSpaceShip = SpaceShip[i];
        CurrentSpaceShipDescription = SpaceShipDescription[i];
        CurrentUnlockShipsSciprt = unlockShips[i];
        CurrentSpaceShip.gameObject.SetActive(true);
        CurrentSpaceShipDescription.gameObject.SetActive(true);

        // Afficher les valeurs des float sur la desctiption des vaissaux
        if (i == 0)
        {
            //SHIP 1 SPEC
            gameManager.shipUnlock.SpecShip1TextLife.text = "Life " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.maxHealth.ToString() + "  /  " + gameManager.MaxHealthAllShip;
            gameManager.shipUnlock.SpecShip1TextMoveSpeed.text = "Speed " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.MoveSpeed.ToString() + "  /  " + gameManager.MaxSpeedAllShip;
            gameManager.shipUnlock.SpecShip1TextFireRate.text = "FireRate " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.fireRate.ToString() + "  /  " + gameManager.MaxFireRateAllShip;

        }

        if (i == 1)
        {
            //SHIP 2 SPEC
            gameManager.shipUnlock.SpecShip2TextLife.text = "Life " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.maxHealth.ToString() + "  /  " + gameManager.MaxHealthAllShip;
            gameManager.shipUnlock.SpecShip2TextMoveSpeed.text = "Speed " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.MoveSpeed.ToString() + "  /  " + gameManager.MaxSpeedAllShip;
            gameManager.shipUnlock.SpecShip2TextFireRate.text = "FireRate " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.fireRate.ToString() + "  /  " + gameManager.MaxFireRateAllShip;
            if (unlockShips[1].IsUnlock == false)
            {
                gameManager.shipUnlock.SpecShip1TextPrice.text = "Unlock: " + unlockShips[1].specShip.Price.ToString();
            }
        }

        if (i == 2)
        {
            //SHIP 3 SPEC
            gameManager.shipUnlock.SpecShip3TextLife.text = "Life " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.maxHealth.ToString() + "  /  " + gameManager.MaxHealthAllShip;
            gameManager.shipUnlock.SpecShip3TextMoveSpeed.text = "Speed " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.MoveSpeed.ToString() + "  /  " + gameManager.MaxSpeedAllShip;
            gameManager.shipUnlock.SpecShip3TextFireRate.text = "FireRate " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.fireRate.ToString() + "  /  " + gameManager.MaxFireRateAllShip;
            if (unlockShips[2].IsUnlock == false)
            {
                gameManager.shipUnlock.SpecShip1TextPrice.text = "Unlock: " + unlockShips[2].specShip.Price.ToString();
            }
        }

        if (i == 3)
        {
            //SHIP 4 SPEC
            gameManager.shipUnlock.SpecShip4TextLife.text = "Life " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.maxHealth.ToString() + "  /  " + gameManager.MaxHealthAllShip;
            gameManager.shipUnlock.SpecShip4TextMoveSpeed.text = "Speed " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.MoveSpeed.ToString() + "  /  " + gameManager.MaxSpeedAllShip;
            gameManager.shipUnlock.SpecShip4TextFireRate.text = "FireRate " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.fireRate.ToString() + "  /  " + gameManager.MaxFireRateAllShip;
            if (unlockShips[3].IsUnlock == false)
            {
                gameManager.shipUnlock.SpecShip1TextPrice.text = "Unlock: " + unlockShips[3].specShip.Price.ToString();
            }
        }

        if (i == 4)
        {
            //SHIP 5 SPEC
            gameManager.shipUnlock.SpecShip5TextLife.text = "Life " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.maxHealth.ToString() + "  /  " + gameManager.MaxHealthAllShip;
            gameManager.shipUnlock.SpecShip5TextMoveSpeed.text = "Speed " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.MoveSpeed.ToString() + "  /  " + gameManager.MaxSpeedAllShip;
            gameManager.shipUnlock.SpecShip5TextFireRate.text = "FireRate " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.fireRate.ToString() + "  /  " + gameManager.MaxFireRateAllShip;
            if (unlockShips[4].IsUnlock == false)
            {
                gameManager.shipUnlock.SpecShip1TextPrice.text = "Unlock: " + unlockShips[4].specShip.Price.ToString();
            }
        }


    }
    public void Onclickpast()
    {
        if (--i <= -1)
        {
            i = SpaceShip.Length -1;
        }

        CurrentSpaceShip.gameObject.SetActive(false);
        CurrentSpaceShipDescription.gameObject.SetActive(false);
        CurrentSpaceShip = SpaceShip[i];
        CurrentSpaceShipDescription = SpaceShipDescription[i];
        CurrentUnlockShipsSciprt = unlockShips[i];
        CurrentSpaceShip.gameObject.SetActive(true);
        CurrentSpaceShipDescription.gameObject.SetActive(true);

        // Afficher les valeurs des float sur la desctiption des vaissaux
        if (i == 0)
        {
            //SHIP 1 SPEC
            gameManager.shipUnlock.SpecShip1TextLife.text = "Life " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.maxHealth.ToString() + "  /  " + gameManager.MaxHealthAllShip;
            gameManager.shipUnlock.SpecShip1TextMoveSpeed.text = "Speed " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.MoveSpeed.ToString() + "  /  " + gameManager.MaxSpeedAllShip;
            gameManager.shipUnlock.SpecShip1TextFireRate.text = "FireRate " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.fireRate.ToString() + "  /  " + gameManager.MaxFireRateAllShip;

        }

        if (i == 1)
        {
            //SHIP 2 SPEC
            gameManager.shipUnlock.SpecShip2TextLife.text = "Life " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.maxHealth.ToString() + "  /  " + gameManager.MaxHealthAllShip;
            gameManager.shipUnlock.SpecShip2TextMoveSpeed.text = "Speed " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.MoveSpeed.ToString() + "  /  " + gameManager.MaxSpeedAllShip;
            gameManager.shipUnlock.SpecShip2TextFireRate.text = "FireRate " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.fireRate.ToString() + "  /  " + gameManager.MaxFireRateAllShip;
            if (unlockShips[1].IsUnlock == false)
            {
                gameManager.shipUnlock.SpecShip1TextPrice.text = "Unlock: " + unlockShips[1].specShip.Price.ToString();
            }
        }

        if (i == 2)
        {
            //SHIP 3 SPEC
            gameManager.shipUnlock.SpecShip3TextLife.text = "Life " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.maxHealth.ToString() + "  /  " + gameManager.MaxHealthAllShip;
            gameManager.shipUnlock.SpecShip3TextMoveSpeed.text = "Speed " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.MoveSpeed.ToString() + "  /  " + gameManager.MaxSpeedAllShip;
            gameManager.shipUnlock.SpecShip3TextFireRate.text = "FireRate " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.fireRate.ToString() + "  /  " + gameManager.MaxFireRateAllShip;
            if (unlockShips[2].IsUnlock == false)
            {
                gameManager.shipUnlock.SpecShip1TextPrice.text = "Unlock: " + unlockShips[2].specShip.Price.ToString();
            }
        }

        if (i == 3)
        {
            //SHIP 4 SPEC
            gameManager.shipUnlock.SpecShip4TextLife.text = "Life " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.maxHealth.ToString() + "  /  " + gameManager.MaxHealthAllShip;
            gameManager.shipUnlock.SpecShip4TextMoveSpeed.text = "Speed " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.MoveSpeed.ToString() + "  /  " + gameManager.MaxSpeedAllShip;
            gameManager.shipUnlock.SpecShip4TextFireRate.text = "FireRate " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.fireRate.ToString() + "  /  " + gameManager.MaxFireRateAllShip;
            if (unlockShips[3].IsUnlock == false)
            {
                gameManager.shipUnlock.SpecShip1TextPrice.text = "Unlock: " + unlockShips[3].specShip.Price.ToString();
            }
        }

        if (i == 4)
        {
            //SHIP 5 SPEC
            gameManager.shipUnlock.SpecShip5TextLife.text = "Life " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.maxHealth.ToString() + "  /  " + gameManager.MaxHealthAllShip;
            gameManager.shipUnlock.SpecShip5TextMoveSpeed.text = "Speed " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.MoveSpeed.ToString() + "  /  " + gameManager.MaxSpeedAllShip;
            gameManager.shipUnlock.SpecShip5TextFireRate.text = "FireRate " + gameManager.changeShip.CurrentUnlockShipsSciprt.specShip.fireRate.ToString() + "  /  " + gameManager.MaxFireRateAllShip;
            if (unlockShips[4].IsUnlock == false)
            {
                gameManager.shipUnlock.SpecShip1TextPrice.text = "Unlock: " + unlockShips[4].specShip.Price.ToString();
            }
        }

    }

    public void Unlock()
    {
        if (i == 0)
        {
            unlockShips[0].IsShipBase = true;
            gameManager.shipUnlock.BaseShip = true;
        }
        if (i== 1)
        {
            if(gameManager.money >= unlockShips[1].specShip.Price)
            {
                unlockShips[1].IsUnlock = true;
                gameManager.shipUnlock.Ship2 = true;
                gameManager.money -= unlockShips[1].specShip.Price;
                PlayerPrefs.SetInt("shipUnlock.Ship2", gameManager.shipUnlock.Ship2 ? 1 : 0);
            }
            else
            {
                Debug.Log("Argent Manquant");
            }

        }
        if (i == 2)
        {
            if (gameManager.money >= unlockShips[2].specShip.Price)
            {
                unlockShips[2].IsUnlock = true;
                gameManager.shipUnlock.Ship3 = true;
                gameManager.money -= unlockShips[2].specShip.Price;
                PlayerPrefs.SetInt("shipUnlock.Ship3", gameManager.shipUnlock.Ship3 ? 1 : 0);
            }
            else
            {
                Debug.Log("Argent Manquant");
            }


        }
        if (i == 3)
        {
            if (gameManager.money >= unlockShips[3].specShip.Price)
            {
                unlockShips[3].IsUnlock = true;
                gameManager.shipUnlock.Ship4 = true;
                gameManager.money -= unlockShips[3].specShip.Price;
                PlayerPrefs.SetInt("shipUnlock.Ship4", gameManager.shipUnlock.Ship4 ? 1 : 0);
            }
            else
            {
                Debug.Log("Argent Manquant");
            }


        }
        if (i == 4)
        {
            if (gameManager.money >= unlockShips[4].specShip.Price)
            {
                unlockShips[4].IsUnlock = true;
                gameManager.shipUnlock.Ship5 = true;
                gameManager.money -= unlockShips[4].specShip.Price;
                PlayerPrefs.SetInt("shipUnlock.Ship5", gameManager.shipUnlock.Ship5 ? 1 : 0);
            }
            else
            {
                Debug.Log("Argent Manquant");
            }
        }
    }


    public void Select()
    {
        //Set CurrentSpaceShipSelect on click Button SelectShip
        if (i == 0)
        {
            CurrentSpaceShipSelect = i;
        }
        if (i == 1)
        {
            CurrentSpaceShipSelect = i;
        }
        if (i == 2)
        {
            CurrentSpaceShipSelect = i;
        }
        if (i == 3)
        {
            CurrentSpaceShipSelect = i;
        }
        if (i == 4)
        {
            CurrentSpaceShipSelect = i;

        }
        PlayerPrefs.SetInt("Current_Space_Ship_Select", CurrentSpaceShipSelect);
    }

    public void QuitSelectionShip()
    {
        //Faire spawn le vaisseaux séléctionné
        if (CurrentSpaceShipSelect == 0)
        {
            SpaceShip[0].SetActive(true);
            SpaceShip[1].SetActive(false);
            SpaceShip[2].SetActive(false);
            SpaceShip[3].SetActive(false);
            SpaceShip[4].SetActive(false);
        }
        if (CurrentSpaceShipSelect == 1)
        {
            SpaceShip[0].SetActive(false);
            SpaceShip[1].SetActive(true);
            SpaceShip[2].SetActive(false);
            SpaceShip[3].SetActive(false);
            SpaceShip[4].SetActive(false);
        }
        if (CurrentSpaceShipSelect == 2)
        {
            SpaceShip[0].SetActive(false);
            SpaceShip[1].SetActive(false);
            SpaceShip[2].SetActive(true);
            SpaceShip[3].SetActive(false);
            SpaceShip[4].SetActive(false);
        }
        if (CurrentSpaceShipSelect == 3)
        {
            SpaceShip[0].SetActive(false);
            SpaceShip[1].SetActive(false);
            SpaceShip[2].SetActive(false);
            SpaceShip[3].SetActive(true);
            SpaceShip[4].SetActive(false);
        }
        if (CurrentSpaceShipSelect == 4)
        {
            SpaceShip[0].SetActive(false);
            SpaceShip[1].SetActive(false);
            SpaceShip[2].SetActive(false);
            SpaceShip[4].SetActive(true);
        }
    }
}
