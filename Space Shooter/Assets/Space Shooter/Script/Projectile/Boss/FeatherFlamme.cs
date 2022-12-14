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
    private BossSciript BossScript;
    // Start is called before the first frame update
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Boss = GameObject.FindGameObjectWithTag("Boss");
        ship_Controller = target.GetComponent<Ship_Controller>();
        dammage = Boss.gameObject.GetComponentInChildren<BossSciript>().FeatherDamage;
        speed = Boss.gameObject.GetComponentInChildren<BossSciript>().FeatherSpeed;
        BossScript = Boss.GetComponent<BossSciript>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 a = transform.position;
        Vector3 b = target.position;
        transform.position = Vector3.MoveTowards(a, Vector3.Lerp(a, b, t), speed);
        transform.LookAt(target);
        Physics.IgnoreCollision(Boss.transform.GetComponentInChildren<Collider>(), gameObject.transform.GetComponent<Collider>());
        if (BossScript.isAlvie == false)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(gameObject);
        }
    }
}
