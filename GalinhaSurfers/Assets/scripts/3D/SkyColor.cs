using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyColor : MonoBehaviour
{
    public Light directionalLight;

    public Color colorDay = new Color(0f, 0f, 0f);
    public Color colorEvening = new Color(0f, 0f, 0f);
    public Color colorNight = new Color(0f, 0f, 0f);

    public float durationDtoE = 31f;
    public float durationEtoN = 21f;
    public float durationNtoD = 21f;

    private void Start()
    {
        if (directionalLight == null)
            directionalLight = GetComponent<Light>();

        StartCoroutine(CycleColors());
    }

    IEnumerator CycleColors()
    {
        while (true)
        {
            yield return StartCoroutine(ChangeColor(colorDay, colorEvening, durationDtoE));
            yield return StartCoroutine(ChangeColor(colorEvening, colorNight, durationEtoN));
            yield return StartCoroutine(ChangeColor(colorNight, colorDay, durationNtoD));
        }
    }

    IEnumerator ChangeColor(Color from, Color to, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            directionalLight.color = Color.Lerp(from, to, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        directionalLight.color = to;
    }
}
