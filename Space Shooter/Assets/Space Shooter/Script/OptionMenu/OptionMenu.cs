using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    [SerializeField]  AudioMixer mixer;
    [SerializeField]  Slider MasterSlider;
    [SerializeField]  Slider MusicSlider;
    [SerializeField]  Slider FxSlider;


    const string MIXER_MUSIC = "Music";
    const string MIXER_MASTER = "Master";
    const string MIXER_FX = "Fx";
    // Start is called before the first frame update
    void Awake()
    {
        MusicSlider.value = 1;
        MasterSlider.value = 1;
        FxSlider.value = 1;
        MasterSlider.onValueChanged.AddListener(SetMasterVolume);
        MusicSlider.onValueChanged.AddListener(SetMusicVolume);
        FxSlider.onValueChanged.AddListener(SetFxVolume);
        MusicSlider.value = PlayerPrefs.GetFloat("volumeMusic");
        FxSlider.value = PlayerPrefs.GetFloat("volumeFx");
        MasterSlider.value = PlayerPrefs.GetFloat("masterMusic");
        SetMusicVolume(MusicSlider.value);
        SetFxVolume(FxSlider.value);
        SetMasterVolume(MasterSlider.value);
    }

    void SetMasterVolume(float value)
    {
        float VolumeValue = Mathf.Log10(value) * 20;
        mixer.SetFloat(MIXER_MASTER, VolumeValue);
        PlayerPrefs.SetFloat("masterMusic", value);
    }
    void SetMusicVolume(float value)
    {
        float VolumeValue = Mathf.Log10(value) * 20;
        mixer.SetFloat(MIXER_MUSIC, VolumeValue);
        PlayerPrefs.SetFloat("volumeMusic", value);
    }

    void SetFxVolume(float value)
    {
        float VolumeValue = Mathf.Log10(value) * 20;
        mixer.SetFloat(MIXER_FX, VolumeValue);
        PlayerPrefs.SetFloat("volumeFx", value);
    }

    public void low(bool quality)
    {
        if (quality)
        {
            QualitySettings.SetQualityLevel(1);
        }
    }

    public void medium(bool quality)
    {
        if (quality)
        {
            QualitySettings.SetQualityLevel(2);
        }
    }
    public void Hight(bool quality)
    {
        if (quality)
        {
            QualitySettings.SetQualityLevel(3);
        }
    }
    public void Ultra(bool quality)
    {
        if (quality)
        {
            QualitySettings.SetQualityLevel(4);
        }
    }


}
