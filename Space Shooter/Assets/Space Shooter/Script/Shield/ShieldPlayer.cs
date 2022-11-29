using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPlayer : MonoBehaviour
{
    public Ship_Controller controller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            controller = GameObject.FindObjectOfType<Ship_Controller>();
            controller.ShieldActivate = true;
            Destroy(gameObject);
        }
    }
}
