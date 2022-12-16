using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class UltimateDammage : MonoBehaviour
{
    private Ennemis MinautorLife;
    private Hydre HydreLife;
    private LifeStats lifeStats;
    private GameObject pégaseStats;
    public int Dammage = 200;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ennemis"))
        {
            MinautorLife = other.gameObject.GetComponent<Ennemis>();
            MinautorLife.Dead();
        }
        if(other.gameObject.CompareTag("Asteroid"))
        {
            lifeStats = other.gameObject.GetComponent<LifeStats>();
            lifeStats.currentHealth -= Dammage;
        }
        if(other.gameObject.CompareTag("Pégas"))
        {
            Debug.Log("Pégas");
            pégaseStats = other.gameObject;
            Destroy(pégaseStats);
        }
        if(other.gameObject.CompareTag("Hydre"))
        {
            HydreLife = other.gameObject.GetComponent<Hydre>();
            HydreLife.Dead();
        }
        else if (other.gameObject.CompareTag("Boss"))
        {
            lifeStats = other.gameObject.GetComponent<LifeStats>();
            lifeStats.currentHealth -= Dammage;
        }
    }
}
