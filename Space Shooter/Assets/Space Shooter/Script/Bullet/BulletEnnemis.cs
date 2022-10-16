using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BulletEnnemis : MonoBehaviour
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



    /// <summary>
    /// Destroy Collision enter
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.GetComponent<Ship_Controller>().stats.CurrentHealth -= dammage;
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "BlockPalyer")
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "DestroyAsteroid")
        {
            Destroy(gameObject);

        }
    }


}
