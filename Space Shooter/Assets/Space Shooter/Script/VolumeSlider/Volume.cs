using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    [SerializeField]  AudioMixer mixer;
    [SerializeField]  Slider MusicSlider;
    [SerializeField]  Slider FxSlider;


    const string MIXER_MUSIC = "Music";
    const string MIXER_FX = "Fx";
    // Start is called before the first frame update
    void Awake()
    {
        MusicSlider.onValueChanged.AddListener(SetMusicVolume);
        FxSlider.onValueChanged.AddListener(SetFxVolume);
    }

    // Update is called once per frame
    void SetMusicVolume(float value)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }

    void SetFxVolume(float value)
    {
        mixer.SetFloat(MIXER_FX, Mathf.Log10(value) * 20);
    }
}
