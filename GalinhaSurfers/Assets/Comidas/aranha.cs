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
            bool novaArmada = sorteio < chanceArmada;

            if (novaArmada != armada)
            {
                armada = novaArmada;

                float duracao = 0.5f;
                float elapsed = 0f;
                bool spriteTrocado = false;
                float rotacaoTotal = 0f;

                while (elapsed < duracao)
                {
                    float delta = Time.deltaTime;
                    elapsed += delta;

                    // rota incremental: 360° / duracao * delta
                    float rotY = (360f / duracao) * delta;
                    transform.Rotate(0, rotY, 0);
                    rotacaoTotal += rotY;

                    // troca o sprite quando passar metade da rotação (180°)
                    if (!spriteTrocado && rotacaoTotal >= 180f)
                    {
                        spriteTrocado = true;
                        sr.sprite = armada ? spriteArmada : spriteNormal;
                    }

                    yield return null;
                }

                // garante que completou a rotação exatamente 360°
                transform.Rotate(0, 360f - rotacaoTotal, 0);
            }
        }
    }
    public bool EstaArmada()
    {
        return armada; 
    }
}
