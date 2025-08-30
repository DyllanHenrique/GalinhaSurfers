using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class tresDoisUm : MonoBehaviour
{
    public Image[] images;
    public float fadeDuration = 0.2f;
    public float scaleDuration = 1.5f;
    public float tempoVisivel = 0.5f;
    public bool TresDoisUmGO = false;
    public AudioSource audLargada;
    public AudioClip audio321;
    public AudioClip audioGo;   
    private void Start()
    {
        TresDoisUmGO = true;
        StartCoroutine(Sequencia());
    }

    IEnumerator Sequencia()
    {

        yield return new WaitForSeconds(1f);
        foreach (var img in images)
        {
            yield return StartCoroutine(Aparecer(img));
        }
        audLargada.clip = audioGo;
        audLargada.Play();
        TresDoisUmGO = false;
    }

    IEnumerator Aparecer(Image img)
    {
        audLargada.clip = audio321;
        audLargada.Play();
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

        // Tempo vis�vel
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
