using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public GameObject[] prefabs;
    public int QntdSpawnSorteado;
    public Transform[] spawns;


    // Start is called before the first frame update
    void Start()
    {
        spawns = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            spawns[i] = transform.GetChild(i);
        }
        SpawnarItens();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SpawnarItens()
    {
        QntdSpawnSorteado = 5;
        for (int i = 0; i < QntdSpawnSorteado; i++)
        {
            int spawnEscolhido = Random.Range(0,5);
            Instantiate(prefabs[0], spawns[spawnEscolhido].position, transform.rotation);
        }
    }
}
