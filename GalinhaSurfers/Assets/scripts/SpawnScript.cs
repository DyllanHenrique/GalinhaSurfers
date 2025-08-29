using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public GameObject[] prefabs;
    public float[] comidaChances;
    public int QntdSpawnSorteado;
    public Transform[] spawns;
    public int IntervaloSpawn;
    public Pontos pontosdacena;
    public bool morreu = false;

    // Start is called before the first frame update
    void Start()
    {
        spawns = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            spawns[i] = transform.GetChild(i);
        }
        StartCoroutine(SpawnLoop());
    }

    public IEnumerator SpawnLoop()
    {
        while (!morreu)
        {
            SpawnarItens();
            yield return new WaitForSeconds(IntervaloSpawn);
        }
    }
    private void SpawnarItens()
    {
        List<int> spawnsOcupados = new List<int>();
        QntdSpawnSorteado = Random.Range(1,4);
        for (int i = 0; i < QntdSpawnSorteado; i++)
        {
            int spawnEscolhido;
            do 
            {
                spawnEscolhido = Random.Range(0, spawns.Length);
            }
            while (spawnsOcupados.Contains(spawnEscolhido));
            spawnsOcupados.Add(spawnEscolhido);
            int prefabEscolhido = SortearComida();
            GameObject foodSpawn = Instantiate(prefabs[prefabEscolhido], spawns[spawnEscolhido].position, transform.rotation);
            Rigidbody rb = foodSpawn.GetComponent<Rigidbody>();
            comida_geral script = foodSpawn.GetComponent<comida_geral>();
            script.ponto = pontosdacena;
        }
    }
    private int SortearComida()
    {
        float total = 0f;
        foreach(float prob in comidaChances)
        {
            total += prob;
        }
        float sorteio = Random.Range(0f,total);
        float acumulado = 0f;
        for (int i = 0; i < comidaChances.Length; i++)
        {
            acumulado += comidaChances[i];
            if (sorteio < acumulado)
            {
                return i;
            }
        }
        return comidaChances.Length - 1;
    }
}