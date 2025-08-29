using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public Animator galinAnimator;
    public ParticleSystem transitionParticles; // <- adicione no Inspector as partículas da transição

    private bool isTransitioning = false;

    //MenuOpcoes
    public GameObject optionsMenu;

    //MenuHowToPlay
    public GameObject htpMenu;
    public Animator animatorA;
    public Animator animatorD;
    public Animator animatorGalin;
    private Coroutine animationCoroutine;

    void Start()
    {
        Debug.Log("Idle");
        galinAnimator.Play("Idle");
        StartCoroutine(RandomIdleCycle());

        //MO
        optionsMenu.SetActive(false);
        htpMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTransitioning)
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
            float idleTime = Random.Range(5f, 30f);
            yield return new WaitForSeconds(idleTime);

            int randomIdle = Random.Range(1, 6);
            string animName = "Idle_" + randomIdle;

            galinAnimator.Play(animName);

            yield return null;

            AnimatorStateInfo state = galinAnimator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(state.length);

            galinAnimator.Play("Idle");
        }
    }

    IEnumerator TransitionToGame()
    {
        isTransitioning = true;

        // Ativar partículas
        if (transitionParticles != null)
        {
            transitionParticles.Play();
        }

        // Espera 1 segundo e destrói a galinha
        yield return new WaitForSeconds(2f);
        if (galinAnimator != null)
        {
            Destroy(galinAnimator.gameObject);
        }

        // Espera mais 1 segundo (total 2s desde as partículas) e troca de cena
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Galinha_MasComProfundidade");
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
        htpMenu.SetActive(false);
        StopHTPSequence();
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
        Debug.Log("Terminou");
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }

        animatorA.Play("Idle_A");
        animatorD.Play("Idle_D");
        animatorGalin.Play("Idle");
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

            yield return new WaitForSeconds(1f);
        }
    }
}
