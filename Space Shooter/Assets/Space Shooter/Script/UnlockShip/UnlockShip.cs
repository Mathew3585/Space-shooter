using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class SpecShip
{
    [Header("Health")]
    public float maxHealth;
    [Header("Speed")]
    public float MoveSpeed;
    [Header("Fire rate")]
    public float fireRate;
    [Header("Power")]
    public float maxPower;
    [Header("Price")]
    public int Price;
}
public class UnlockShip : MonoBehaviour
{
    public SpecShip specShip;
    public Material[] Holograme;
    public Material[] materials;

    private List<Material[]> materials2 = new List<Material[]>();

    public MeshRenderer[] renderers;
    public float waitTime;
    public bool IsShipBase;
    public bool IsUnlock;
    private ChangeShip changeShip;
    private GameManager gameManager;
    public Ship_Controller ship_Controller;
    public int i;

    private void Awake()
    {
        ship_Controller = GameObject.FindObjectOfType<Ship_Controller>();
        changeShip = GameObject.FindObjectOfType<ChangeShip>();
        gameManager = GameObject.FindObjectOfType<GameManager>();

        //Spec ship Unlock to Ship ship controller
        ship_Controller.shipStats.maxHealth = specShip.maxHealth;
        ship_Controller.shipStats.MoveSpeed = specShip.MoveSpeed;
        ship_Controller.shipStats.fireRate = specShip.fireRate;
        ship_Controller.shipStats.maxPower = specShip.maxPower;
    }


    // Si c est le vaisueax de base alors unlock et desactiver/ recuper le render du GameObject
    void Start()
    {
        if (gameObject.tag == "Ship2")
        {
            {
                IsUnlock = gameManager.shipUnlock.Ship2;
            }
        }

        if (gameObject.tag == "Ship3")
        {
            {
                IsUnlock = gameManager.shipUnlock.Ship3;
            }
        }

        if (gameObject.tag == "Ship4")
        {
            {
                IsUnlock = gameManager.shipUnlock.Ship4;
            }
        }

        if (gameObject.tag == "Ship5")
        {
            {
                IsUnlock = gameManager.shipUnlock.Ship5;
            }
        }

        if (gameManager.GreekMenu)
        {
            if (IsShipBase == true)
            {
                changeShip.Unlockbutton.SetActive(false);
                return;
            }
            if (IsShipBase == false)
            {
                changeShip.Unlockbutton.SetActive(true);
            }
            if (IsUnlock == false)
            {
                materials2.Clear();
                foreach (Renderer renderermats in renderers)
                {
                    List<Material> materials = Holograme.ToList();
                    int j = renderermats.materials.Length - 1;
                    for (int i = 0; i < j; ++i) materials.AddRange(Holograme);
                    materials2.Add(renderermats.materials);
                    renderermats.materials = materials.ToArray();
                    Debug.Log(renderermats.material);
                }

            }
        }
    }
    
    // Si Il est acheter
    void Update()
    {
        if (gameManager.GreekMenu)
        {
            // Si ce n est pas Unlock alors affiche le button Unlock
            if (IsUnlock == false)
            {
                changeShip.Unlockbutton.SetActive(true);
                changeShip.SelectShip.SetActive(false);
            }

            //Button Select disparait quand on click dessus pour le vaiseau de base
            if (IsShipBase == true)
            {
                if (changeShip.CurrentSpaceShipSelect == 0 && changeShip.i == 0)
                {
                    changeShip.Unlockbutton.SetActive(false);
                    changeShip.SelectShip.SetActive(false);
                }
                if (changeShip.CurrentSpaceShipSelect != 0 && changeShip.i == 0)
                {
                    changeShip.Unlockbutton.SetActive(false);
                    changeShip.SelectShip.SetActive(true);
                }
            }
            //Button Select disparait quand on click dessus 
            if (IsUnlock == true)
            {
                if (changeShip.CurrentSpaceShipSelect == 1 && changeShip.i == 1)
                {
                    changeShip.Unlockbutton.SetActive(false);
                    changeShip.SelectShip.SetActive(false);
                }
                if (changeShip.CurrentSpaceShipSelect != 1 && changeShip.i == 1)
                {
                    changeShip.Unlockbutton.SetActive(false);
                    changeShip.SelectShip.SetActive(true);
                }
                if (changeShip.CurrentSpaceShipSelect == 2 && changeShip.i == 2)
                {
                    changeShip.Unlockbutton.SetActive(false);
                    changeShip.SelectShip.SetActive(false);
                }
                if (changeShip.CurrentSpaceShipSelect != 2 && changeShip.i == 2)
                {
                    changeShip.Unlockbutton.SetActive(false);
                    changeShip.SelectShip.SetActive(true);
                }
                if (changeShip.CurrentSpaceShipSelect == 3 && changeShip.i == 3)
                {
                    changeShip.Unlockbutton.SetActive(false);
                    changeShip.SelectShip.SetActive(false);
                }
                if (changeShip.CurrentSpaceShipSelect != 3 && changeShip.i == 3)
                {
                    changeShip.Unlockbutton.SetActive(false);
                    changeShip.SelectShip.SetActive(true);
                }
                if (changeShip.CurrentSpaceShipSelect == 4 && changeShip.i == 4)
                {
                    changeShip.Unlockbutton.SetActive(false);
                    changeShip.SelectShip.SetActive(false);
                }
                if (changeShip.CurrentSpaceShipSelect != 4 && changeShip.i == 4)
                {
                    changeShip.Unlockbutton.SetActive(false);
                    changeShip.SelectShip.SetActive(true);
                }
                StartCoroutine(FxToMaterial());
            }
        }



        if(gameObject.tag == "Ship2") {
            {
                IsUnlock = gameManager.shipUnlock.Ship2;
            } 
        }

        if (gameObject.tag == "Ship3")
        {
            {
                IsUnlock = gameManager.shipUnlock.Ship3;
            }
        }

        if (gameObject.tag == "Ship4")
        {
            {
                IsUnlock = gameManager.shipUnlock.Ship4;
            }
        }

        if (gameObject.tag == "Ship5")
        {
            {
                IsUnlock = gameManager.shipUnlock.Ship5;
            }
        }

    }

    //Couroutine pour le fx
    IEnumerator FxToMaterial()
    {
/*        foreach (Renderer renderermats in renderers)
        {
            List<Material> materials = Holograme.ToList();
            int j = renderermats.materials.Length - 1;
            for (int i = 0; i < j; ++i) materials.AddRange(Holograme);
            renderermats.materials = materials.ToArray();
        }*/
        yield return new WaitForSeconds(waitTime);


        Debug.Log(renderers.Length);
        Debug.Log(materials2.Count);

        for (int i = 0; i < renderers.Length && i < materials2.Count; ++i)
        {
            Renderer renderer = renderers[i];
            renderer.materials = materials2[i];
        }
    }
}
