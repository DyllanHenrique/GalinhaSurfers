using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public GameObject[] prefabs;
    public int QntdSpawnSorteado;
    public Transform[] spawns;
    public int IntervaloSpawn;
    public Pontos pontosdacena;


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
        while (true)
        {
            SpawnarItens();
            yield return new WaitForSeconds(IntervaloSpawn);
        }
    }
    private void SpawnarItens()
    {
        List<int> spawnsOcupados = new List<int>();
        QntdSpawnSorteado = Random.Range(1,6);
        for (int i = 0; i < QntdSpawnSorteado; i++)
        {
            int spawnEscolhido;
            do 
            {
                spawnEscolhido = Random.Range(0, spawns.Length);
            }
            while (spawnsOcupados.Contains(spawnEscolhido));
            spawnsOcupados.Add(spawnEscolhido);
            GameObject foodSpawn = Instantiate(prefabs[0], spawns[spawnEscolhido].position, transform.rotation);
            Rigidbody rb = foodSpawn.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(0,0,-5) * 3;
            comida_geral script = foodSpawn.GetComponent<comida_geral>();
            script.ponto = pontosdacena;
        }
    }
}