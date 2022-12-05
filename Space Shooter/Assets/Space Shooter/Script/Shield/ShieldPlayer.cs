using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ShieldPlayer : MonoBehaviour
{
    public Ship_Controller controller;
    public int Speed;
    // Start is called before the first frame update
    void Start()
    {
        
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
        if(collision.gameObject.tag == "Player")
        {
            controller = GameObject.FindObjectOfType<Ship_Controller>();
            controller.ShieldActivate = true;
            Destroy(gameObject);
        }
    }
}
