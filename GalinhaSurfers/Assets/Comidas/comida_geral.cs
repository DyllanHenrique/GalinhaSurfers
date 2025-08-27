using UnityEngine;

public class comida_geral : MonoBehaviour
{
    public comidaConfig config; // refer�ncia para o preset
    private int cliquesRestantes;
    public fome Fome;
    private Rigidbody rb;
    public Pontos ponto;
    private void Start()
    {
        cliquesRestantes = config != null ? config.cliquesParaComer : 1;

        if (Fome == null)
        {
            Fome = FindObjectOfType<fome>();
        }
        rb = gameObject.GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other) 
    {
        Destroy(gameObject);
    }
    private void Update() 
    { 
        rb.velocity = new Vector3(0, 0, -ponto.MetrosPorSegundo); 
    }

    void OnMouseDown()
    {
        cliquesRestantes--;

        if (cliquesRestantes <= 0)
            Comer();
    }

    void Comer()
    {
        if (Fome != null && config != null)
        {
            Fome.AdicionarFome(config.valorFome);
            Fome.AlterarMaxFome(config.valorMaxFome);
            if (config.temPoder)
            {
                AtivarPoder(config.nomeComida);
            }
        }
        Debug.Log($"COMEU {config?.nomeComida}");
        Destroy(gameObject);
    }
    void AtivarPoder(string nomePoder)
    {
        switch (nomePoder)
        {
            case "PILULA":
                Debug.Log("Poder" + nomePoder);
                string[] poderes = { "PIMENTA", "COOKIE" };
                int index = Random.Range(0, poderes.Length);
                string poderSorteado = poderes[index];
                Debug.Log("P�lula ativou aleatoriamente: " + poderSorteado);
                AtivarPoder(poderSorteado); 
                break;
            case "PIMENTA":
                Debug.Log("Poder" + nomePoder);
                Fome.AtivarPimenta();
                break;
            case "COOKIE":
                Debug.Log("Poder" + nomePoder);
                Fome.AtivarCookie();
                break;
            case "COGUMELOMAL":
                Debug.Log("Poder" + nomePoder);
                Fome.AtivarCogumeloMal();
                break;
            case "COGUMELOMALUCO":
                Debug.Log("Poder" + nomePoder);
                Fome.AtivarCogumeloMaluco();
                break;

            default:
                Debug.Log("Comida sem poder espec�fico");
                break;
        }
    }

}
