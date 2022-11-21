using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using EZCameraShake;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

[System.Serializable]
public class ShipStats
{
    [Header("Health")]
    public float maxHealth;
    public float maxPower;
    [Header("Speed")]
    public float MoveSpeed;
    [Header("Shield")]
    public float MaxShield;
    public int MaxTimeShield;
    public float ShieldTime;
    [HideInInspector]
    public float CurrentHealth;
    public float CurrentShield;
    [Header("Current Power")]
    public float CurrentPower;
    [Header("Fire rate")]
    public float fireRate;

}

public class Ship_Controller : MonoBehaviour
{

    Rigidbody rb;

    public ShipStats shipStats;
    public CameraShaker cameraShaker;

    [Space(10)]
    [Header("Float")]
    public float titleAngle;

    [Space(10)]
    [Header("Bools")]
    public bool Isdead;
    public bool ShieldActivate;

    public GameObject Bullet;
    public GameObject Ultimates;
    public GameObject Shield;
    public GameObject Ultimategun;
    public GameObject explosionShip;

    [Space(10)]
    [Header("Transphorme List")]
    public Transform[] FirePointsBaseShip;
    public Transform[] FirePointsShip1;
    public Transform[] FirePointsShip2;
    public Transform[] FirePointsShip3;
    public Transform[] FirePointsShip4;
    public List <Transform> CurrentFirePoint;


    private float nextFire;
    private GameManager gameManager;
    private Bullet_Controller bulletController;
    private float CurrentIndexGun;


