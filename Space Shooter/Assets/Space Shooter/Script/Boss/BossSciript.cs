using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
[System.Serializable]


[RequireComponent(typeof(LifeStats))]
public class BossSciript : MonoBehaviour
{
    [Header("Stats")]
    public LifeStats stats;
    [Space(5)]
    [Header("Ints")]
    public int LifePhase1;
    public int LifePhase2;
    public int LifePhase3;

    [Header("Damage")]
    public float FireBallDamage;
    public float FireBallSpeed;
    [Space(10)]
    public float FlammeThorwerDamage;
    [Space(10)]
    public float FeatherDamage;
    public float FeatherSpeed;


    [Header("Time")]
    public int WaitTimeBetweenAttack;
    public float WaitTimeValue;


    [Space(5)]
    [Header("GameObject")]
    public GameObject FireBall;
    public GameObject FlammeThower;
    public GameObject FheaterFlamme;
    public GameObject explosionPrefabs;
    public GameObject RootObject;
    private Transform PostionStart;
    public BossMove ennemismove;
    public GameObject Portail;

    [Space(10)]
    [Header("List")]
    public Transform[] FirePoints;
    public Transform[] FheaterFlammePoints;
    private GameObject FlammeThowerclone;

    private Asteroid_Field field;   
    public GameManager gameManager;
    private BulletBoss bulletController;

    [Space(5)]
    [Header("Floats")]
    public float fireRate;

    private float nextFire;
    private float firepointlist;


    [Space(5)]
    [Header("Bools")]
    public bool isAlvie;
    public bool PositionStartValidate;
    public bool FlammeThorwerActivate;
    public bool FheaterFlammeActivate;
    public bool Attack2;
    public bool Attack3;


    private void Awake()
    {
        //Initalize Gameobject and bool 
        isAlvie = true;
        FlammeThorwerActivate = true;
        FheaterFlammeActivate = true;
        Attack2 = true;
        Attack3 = true;
        stats.currentHealth = stats.MaxHealth;
        bulletController = FireBall.gameObject.GetComponent<BulletBoss>();
        PostionStart = GameObject.FindGameObjectWithTag("BossPostion").GetComponent<Transform>();
        bulletController.dammage = stats.Damage;
        bulletController.bulletSpeed = stats.BulletSpeed;
        ennemismove.enabled = false;
        field = GameObject.FindObjectOfType<Asteroid_Field>();
        gameManager = FindObjectOfType<GameManager>();
    }
    // Start is called before the first frame update
    void Start()
    {


        firepointlist = FirePoints.Count();
        //Initalize Dommage and speed 
        bulletController.dammage = FireBallDamage;
        bulletController.bulletSpeed = FireBallSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (PositionStartValidate)
        {
            RootObject.gameObject.transform.position = Vector3.MoveTowards(RootObject.gameObject.transform.position, PostionStart.position, stats.Speed * Time.deltaTime);
        }

        if (RootObject.gameObject.transform.position == PostionStart.position)
        {
            PositionStartValidate = false;
        }

        if (stats.currentHealth <= 0)
        {
            Instantiate(explosionPrefabs, transform.position, Quaternion.identity);
            field.asteroidsClones.Remove(RootObject);
            field.enabled = false;
            gameManager.game.Progress = 100;
            gameManager.money += stats.MoneyDrop;
            Destroy(RootObject);
        }
    }

