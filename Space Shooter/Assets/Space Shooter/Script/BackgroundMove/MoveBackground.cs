using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    public float SpeedScrolling;


    // Update is called once per frame
    void Update()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        Material mat = mr.sharedMaterial;
        Vector2 offest = mat.mainTextureOffset;

        offest.y += Time.deltaTime / SpeedScrolling;
        mat.mainTextureOffset = offest;
    }
}
