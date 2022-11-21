using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Controller : MonoBehaviour
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
        if (collision.gameObject.tag == "Asteroid")
        {
            collision.transform.GetComponent<Astéroide_Controller>().stats.currentHealth -= dammage;
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "DestroyAsteroid")
        {
            Destroy(gameObject);

        }
        if (collision.gameObject.tag == "Ennemis")
        {
            collision.transform.GetComponent<Ennemis>().stats.currentHealth -= dammage;
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Boss")
        {
            collision.transform.GetComponent<BossSciript>().stats.currentHealth -= dammage;
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "BlockPalyer")
        {
            Destroy(gameObject);
        }
    }
}
