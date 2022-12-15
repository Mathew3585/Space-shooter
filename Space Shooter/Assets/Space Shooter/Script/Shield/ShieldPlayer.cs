using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ShieldPlayer : MonoBehaviour
{
    public Ship_Controller controller;
    public int Speed;
    public bool Life;
    public bool Sheild;
    public int lifeReg;
    // Start is called before the first frame update
    void Awake()
    {
        controller = GameObject.FindObjectOfType<Ship_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Position = transform.position;
        Position.z += Time.deltaTime * Speed;
        transform.position = Position;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (Life)
            {
                Debug.Log("Player life tuch");
                if (controller.shipStats.CurrentHealth < controller.shipStats.maxHealth)
                {
                    controller = GameObject.FindObjectOfType<Ship_Controller>();
                    controller.shipStats.CurrentHealth += lifeReg;
                    Destroy(gameObject);
                }
                else
                    Destroy(gameObject);
            }
            else if (Sheild)
            {
                controller = GameObject.FindObjectOfType<Ship_Controller>();
                controller.ShieldActivate = true;
                Destroy(gameObject);
            }

        }




    }
}
