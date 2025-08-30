using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerCredit : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    public Slider sliderMusic;
    public Slider sliderFx;
    public Slider sliderMaster;
    public float sliderValue1;
    public float sliderValue2;
    public float sliderValue3;

    private string sliderPrefsKeyMusicCredit = "SliderValueMusic";
    private string sliderPrefsKeyFxCredit = "SliderValueFx";
    private string sliderPrefsKeyMasterCredit = "SliderValueMaster";

    void Start()
    {
        LoadSliderValue();
    }

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
        SaveSliderValue(sliderMaster, sliderPrefsKeyMasterCredit);
    }

    public void SetFXVolume(float level)
    {
        audioMixer.SetFloat("fxVolume", Mathf.Log10(level) * 20f);
        SaveSliderValue(sliderFx, sliderPrefsKeyFxCredit);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
        SaveSliderValue(sliderMusic, sliderPrefsKeyMusicCredit);
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
        if (PlayerPrefs.HasKey(sliderPrefsKeyMusicCredit))
        {
            float savedValue = PlayerPrefs.GetFloat(sliderPrefsKeyMusicCredit);
            sliderMusic.value = savedValue;
            SetMusicVolume(savedValue);
        }

        if (PlayerPrefs.HasKey(sliderPrefsKeyFxCredit))
        {
            float savedValue = PlayerPrefs.GetFloat(sliderPrefsKeyFxCredit);
            sliderFx.value = savedValue;
            SetFXVolume(savedValue);
        }

        if (PlayerPrefs.HasKey(sliderPrefsKeyMasterCredit))
        {
            float savedValue = PlayerPrefs.GetFloat(sliderPrefsKeyMasterCredit);
            sliderMaster.value = savedValue;
            SetMasterVolume(savedValue);
        }
    }
}