    private void Awake()
    {
        bulletController = Bullet.gameObject.GetComponent<Bullet_Controller>();
        rb = transform.GetComponent<Rigidbody>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        Shield.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (gameManager.Game)
        {
            if (gameManager.changeShip.CurrentSpaceShipSelect == 0)
            {
                Debug.Log("Ok vaisseaux 1");
                if (gameManager.upgrade.UpagradeBaseShip[0] == true)
                {
                    CurrentFirePoint.Add(FirePointsShip1[0]);
                    Debug.Log(CurrentFirePoint);
                }
                if (gameManager.upgrade.UpagradeBaseShip[1] == true)
                {
                    CurrentFirePoint.Add(FirePointsShip1[1]);
                }
                if (gameManager.upgrade.UpagradeBaseShip[2] == true)
                {
                    CurrentFirePoint.Add(FirePointsShip1[2]);
                }

                CurrentIndexGun = CurrentFirePoint.Count();
            }
            if (gameManager.changeShip.CurrentSpaceShipSelect == 1)
            {
                Debug.Log("Ok vaisseaux 1");
                if (gameManager.upgrade.GunUpgardeShip1[0] == true)
                {
                    CurrentFirePoint.Add(FirePointsShip1[0]);
                    CurrentFirePoint.Add(FirePointsShip1[1]);
                    Debug.Log(CurrentFirePoint);
                }
                if (gameManager.upgrade.GunUpgardeShip1[1] == true)
                {
                    CurrentFirePoint.Add(FirePointsShip1[1]);
                }
                if (gameManager.upgrade.GunUpgardeShip1[2] == true)
                {
                    CurrentFirePoint.Add(FirePointsShip1[2]);
                }

                CurrentIndexGun = CurrentFirePoint.Count();
            }
            if (gameManager.changeShip.CurrentSpaceShipSelect == 2)
            {
                Debug.Log("Ok vaisseaux 2");
                if (gameManager.upgrade.GunUpgardeShip2[0] == true)
                {
                    CurrentFirePoint.Add(FirePointsShip2[0]);
                    Debug.Log(CurrentFirePoint);
                }
                if (gameManager.upgrade.GunUpgardeShip2[1] == true)
                {
                    CurrentFirePoint.Add(FirePointsShip2[1]);
                }
                if (gameManager.upgrade.GunUpgardeShip2[2] == true)
                {
                    CurrentFirePoint.Add(FirePointsShip2[2]);
                }

                CurrentIndexGun = CurrentFirePoint.Count();
            }
            if (gameManager.changeShip.CurrentSpaceShipSelect == 3)
            {
                Debug.Log("Ok vaisseaux 3");
                if (gameManager.upgrade.GunUpgardeShip3[0] == true)
                {
                    CurrentFirePoint.Add(FirePointsShip3[0]);
                    Debug.Log(CurrentFirePoint);
                }
                if (gameManager.upgrade.GunUpgardeShip3[1] == true)
                {
                    CurrentFirePoint.Add(FirePointsShip3[1]);
                }
                if (gameManager.upgrade.GunUpgardeShip3[2] == true)
                {
                    CurrentFirePoint.Add(FirePointsShip3[2]);
                }

                CurrentIndexGun = CurrentFirePoint.Count();
            }
            if (gameManager.changeShip.CurrentSpaceShipSelect == 4)
            {
                Debug.Log("Ok vaisseaux 4");
                if (gameManager.upgrade.GunUpgardeShip4[0] == true)
                {
                    CurrentFirePoint.Add(FirePointsShip4[0]);
                    Debug.Log(CurrentFirePoint);
                }
                if (gameManager.upgrade.GunUpgardeShip4[1] == true)
                {
                    CurrentFirePoint.Add(FirePointsShip4[1]);
                }
                if (gameManager.upgrade.GunUpgardeShip4[2] == true)
                {
                    CurrentFirePoint.Add(FirePointsShip4[2]);
                }

                CurrentIndexGun = CurrentFirePoint.Count();
            }
        }



        shipStats.CurrentHealth = shipStats.maxHealth;

        nextFire = 1 / shipStats.fireRate;
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (gameManager.Game)
        {
            //Voir si le joueur et mort 
            if (shipStats.CurrentHealth <= 0)
            {
                Instantiate(explosionShip, transform.position, Quaternion.identity);
                Isdead = true;
                Destroy(gameObject);
            }


            //Ulti Activation/Tire
            if (shipStats.CurrentPower == 100)
            {
                Debug.Log("Ultimate is ready");
                if (Input.GetButtonDown("Ultimate"))
                {
                    gameManager.game.UltimateActive = true;
                    cameraShaker.ShakeOnce(4f, 4f, 4f, 4f);
                    GameObject VfxUltimateClone = Instantiate(Ultimates, Ultimategun.gameObject.transform);
                    Debug.Log(CurrentIndexGun);
                    Destroy(VfxUltimateClone, 3);
                    gameManager.game.UltimateActive = false;
                    shipStats.CurrentPower = 0;
                }
            }

            //Activation du shield
            if (ShieldActivate)
            {
                shipStats.ShieldTime += Time.deltaTime;
                Shield.SetActive(true);
                if (shipStats.ShieldTime >= shipStats.MaxTimeShield)
                {
                    shipStats.ShieldTime = 0;
                    shipStats.CurrentShield = 100;
                    Shield.SetActive(false);
                    ShieldActivate = false;
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Tires des projectiles
        if (gameManager.Game)
        {
            float moveRL = Input.GetAxis("Horizontal");
            float moveFB = Input.GetAxis("Vertical");

            Vector3 mouvement = new Vector3(moveRL, 0, moveFB);
            rb.velocity = mouvement * shipStats.MoveSpeed;

            rb.rotation = Quaternion.Euler(Vector3.forward * moveRL * -titleAngle);

            bool fireButton = Input.GetButton("Fire1");

            Collider[] shipCollider = transform.GetComponentsInChildren<Collider>();

            if(gameManager.changeShip.CurrentSpaceShipSelect == 0)
            {
                if (fireButton)
                {
                    nextFire -= Time.fixedDeltaTime;
                    if (nextFire <= 0)
                    {
                        for (int i = 0; i < CurrentIndexGun; i++)
                        {
                            GameObject bulletClone = Instantiate(Bullet, FirePointsBaseShip[i].position, Quaternion.identity);

                            for (int x = 0; x < shipCollider.Length; x++)
                            {
                                Physics.IgnoreCollision(bulletClone.transform.GetComponent<Collider>(), shipCollider[x]);
                            }
                        }
                        nextFire += 1 / shipStats.fireRate;
                    }
                }
            }
            if (gameManager.changeShip.CurrentSpaceShipSelect == 1)
            {
                if (fireButton)
                {
                    nextFire -= Time.fixedDeltaTime;
                    if (nextFire <= 0)
                    {
                        for (int i = 0; i < CurrentIndexGun; i++)
                        {
                            GameObject bulletClone = Instantiate(Bullet, FirePointsShip1[i].position, Quaternion.identity);

                            for (int x = 0; x < shipCollider.Length; x++)
                            {
                                Physics.IgnoreCollision(bulletClone.transform.GetComponent<Collider>(), shipCollider[x]);
                            }
                        }
                        nextFire += 1 / shipStats.fireRate;
                    }
                }
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameManager.Game)
        {
            if (collision.gameObject.tag == "Asteroid")
            {
                shipStats.CurrentHealth -= collision.transform.GetComponent<Astéroide_Controller>().stats.dammage;
                Destroy(collision.gameObject);
            }
        }
        
    }
}
