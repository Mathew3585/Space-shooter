using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ship_Controller : MonoBehaviour
{

    Rigidbody rb;

    public GameObject bullet;
    public Transform[] FirePoints = new Transform[2];
    public float fireRate;
    private float nextFire;

    public float MoveSpeed;
    public  float titleAngle;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();

        nextFire = 1 / fireRate;   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveRL = Input.GetAxis("Horizontal");
        float moveFB = Input.GetAxis("Vertical");

        Vector3 mouvement = new Vector3(moveRL, 0, moveFB)  ;
        rb.velocity = mouvement *MoveSpeed;

        rb.rotation =Quaternion.Euler(Vector3.forward*moveRL* -titleAngle);

        bool fireButton = Input.GetButton("Fire1");

        Collider[] shipCollider = transform.GetComponentsInChildren<Collider>();

        if (fireButton)
        {
            nextFire -= Time.fixedDeltaTime;  
            if(nextFire <= 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    GameObject bulletClone = Instantiate(bullet, FirePoints[i].position, Quaternion.identity);

                    for (int x = 0; x <  shipCollider.Length; x++)
                    {
                          Physics.IgnoreCollision(bulletClone.transform.GetComponent<Collider>(), shipCollider[x]);
                    }
                }
                nextFire += 1 / fireRate;
            }
        }
    }
}
