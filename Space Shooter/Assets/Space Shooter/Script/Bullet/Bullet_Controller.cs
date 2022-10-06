using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Controller : MonoBehaviour
{
    public float bulletSpeed;

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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Asteroid")
        {
            collision.transform.GetComponent<Astéroide_Controller>().stats.currentHealth -= dammage;
            Destroy(gameObject); 
        }
    }
}
