using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerLoby : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    public Slider sliderMusic;
    public Slider sliderFx;
    public Slider sliderMaster;
    public float sliderValue1;
    public float sliderValue2;
    public float sliderValue3;

    private string sliderPrefsKeyMusicLoby = "SliderValueMusic";
    private string sliderPrefsKeyFxLoby = "SliderValueFx";
    private string sliderPrefsKeyMasterLoby = "SliderValueMaster";

    void Start()
    {
        LoadSliderValue();
    }

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
        SaveSliderValue(sliderMaster, sliderPrefsKeyMasterLoby);
    }

    public void SetFXVolume(float level)
    {
        audioMixer.SetFloat("fxVolume", Mathf.Log10(level) * 20f);
        SaveSliderValue(sliderFx, sliderPrefsKeyFxLoby);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
        SaveSliderValue(sliderMusic, sliderPrefsKeyMusicLoby);
    }

    public void OnSliderValueChanged(Slider slider, string prefsKey)
    {
        SaveSliderValue(slider, prefsKey);
    }

    private void SaveSliderValue(Slider slider, string prefsKey)
    {
        PlayerPrefs.SetFloat(prefsKey, slider.value);
        PlayerPrefs.Save();
    }

    private void LoadSliderValue()
    {
        if (PlayerPrefs.HasKey(sliderPrefsKeyMusicLoby))
        {
            float savedValue = PlayerPrefs.GetFloat(sliderPrefsKeyMusicLoby);
            sliderMusic.value = savedValue;
            SetMusicVolume(savedValue);
        }

        if (PlayerPrefs.HasKey(sliderPrefsKeyFxLoby))
        {
            float savedValue = PlayerPrefs.GetFloat(sliderPrefsKeyFxLoby);
            sliderFx.value = savedValue;
            SetFXVolume(savedValue);
        }

        if (PlayerPrefs.HasKey(sliderPrefsKeyMasterLoby))
        {
            float savedValue = PlayerPrefs.GetFloat(sliderPrefsKeyMasterLoby);
            sliderMaster.value = savedValue;
            SetMasterVolume(savedValue);
        }
    }
}
