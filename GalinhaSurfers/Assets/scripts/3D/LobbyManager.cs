using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public Animator galinAnimator;
    public ParticleSystem transitionParticles; // <- adicione no Inspector as partículas da transição

    private bool isTransitioning = false;

    void Start()
    {
        Debug.Log("Idle");
        galinAnimator.Play("Idle");
        StartCoroutine(RandomIdleCycle());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTransitioning)
        {
            StartCoroutine(TransitionToGame());
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
}
