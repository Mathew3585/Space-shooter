using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using EZCameraShake;

[System.Serializable]
public class ShipStats
{
    [Header("Health")]
    public float maxHealth;
    public float maxPower;
    [HideInInspector]
    public float CurrentHealth;
    [Header("Speed")]
    public float CurrentPower;
    [Header("Fire rate")]
    public float fireRate;

}


public class Ship_Controller : MonoBehaviour
{

    Rigidbody rb;

    public ShipStats stats;
    public CameraShaker cameraShaker;

    public GameObject bullet;
    public GameObject Ultimates;
    public Transform[] FirePoints;
    public GameObject Ultimategun;
    private float nextFire;
    private GameManager gameManager;
    private Bullet_Controller bulletController;
    private float CurrentIndexGun;


    public float MoveSpeed;
    public  float titleAngle;

    public GameObject explosionShip;



    // Start is called before the first frame update
    void Start()
    {
        bulletController = bullet.gameObject.GetComponent<Bullet_Controller>();
        rb = transform.GetComponent<Rigidbody>();
        CurrentIndexGun = FirePoints.Count();

        stats.CurrentHealth = stats.maxHealth;

        nextFire = 1 / stats.fireRate;
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private void Update()
    {

        //Voir si le joueur et mort 
        if(stats.CurrentHealth <= 0)
        {
            Instantiate(explosionShip, transform.position , Quaternion.identity);
            Destroy(gameObject);
        }


        //Ulti Activation/Tire
        if(stats.CurrentPower == 100)
        {
            Debug.Log("Ultimate is ready");
            if (Input.GetButtonDown("Ultimate"))
            {
                gameManager.game.UltimateActive = true;
                cameraShaker.ShakeOnce(4f,4f,4f,4f);
                GameObject VfxUltimateClone = Instantiate(Ultimates, Ultimategun.gameObject.transform);
                Debug.Log(CurrentIndexGun);
                Destroy(VfxUltimateClone,3);
                gameManager.game.UltimateActive = false;
                stats.CurrentPower = 0;
            }
        }
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
                for (int i = 0; i < CurrentIndexGun; i++)
                {
                    GameObject bulletClone = Instantiate(bullet, FirePoints[i].position, Quaternion.identity);

                    for (int x = 0; x <  shipCollider.Length; x++)
                    {
                          Physics.IgnoreCollision(bulletClone.transform.GetComponent<Collider>(), shipCollider[x]);
                    }
                }
                nextFire += 1 / stats.fireRate;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Asteroid")
        {
            stats.CurrentHealth -= collision.transform.GetComponent<Astéroide_Controller>().stats.dammage;
            Destroy(collision.gameObject);
        }
    }
}
