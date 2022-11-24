using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PégaseStats : MonoBehaviour
{
    public Ennemis_Stats ennemis_Stats;
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
        ennemis_Stats.currentHealth = ennemis_Stats.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 a = transform.position;
        Vector3 b = target.position;
        transform.position = Vector3.MoveTowards(a, Vector3.Lerp(a, b, TimeTranslate), speed);
        transform.LookAt(target);
        if (ennemis_Stats.currentHealth <= 0 )
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
                ship_Controller.shipStats.CurrentShield -= ennemis_Stats.dammage;
                Destroy(gameObject);
            }
            else
                ship_Controller.shipStats.CurrentHealth -= ennemis_Stats.dammage;
                Destroy(gameObject);
        }
    }
}
