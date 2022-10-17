using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateDammage : MonoBehaviour
{
    public Ennemis ennemis;
    public Ast�roide_Controller ast�roide_;
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
            ast�roide_ = other.gameObject.GetComponent<Ast�roide_Controller>();
            ast�roide_.stats.currentHealth -= Dammage;
        }
    }
}
