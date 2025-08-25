using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject MenuDePause;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause(); //o mundo é divino as vezes
        }
    }
    public void TogglePause()
    {
        if (isPaused)
        {
            MenuDePause.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
        else
        {
            MenuDePause.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
    }
    public void Quit()
    {
        Debug.Log("ISSO AQUI ESTA CANCELADO");
        UnityEditor.EditorApplication.isPlaying = false; //a vida me deu um ataque cardiaco
        Application.Quit();
    }
}
