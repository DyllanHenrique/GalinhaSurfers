using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VFX;

public class LobbyManager : MonoBehaviour
{
    public Animator animatorGalinha;

    public GameObject smokeObject;

    private bool isTransitioning = false;

    public GameObject startObject;

    public ParticleSystem penasPartic;

    //MenuOpcoes
    public GameObject optionsMenu;

    //MenuHowToPlay
    public GameObject htpMenu;
    public Animator animatorA;
    public Animator animatorD;
    public Animator animatorGalin;
    public Animator animatorSpace;
    private Coroutine animationCoroutine;
    public Image imageMilho;
    public Image imageCoconut;
    public float fadeDuration = 1f;
    public Animator animatorChomp;

    void Start()
    {
        startObject.SetActive(true);

        smokeObject.SetActive(false);

        penasPartic.Stop();


        Debug.Log("Idle");
        animatorGalinha.Play("Idle");
        StartCoroutine(RandomIdleCycle());

        //MO
        optionsMenu.SetActive(false);
        htpMenu.SetActive(false);

        //MHTP
        if (imageMilho != null)
        {
            Image[] milhoImages = imageMilho.GetComponentsInChildren<Image>();
            foreach (var img in milhoImages)
            {
                Color c = img.color;
                c.a = 0f;
                img.color = c;
            }
        }

        if (imageCoconut != null)
        {
            Image[] coconutImages = imageCoconut.GetComponentsInChildren<Image>();
            foreach (var img in coconutImages)
            {
                Color c = img.color;
                c.a = 0f;
                img.color = c;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTransitioning
            && !optionsMenu.activeSelf && !htpMenu.activeSelf)
        {
            StartCoroutine(TransitionToGame());
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            optionsMenu.SetActive(true);
        }
    }

    IEnumerator RandomIdleCycle()
    {
        while (true)
        {
            float idleTime = Random.Range(2f, 10f);
            yield return new WaitForSeconds(idleTime);

            int randomIdle = Random.Range(1, 6);
            string animName = "Idle_" + randomIdle;

            animatorGalinha.Play(animName);

            yield return null;

            AnimatorStateInfo state = animatorGalinha.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(state.length);

            animatorGalinha.Play("Idle");
        }
    }

    IEnumerator TransitionToGame()
    {
        isTransitioning = true;

        startObject.SetActive(false);

        smokeObject.SetActive(true);

        penasPartic.Play();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Galinha_MasComProfundidade");
        asyncLoad.allowSceneActivation = false; 
        yield return new WaitForSeconds(0.2f);

        animatorGalinha.Play("Idle");

        yield return new WaitForSeconds(1.5f);
        asyncLoad.allowSceneActivation = true;
    }

    //OptionsMenu
    public void CloseOptionMenu()
    {
        optionsMenu.SetActive(false);
    }

    //HTPMenu
    public void OpenHTPMenu()
    {
        optionsMenu.SetActive(false);
        htpMenu.SetActive(true);
        StartHTPSequence();
    }

    public void CloseHTPMenu()
    {
        Debug.Log("Fechou");
        StopHTPSequence();
        htpMenu.SetActive(false);
    }

    public void StartHTPSequence()
    {
        Debug.Log("Comecou");
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        animationCoroutine = StartCoroutine(PlayHTPSequence());
    }

    public void StopHTPSequence()
    {
        animatorA.Rebind();
        animatorA.Update(0f);
        animatorA.Play("Idle_A", 0, 0f);

        animatorD.Rebind();
        animatorD.Update(0f);
        animatorD.Play("Idle_D", 0, 0f);

        animatorGalin.Rebind();
        animatorGalin.Update(0f);
        animatorGalin.Play("Idle", 0, 0f);

        animatorSpace.Rebind();
        animatorSpace.Update(0f);
        animatorSpace.Play("Idle_Space", 0, 0f);
    }

    private IEnumerator PlayHTPSequence()
    {
        while (htpMenu.activeSelf)
        {
            yield return new WaitForSeconds(0.5f);

            animatorA.SetTrigger("T_Press_A");
            animatorGalin.SetTrigger("T_ME");
            yield return new WaitForSeconds(0.5f);
            animatorA.SetTrigger("T_Idle_A");

            yield return new WaitForSeconds(1f);

            animatorD.SetTrigger("T_Press_D");
            animatorGalin.SetTrigger("T_EM");
            yield return new WaitForSeconds(0.5f);
            animatorD.SetTrigger("T_Idle_D");

            yield return new WaitForSeconds(1f);

            animatorD.SetTrigger("T_Press_D");
            animatorGalin.SetTrigger("T_MD");
            yield return new WaitForSeconds(0.5f);
            animatorD.SetTrigger("T_Idle_D");

            yield return new WaitForSeconds(1f);

            animatorA.SetTrigger("T_Press_A");
            animatorGalin.SetTrigger("T_DM");
            yield return new WaitForSeconds(0.5f);
            animatorA.SetTrigger("T_Idle_A");
            animatorGalin.SetTrigger("T_Idle");

            yield return new WaitForSeconds(0.5f);

            animatorChomp.SetTrigger("Comeu");
            animatorSpace.SetTrigger("T_Press_Space");
            animatorGalin.SetTrigger("T_Eating");
            StartCoroutine(Fade());
            yield return new WaitForSeconds(1.5f);
            animatorSpace.SetTrigger("T_Idle_Space");
            animatorGalin.SetTrigger("T_Idle");
            animatorChomp.SetTrigger("Cabo");

            yield return new WaitForSeconds(1f);

        }
    }

    IEnumerator Fade()
    {
        if (imageMilho == null || imageCoconut == null) yield break;

        Image[] milhoImages = imageMilho.GetComponentsInChildren<Image>();
        Image[] coconutImages = imageCoconut.GetComponentsInChildren<Image>();

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

            foreach (var img in milhoImages)
            {
                Color c = img.color;
                c.a = alpha;
                img.color = c;
            }

            foreach (var img in coconutImages)
            {
                Color c = img.color;
                c.a = alpha;
                img.color = c;
            }

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));

            foreach (var img in milhoImages)
            {
                Color c = img.color;
                c.a = alpha;
                img.color = c;
            }

            foreach (var img in coconutImages)
            {
                Color c = img.color;
                c.a = alpha;
                img.color = c;
            }

            yield return null;
        }
    }
}
