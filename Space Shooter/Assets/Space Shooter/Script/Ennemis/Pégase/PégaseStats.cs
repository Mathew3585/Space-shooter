using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LifeStats))]
public class PégaseStats : MonoBehaviour
{
    public LifeStats stats;
    [Space(10)]
    public float TimeTranslate;
    public float speed;
    public GameObject explosionPrefabs; 
    private Transform target;
    private Ship_Controller ship_Controller;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        ship_Controller = GameObject.FindGameObjectWithTag("Player").GetComponent<Ship_Controller>();
        stats.currentHealth = stats.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 a = transform.position;
        Vector3 b = target.position;
        transform.position = Vector3.MoveTowards(a, Vector3.Lerp(a, b, TimeTranslate), speed);
        transform.LookAt(target);
        if (stats.currentHealth <= 0 )
        {
            Instantiate(explosionPrefabs, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (ship_Controller.ShieldActivate)
            {
                ship_Controller.shipStats.CurrentShield -= stats.Damage;
                Destroy(gameObject);
            }
            else
                ship_Controller.shipStats.CurrentHealth -= stats.Damage;
                Destroy(gameObject);
        }
    }
}
