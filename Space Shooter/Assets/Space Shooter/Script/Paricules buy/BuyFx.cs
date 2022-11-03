using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BuyFx : MonoBehaviour
{
    [Header("Bools")]
    public bool BaseShip;
    public bool Ship1;
    public bool Ship2;
    public bool Ship3;
    public bool Ship4;

    [Space(10)]
    [Header("Level Upgrade")]
    public bool Upgarde1;
    public bool Upgarde2;

    public Material[] materials;
    public float waitTime;
    GameManager gameManager;
    Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        renderer = GetComponent<Renderer>();
        renderer.enabled = true;
        if (BaseShip)
        {
            if(Upgarde1 && gameManager.upgrade.UpagradeBaseShip[1])
            {
                renderer.material = materials[1];
            }
            if (Upgarde2 && gameManager.upgrade.UpagradeBaseShip[2])
            {
                renderer.material = materials[1];
            }
        }
        renderer.material = materials[2];
        Debug.Log("hOLOGRAME");
    }



    public void BuyUpagradeGun()
    {
        if (gameManager.upgradeScript.UpagrdeBaseShip[0] == false)
        {
            StartCoroutine(FxToMat());
        }

        if (gameManager.upgradeScript.UpagrdeBaseShip[0] == false)
        {
            StartCoroutine(FxToMat());
        }
        if (gameManager.upgradeScript.UpagrdeShip1[0] == false)
        {
            StartCoroutine(FxToMat());
        }

        if (gameManager.upgradeScript.UpagrdeShip1[1] == false)
        {
            StartCoroutine(FxToMat());
        }
    }

    IEnumerator FxToMat()
    {
        renderer.material = materials[0];
        yield return new WaitForSeconds(waitTime);
        renderer.material = materials[1];
    }
}
