using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFieldOfView : MonoBehaviour
{
    private Camera fov;
    public GameObject Particules;
    public float speed = 10;
    public float speedreste = 10;
    public float DestFov = 150;
    public float DeafaultFov = 50;
    public bool Reste;

    void Start()
    {
        fov = GameObject.FindObjectOfType<Camera>();
        Particules.SetActive(false);
    }

    void Update()
    {
        if(fov.fieldOfView <= DestFov && Reste == false)
        {
            fov.fieldOfView = Mathf.MoveTowards(Camera.main.fieldOfView, DestFov, Time.deltaTime * speed);

        }
        if (fov.fieldOfView == DestFov)
        {
            fov.fieldOfView = 30;
            Reste = true;
        }

        if (Reste)
        {
            fov.fieldOfView = Mathf.MoveTowards(Camera.main.fieldOfView, DeafaultFov, Time.deltaTime * speedreste);
            Particules.SetActive(true);
        }

    }
}
