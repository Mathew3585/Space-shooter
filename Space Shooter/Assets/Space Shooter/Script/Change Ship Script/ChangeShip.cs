using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeShip : MonoBehaviour
{

    public Button rightbutton;
    public Button leftbutton;
    public GameObject[] SpaceShip;
    public GameObject CurrentSpaceShip;
    public int i;

    // Start is called before the first frame update
    void Start()
    {
        i = PlayerPrefs.GetInt("BuyShip", i);
        CurrentSpaceShip = SpaceShip[i];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Onclicknext()
    {
        if (++i >= SpaceShip.Length)
        {
            i = 0;
        }

        CurrentSpaceShip.gameObject.SetActive(false);
        CurrentSpaceShip = SpaceShip[i];
        CurrentSpaceShip.gameObject.SetActive(true);


    }
    public void Onclickpast()
    {
        if (--i <= -1)
        {
            i = SpaceShip.Length -1;
        }

        CurrentSpaceShip.gameObject.SetActive(false);
        CurrentSpaceShip = SpaceShip[i];
        CurrentSpaceShip.gameObject.SetActive(true);
        
    }

    public void Select()
    {
        PlayerPrefs.SetInt("BuyShip",i);
    }

}
