using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotatePreseantation : MonoBehaviour
{
    public float Speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Speed * Time.deltaTime, 0);
    }
}
