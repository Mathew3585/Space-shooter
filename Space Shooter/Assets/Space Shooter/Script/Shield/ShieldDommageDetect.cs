using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class ShieldDommageDetect : MonoBehaviour
{
    public GameObject shieldRipples;
    private VisualEffect shieldRippleVfx;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "BulletEnemis" || collision.gameObject.tag == "Ennemis" || collision.gameObject.tag == "Asteroid")
        {
            var ripples = Instantiate(shieldRipples, transform) as GameObject;
            shieldRippleVfx = ripples.GetComponent<VisualEffect>();
            shieldRippleVfx.SetVector3("Sphere Center", collision.contacts[0].point);

            Destroy(ripples, 2);
        }
    }
}
