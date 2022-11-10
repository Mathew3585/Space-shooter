    using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class TutoManager : MonoBehaviour
{
    //Fonction
    public KeyCode NextpopupTuto;
    [Header("Popup"), Tooltip("Cette Variable permet de gère le nombre de popup")]
    public GameObject[] popUps;
    public int popUpIndex;
    public float WaitTime;
    public float MaxWaitTime;
    public int TimeSpeedDivide; 
    [Header("Text tuto")]
    public TextMeshProUGUI[] text;
    public string[] TextTuto;
    public string[] Commende;

    [Space(10)]
    [Header("Bools")]
    public bool Front;
    public bool Back;
    public bool Left;
    public bool Right;

    [Header("List Audio"), Tooltip("Cette Variable permet de renseigner la List Audio")]
    public List<AudioClip> ListAudio;

    private float _timer = 0;
    private GameManager gameManager;
    private Asteroid_Field asteroid_Field;  

    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        asteroid_Field = GameObject.FindObjectOfType<Asteroid_Field>();
        asteroid_Field.enabled = false;
        text[0].text = TextTuto[0].ToString();
        text[1].text = TextTuto[1].ToString();
        text[2].text = TextTuto[2].Replace("{0}", Commende[0]).Replace("{1}", Commende[1]).ToString();
        text[3].text = TextTuto[3].Replace("{2}", Commende[2]).Replace("{3}", Commende[3]).ToString();
        text[4].text = TextTuto[4].Replace("{4}", Commende[4]).ToString();
        text[5].text = TextTuto[5].ToString();
    }

    //Update
    private void Update()
    {
        PupupIndexParamter();
        PopupParamter();
        if(WaitTime > MaxWaitTime)
        {
            WaitTime = MaxWaitTime;
        }

    }

    public void PupupIndexParamter()
    {
        //Afficher le popIndex avec +1
        for (int i = 0; i < popUps.Length; i++)
        {
            if (i == popUpIndex)
            {
                popUps[i].SetActive(true);

            }
            else
            {
                popUps[i].SetActive(false);
            }
        }

    }

    //Régler les parametre des popups
    public void PopupParamter()
    {
        Debug.Log("I : " + popUpIndex);

        if (popUpIndex == 0)
        {
            if (Input.GetKeyDown(NextpopupTuto))
            {
                asteroid_Field.enabled = false;
                popUpIndex++;
            }
        }

        else if (popUpIndex == 1)
        {
            if (Input.GetButton("AccepteTuto"))
            {
                popUpIndex++;
            }
            if (Input.GetButton("RefuseTuto"))
            {
                popUps[1].SetActive(false);
                gameObject.SetActive(false);
            }
        }
        else if (popUpIndex == 2)
        {
            WaitTime += Time.deltaTime / TimeSpeedDivide;
            if (WaitTime >= MaxWaitTime)
            {
                Debug.Log("Ok timer");
                if (Input.GetAxis("Vertical") < 0)
                {
                    Debug.Log("front");
                    Front = true;
                }
            }

            if(WaitTime >= MaxWaitTime)
            {

            }

        }
        else if (popUpIndex == 3 )
        {
            WaitTime += Time.deltaTime / TimeSpeedDivide;
            if (WaitTime >= MaxWaitTime)
            {
                Debug.Log("Ok timer");
                if (Input.GetAxis("Horizontal") > 0)
                {
                    Debug.Log("Left");
                    Left = true;
                }
                if (Input.GetAxis("Horizontal") < 0 && Left)
                {
                    Debug.Log("Back");
                    Right = true;
                    popUpIndex++;
                }
            }
        }
        else if (popUpIndex == 4)
        {
            if (Input.GetKeyDown(NextpopupTuto))
            {
                popUpIndex++;
            }

        }
        else if (popUpIndex == 5)
        {
            WaitTime = 0;
            MaxWaitTime = 10;
            asteroid_Field.enabled = true;
            if (WaitTime >= MaxWaitTime)
            {
                popUpIndex++;
            }

        }
    }

}
