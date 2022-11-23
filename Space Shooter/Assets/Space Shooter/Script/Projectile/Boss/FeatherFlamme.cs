using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FeatherFlamme : MonoBehaviour
{
    public float dammage;
    public float t;
    public float speed;

    private Transform target;
    private Ship_Controller ship_Controller;
    private GameObject Boss;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Boss = GameObject.FindGameObjectWithTag("Boss");
        ship_Controller = GameObject.FindGameObjectWithTag("Player").GetComponent<Ship_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 a = transform.position;
        Vector3 b = target.position;
        transform.position = Vector3.MoveTowards(a, Vector3.Lerp(a, b, t), speed);
        transform.LookAt(target);
        Physics.IgnoreCollision(Boss.transform.GetComponent<Collider>(), gameObject.transform.GetComponent<Collider>());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (ship_Controller.ShieldActivate)
            {
                ship_Controller.shipStats.CurrentShield -= dammage;
                Destroy(gameObject);
            }
            else
            ship_Controller.shipStats.CurrentHealth -= dammage;
            Destroy(gameObject);
        }

        if(collision.gameObject.tag == "Bullet")
        {
            Destroy(gameObject);
        }
    }
}
