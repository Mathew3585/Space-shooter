using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LifeStats))]
public class PégaseStats : MonoBehaviour
{
    public LifeStats stats;

    public float timer;

    public float timerMax;

    public float titleAngle;
    public float Force;

    public int Speed;

    public int randDir;

    public Transform targetLeft;

    public Transform targetRight;

    public Transform rootObject;

    private Ship_Controller ship_Controller;
    // Start is called before the first frame update
    void Start()
    {
        ship_Controller = GameObject.FindGameObjectWithTag("Player").GetComponent<Ship_Controller>();
        stats.currentHealth = stats.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Position = rootObject.position;
        Position.z += Time.deltaTime * Speed;
        rootObject.position = Position;

        timer += Time.deltaTime;

        rootObject.position = Vector3.MoveTowards(rootObject.position, targetRight.position, Time.deltaTime * Speed);

        if (timer > timerMax)
        {
            // pick a new direction
            randDir = Random.Range(1, 3);
            Debug.Log(randDir);
            timer = 0;
        }
        // movement here
        // ....
        if (randDir == 1)
        {
            rootObject.position = Vector3.Lerp(rootObject.position, targetLeft.position, Time.deltaTime * Speed);
            Quaternion rotation = Quaternion.Euler(Vector3.forward * Force * -titleAngle);
            rootObject.rotation = Quaternion.Lerp(rootObject.rotation, rotation, Time.deltaTime);
            rootObject.position = new Vector3(rootObject.position.x, 0.8f, transform.position.z);
        }
        if (randDir == 2)
        {
            rootObject.position = Vector3.Lerp(rootObject.position, targetRight.position, Time.deltaTime * Speed);
            Quaternion rotation = Quaternion.Euler(Vector3.forward * Force * titleAngle);
            rootObject.rotation = Quaternion.Lerp(rootObject.rotation, rotation, Time.deltaTime);
            rootObject.position = new Vector3(rootObject.position.x, 0.8f, transform.position.z);
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
