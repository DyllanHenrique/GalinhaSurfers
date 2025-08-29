using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class tresDoisUm : MonoBehaviour
{
    public Image[] images;
    private float fadeDuration = 0.2f;
    private float scaleDuration = 1.5f;
    private float tempoVisivel = 0.5f;

    private void Start()
    {
        StartCoroutine(Sequencia());
    }

    IEnumerator Sequencia()
    {
        yield return new WaitForSeconds(2f);
        foreach (var img in images)
        {
            yield return StartCoroutine(Aparecer(img));
        }
    }

    IEnumerator Aparecer(Image img)
    {
        Color corOriginal = img.color;
        img.color = new Color(corOriginal.r, corOriginal.g, corOriginal.b, 0f);

        // Pega a escala atual do objeto
        Vector3 escalaOriginal = img.transform.localScale;

        // Fade in
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            img.color = new Color(corOriginal.r, corOriginal.g, corOriginal.b, alpha);
            yield return null;
        }

        // Escala multiplicativa
        t = 0f;
        Vector3 escalaInicial = escalaOriginal;
        Vector3 escalaFinal = escalaOriginal * 2f;
        while (t < scaleDuration)
        {
            t += Time.deltaTime;
            img.transform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, t / scaleDuration);
            yield return null;
        }

        // Tempo visível
        yield return new WaitForSeconds(tempoVisivel);

        // Fade out
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            img.color = new Color(corOriginal.r, corOriginal.g, corOriginal.b, alpha);
            yield return null;
        }

        // Reseta a escala
        img.transform.localScale = escalaOriginal;
    }
}
