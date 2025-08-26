using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerNugget : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    public Slider sliderMusic;
    public Slider sliderFx;
    public Slider sliderMaster;
    public float sliderValue1;
    public float sliderValue2;
    public float sliderValue3;

    private string sliderPrefsKeyMusicNugget = "SliderValueMusic";
    private string sliderPrefsKeyFxNugget = "SliderValueFx";
    private string sliderPrefsKeyMasterNugget = "SliderValueMaster";

    void Start()
    {
        LoadSliderValue();
    }

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
        SaveSliderValue(sliderMaster, sliderPrefsKeyMasterNugget);
    }

    public void SetFXVolume(float level)
    {
        audioMixer.SetFloat("fxVolume", Mathf.Log10(level) * 20f);
        SaveSliderValue(sliderFx, sliderPrefsKeyFxNugget);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
        SaveSliderValue(sliderMusic, sliderPrefsKeyMusicNugget);
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
        if (PlayerPrefs.HasKey(sliderPrefsKeyMusicNugget))
        {
            float savedValue = PlayerPrefs.GetFloat(sliderPrefsKeyMusicNugget);
            sliderMusic.value = savedValue;
            SetMusicVolume(savedValue);
        }

        if (PlayerPrefs.HasKey(sliderPrefsKeyFxNugget))
        {
            float savedValue = PlayerPrefs.GetFloat(sliderPrefsKeyFxNugget);
            sliderFx.value = savedValue;
            SetFXVolume(savedValue);
        }

        if (PlayerPrefs.HasKey(sliderPrefsKeyMasterNugget))
        {
            float savedValue = PlayerPrefs.GetFloat(sliderPrefsKeyMasterNugget);
            sliderMaster.value = savedValue;
            SetMasterVolume(savedValue);
        }
    }
}
