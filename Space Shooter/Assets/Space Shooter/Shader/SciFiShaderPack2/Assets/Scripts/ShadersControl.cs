using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadersControl : MonoBehaviour {

    public float timer = 0;
    public float timerMax;
    float timerSciFi = 0;
    public bool ButtonPress = false;

    

	void Update () {
        if (ButtonPress)
        {
            timer += Time.deltaTime / 2;
            if (timer >= timerMax)
            {
                timer = 0;
                ButtonPress = false;
            }
        }

        Shader.SetGlobalFloat("_ShaderDisplacement", timer);
        Shader.SetGlobalFloat("_ShaderHologramDisplacement", 1 - timer);
        Shader.SetGlobalFloat("_ShaderDissolve", 1 - timer);

        if (timerSciFi > 1.2f)
            timerSciFi = 0;

        timerSciFi += Time.deltaTime / 3;

        
        Shader.SetGlobalFloat("_ShaderSciFi", timerSciFi);
    }


    public void ShaderController()
    {
        ButtonPress = true;

    }

}
