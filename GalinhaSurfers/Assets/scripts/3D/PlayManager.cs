using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public Animator characterAnimator;

    public float delayBeforeAppear = 0.3f;

    void Start()
    {
        characterAnimator.gameObject.SetActive(false);

        StartCoroutine(SpawnSequence());
    }

    IEnumerator SpawnSequence()
    {
        yield return new WaitForSeconds(delayBeforeAppear);


        characterAnimator.gameObject.SetActive(true);

        characterAnimator.Play("Walk");
    }
}
