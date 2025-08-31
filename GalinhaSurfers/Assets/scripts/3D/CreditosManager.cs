using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditosManager : MonoBehaviour
{
    public void AbrirCreditos()
    {
        SceneManager.LoadScene("Creditos");
    }

    public void FecharCreditos()
    {
        SceneManager.LoadScene("Lobyby");
    }
}
