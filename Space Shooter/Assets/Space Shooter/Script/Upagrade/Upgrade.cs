using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{

    public GameObject[] UpagrdeBaseShip;
    public GameObject[] UpagrdeShip1;
    private GameManager gameManager;
    // Start is called before the first frame update

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    public void UprageGunShip()
    {
        if(gameManager.changeShip.CurrentSpaceShipSelect ==0)
        {
            if (gameManager.upgrade.UpagradeBaseShip[1] == true)
            {
                UpagrdeBaseShip[1].SetActive(true);
            }
            if (gameManager.upgrade.UpagradeBaseShip[1] == true && gameManager.upgrade.UpagradeBaseShip[1] == false)
            {
                UpagrdeBaseShip[2].SetActive(true);
            }
        }

        if (gameManager.changeShip.CurrentSpaceShipSelect == 1)
        {
            if (gameManager.upgrade.GunUpgardeShip1[1] == true)
            {
                UpagrdeShip1[1].SetActive(true);
                Debug.Log("Enter");
            }
            if (gameManager.upgrade.GunUpgardeShip1[1] == true && gameManager.upgrade.GunUpgardeShip1[1] == false)
            {
                UpagrdeShip1[2].SetActive(true);
            }
        }
    }


    public void SetFalseGameObject()
    {
        if (gameManager.changeShip.CurrentSpaceShipSelect == 0)
        {
            if (gameManager.upgrade.UpagradeBaseShip[1] == true)
            {
                UpagrdeBaseShip[1].SetActive(false);
                Debug.Log("Exit");
            }
            if (gameManager.upgrade.UpagradeBaseShip[1] == true && gameManager.upgrade.UpagradeBaseShip[1] == false)
            {
                UpagrdeBaseShip[2].SetActive(false);

            }
        }

        if (gameManager.changeShip.CurrentSpaceShipSelect == 1)
        {
            if (gameManager.upgrade.GunUpgardeShip1[1] == true)
            {
                UpagrdeShip1[1].SetActive(false);
            }
            if (gameManager.upgrade.GunUpgardeShip1[1] == true && gameManager.upgrade.GunUpgardeShip1[1] == false)
            {
                UpagrdeShip1[2].SetActive(false);
            }
        }
    }
}
