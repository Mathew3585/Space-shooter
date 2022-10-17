using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateDammage : MonoBehaviour
{
    public Ennemis ennemis;
    public Astéroide_Controller astéroide_;
    public int Dammage = 200;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ennemis")
        {
            ennemis = gameObject.GetComponent<Ennemis>();
            ennemis.stats.currentHealth -= Dammage;
        }

        if(other.gameObject.tag == "Asteroid")
        {
            astéroide_ = other.gameObject.GetComponent<Astéroide_Controller>();
            astéroide_.stats.currentHealth -= Dammage;
        }
    }
}
