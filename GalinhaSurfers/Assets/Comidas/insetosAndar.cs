using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class insetosAndar : MonoBehaviour
{
    private float chanceMove = 0.5f;
    private float tempoEspera = 2f;
    private void Start()
    {
        StartCoroutine(MoverPeriodicamente());
    }
    IEnumerator MoverPeriodicamente()
    {
        while (true)
        {
            yield return new WaitForSeconds(tempoEspera);
            if (Random.value < chanceMove)
            {
                MoverAdjacente();
            }
        }
    }

    void MoverAdjacente()
    {
        Vector3 posicao = transform.position;
        float xAtual = Mathf.Round(posicao.x);

        if (xAtual == 2f)
        {
            posicao.x = 0f; 
        }
        else if (xAtual == -2f)
        {
            posicao.x = 0f; 
        }
        else if (xAtual == 0f)
        {
            posicao.x = (Random.value < 0.5f) ? -2f : 2f;
        }

        transform.position = posicao;
        //Debug.Log("Inseto foi para: " + posicao.x);
    }

    public void Update()
    {
        Vector3 posicao = transform.position;
        if (posicao.x <= 2.5f && posicao.x > 1.7f)
        {
            posicao.x = 2f;
        }
        else if (posicao.x >= -2.4f && posicao.x < -1.5f)
        {
            posicao.x = -2f;
        }
        else posicao.x = 0f;
        transform.position = posicao;

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Frutas"))
        {
            Destroy(collision.gameObject);
            Debug.Log("Inseto comeu a fruta!");
        }
    }
}
