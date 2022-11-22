using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class Boss_Stats
{ 
    [Header("Life")]
    public float MaxHealth;
    public float currentHealth;
    [Header("Damage")]
    public float FireBallDamage;
    public float FireBallSpeed;
    public float FlammeThorwerDamge;
    public float WindWaveDamage;
}


public class BossSciript : MonoBehaviour
{
    [Header("Stats")]
    public Boss_Stats stats;
    [Space(5)]
    [Header("Ints")]
    public int MoneyDrop;
    public int LifePhase1;
    public int LifePhase2;
    public int LifePhase3;
    public int WaitFlammeThorwerValue;
    public int waitTime;

    [Space(5)]
    [Header("GameObject")]
    public GameObject FireBall;
    public GameObject FlammeThower;
    public GameObject WindWave;
    public GameObject explosionPrefabs;
    public Transform[] FirePoints;
    public GameObject FlammeThowerclone;
    private Asteroid_Field field;   
    private GameManager gameManager;
    private BulletEnnemis bulletController;

    [Space(5)]
    [Header("Floats")]
    public float fireRate;
    public float WaitFlammeThorwer;
    private float nextFire;
    private float firepointlist;


    [Space(5)]
    [Header("Bools")]
    public bool isAlvie;
    private bool FlammeThorwerActivate;
    public bool WindWaveActivate;
    public bool Attack2;


    // Start is called before the first frame update
    void Start()
    {
        //Initalize Gameobject and bool 
        isAlvie = true;
        FlammeThorwerActivate = true;
        Attack2 = true;
        stats.currentHealth = stats.MaxHealth;
        bulletController = FireBall.gameObject.GetComponent<BulletEnnemis>();
        field = GameObject.FindObjectOfType<Asteroid_Field>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        firepointlist = FirePoints.Count();

        //Initalize Dommage and speed 
        bulletController.dammage = stats.FireBallDamage;
        bulletController.bulletSpeed = stats.FireBallSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.currentHealth <= 0)
        {
            Instantiate(explosionPrefabs, transform.position, Quaternion.identity);
            //field.asteroidsClones.Remove(gameObject);
            gameManager.money += MoneyDrop;
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if(stats.currentHealth > LifePhase1)
        {
            Debug.Log("Phase 1 Boss");
            InitiateFireBall();
        }
       else  if (stats.currentHealth > LifePhase2)
        {
            Debug.Log("Phase 2 Boss");
            AttackTotalPhase2();
        }
        else if (stats.currentHealth > LifePhase3)
        {
            Debug.Log("Phase 3 Boss");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "DestroyAsteroid" || collision.gameObject.tag == "Player")
        {
            field.asteroidsClones.Remove(gameObject);
            isAlvie = false;
            Destroy(gameObject);

        }
    }
    public void InitiateFireBall()
    {
        Collider[] shipCollider = transform.GetComponentsInChildren<Collider>();
        if (isAlvie)
        {
            if (WaitFlammeThorwer <= WaitFlammeThorwerValue)
            {
                nextFire -= Time.fixedDeltaTime;
                if (nextFire <= 0)
                {
                    GameObject bulletClone = Instantiate(FireBall, FirePoints[0].position, FirePoints[0].rotation);

                    for (int x = 0; x < shipCollider.Length; x++)
                    {
                        Physics.IgnoreCollision(bulletClone.transform.GetComponent<Collider>(), shipCollider[x]);
                    }
                    nextFire += 1 / fireRate;
                }
  
            }
        }
    }
    public void InitiateFlammethorwer()
    {
        Collider[] shipCollider = transform.GetComponentsInChildren<Collider>();
        if (isAlvie)
        {
            if (WaitFlammeThorwer <= WaitFlammeThorwerValue)
            {
                WaitFlammeThorwer += Time.fixedDeltaTime;

                if (FlammeThorwerActivate)
                {
                    FlammeThowerclone = Instantiate(FlammeThower, FirePoints[1]);
                    FlammeThorwerActivate = false;
                    Debug.Log(FlammeThorwerActivate);
                }
            } 
            else if(WaitFlammeThorwer >= WaitFlammeThorwerValue)
            {
                Debug.Log("Reste en cour timer Attack2");
                WaitFlammeThorwer = 0;
                Attack2 = false;
                Destroy(FlammeThowerclone);
            }

        }
    }


    public void AttackTotalPhase2()
    {
        if (Attack2 == true)
        {
            WaitFlammeThorwer += Time.fixedDeltaTime;
            InitiateFlammethorwer();
        }
        else if (Attack2 == false)
        {
            InitiateFireBall();
            WaitFlammeThorwer += Time.fixedDeltaTime;
            if (WaitFlammeThorwer >= WaitFlammeThorwerValue)
            {
                WaitFlammeThorwer = 0;
                Attack2 = true;
                FlammeThorwerActivate = true;
                Debug.Log("Acitvation fLAMME tHOROWER");
            }

        }
    }
}
