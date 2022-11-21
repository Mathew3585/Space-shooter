using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BulletEnnemis : MonoBehaviour
{
    [HideInInspector]
    public float bulletSpeed;
    [HideInInspector]
    public float dammage;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<Rigidbody>().velocity = -transform.forward * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 2);
    }



    /// <summary>
    /// Destroy Collision enter
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.transform.GetComponent<Ship_Controller>().ShieldActivate)
            {
                collision.transform.GetComponent<Ship_Controller>().shipStats.CurrentShield -= dammage;
                Destroy(gameObject);
            }

            if (collision.transform.GetComponent<Ship_Controller>().ShieldActivate == false)
            {
                collision.transform.GetComponent<Ship_Controller>().shipStats.CurrentHealth -= dammage;
                Destroy(gameObject);
            }
        }
            
        if (collision.gameObject.tag == "DestroyAsteroid")
        {
            Destroy(gameObject);
        }
    }


}
