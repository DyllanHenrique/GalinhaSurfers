using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aranha : MonoBehaviour
{
    [Header("Somente para Aranha")]
    public bool armada = false;

    public Sprite spriteNormal;
    public Sprite spriteArmada;
    private SpriteRenderer sr;
    private float chanceArmada = 0.3f;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = spriteNormal;
        armada = false;
        StartCoroutine(TrocarSprite());
    }
    IEnumerator TrocarSprite()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            float sorteio = Random.value;
            if (sorteio < chanceArmada)
            {
                armada = true;
                sr.sprite = spriteArmada;
            }
            else
            {
                armada = false;
                sr.sprite = spriteNormal;
            }
        }
    }
    public bool EstaArmada()
    {
        return armada; 
    }
}
