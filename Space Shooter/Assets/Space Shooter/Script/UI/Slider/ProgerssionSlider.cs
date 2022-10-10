using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgerssionSlider : MonoBehaviour
{
    [Header("Paramerter")]
    public float TimeSpeedDivide;
    public float Progress;
    [Space(10)]
    [Header("Ui")]
    public Slider SliderProgress;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Progress +=  Time.deltaTime / TimeSpeedDivide;
        SliderProgress.value = Progress;
    }
}
