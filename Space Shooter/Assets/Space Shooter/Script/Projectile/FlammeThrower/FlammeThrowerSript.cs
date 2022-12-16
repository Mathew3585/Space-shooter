using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlammeThrowerSript : MonoBehaviour
{
    private float damage;
    private BossSciript bossSciript;
    public Ship_Controller ship_Controller;

    // Start is called before the first frame update*

    private void Awake()
    {
        ship_Controller = FindObjectOfType<Ship_Controller>(); ;
        bossSciript = GameObject.FindObjectOfType<BossSciript>();
        damage = bossSciript.FlammeThorwerDamage;

    }

    // Update is called once per frame


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ship_Controller.shipStats.CurrentHealth -= damage;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ship_Controller.shipStats.CurrentHealth -= damage;
        }
    }

}