    private void FixedUpdate()
    {
        if(PositionStartValidate == false)
        {
            ennemismove.enabled = true;
            if (stats.currentHealth > LifePhase1)
            {
                Debug.Log("Phase 1 Boss");
                InitiateFireBall();
            }
            else if (stats.currentHealth > LifePhase2)
            {
                Debug.Log("Phase 2 Boss");
                AttackTotalPhase2();
            }
            else if (stats.currentHealth > LifePhase3)
            {
                Debug.Log("Phase 3 Boss");
                AttackTotalPhase3();
            }
        }
    }
    public void InitiateFireBall()
    {
        Collider[] shipCollider = transform.GetComponentsInChildren<Collider>();
        if (isAlvie)
        {
            if (WaitTimeValue <= WaitTimeBetweenAttack)
            {
                WaitTimeValue += Time.fixedDeltaTime;

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
            else if (WaitTimeValue >= WaitTimeBetweenAttack)
            {
                Debug.Log("Reste en cour timer Attack 1");
                WaitTimeValue = 0;
            }
        }
    }
    public void InitiateFlammethorwer()
    {
        Collider[] shipCollider = transform.GetComponentsInChildren<Collider>();
        if (isAlvie)
        {
            if (WaitTimeValue <= WaitTimeBetweenAttack)
            {
                WaitTimeValue += Time.fixedDeltaTime;

                if (FlammeThorwerActivate)
                {
                    FlammeThowerclone = Instantiate(FlammeThower, FirePoints[1]);
                    Debug.Log(FlammeThowerclone);
                    FlammeThorwerActivate = false;
                    Debug.Log(FlammeThorwerActivate);
                }
            } 
            else if(WaitTimeValue >= WaitTimeBetweenAttack)
            {
                Debug.Log("Reste en cour timer Attack2");
                WaitTimeValue = 0;
                Attack2 = false;
                Destroy(FlammeThowerclone);
            }

        }
    }


    public void AttackTotalPhase2()
    {
        if (Attack2 == true)
        {
            WaitTimeValue += Time.fixedDeltaTime;
            InitiateFlammethorwer();
        }
        else if (Attack2 == false)
        {
            InitiateFireBall();
            WaitTimeValue += Time.fixedDeltaTime;
            if (WaitTimeValue >= WaitTimeBetweenAttack)
            {
                WaitTimeValue = 0;
                Attack2 = true;
                FlammeThorwerActivate = true;
                Debug.Log("Acitvation fLAMME tHOROWER");
            }

        }
    }
    public void InitiateFheaterFlamme()
    {
        Collider[] shipCollider = transform.GetComponentsInChildren<Collider>();
        if (isAlvie)
        {
            if (WaitTimeValue <= WaitTimeBetweenAttack)
            {
                Debug.Log("Attack3");
                WaitTimeValue += Time.fixedDeltaTime;

                if (FheaterFlammeActivate)
                {

                    for (int x = 0; x < FheaterFlammePoints.Length; x++)
                    {
                        GameObject FlammeThowerclone = Instantiate(FheaterFlamme, FheaterFlammePoints[x].position, FheaterFlamme.gameObject.transform.rotation);
                    }

                    FheaterFlammeActivate = false;
                }
            }
            if (WaitTimeValue >= WaitTimeBetweenAttack)
            {
                Debug.Log("Reste en cour timer Attack3");
                WaitTimeValue = 0;
                Attack3 = false;

            }

        }
    }
    public void AttackTotalPhase3()
    {
        if (Attack3 == true)
        {
            InitiateFheaterFlamme();
            WaitTimeValue += Time.fixedDeltaTime;
            if (WaitTimeValue >= WaitTimeBetweenAttack)
            {
                WaitTimeValue = 0;
                Attack2 = true;
                FlammeThorwerActivate = true;
                Debug.Log("Acitvation Fheather Flamme");
            }

        }
        if (Attack2 == true)
        {
            WaitTimeValue += Time.fixedDeltaTime;
            InitiateFlammethorwer();
        }
        if (Attack2 == false)
        {
            InitiateFireBall();
            WaitTimeValue += Time.fixedDeltaTime;
            if (WaitTimeValue >= WaitTimeBetweenAttack)
            {
                WaitTimeValue = 0;
                Attack3 = true;
                FlammeThorwerActivate = true;
                FheaterFlammeActivate = true;
                Debug.Log("Acitvation Flamme Thrower");
            }

        }

    }
}
