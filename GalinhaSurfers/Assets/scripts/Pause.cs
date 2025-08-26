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
            GameObject[] frutas = GameObject.FindGameObjectsWithTag("Frutas");
            foreach (GameObject fruta in frutas)
            {
                Collider2D col = fruta.GetComponent<Collider2D>();
                if (col != null)
                    col.enabled = true; 
            }
            MenuDePause.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
        else
        {
            GameObject[] frutas = GameObject.FindGameObjectsWithTag("Frutas");
            foreach (GameObject fruta in frutas)
            {
                Collider2D col = fruta.GetComponent<Collider2D>();
                if (col != null)
                    col.enabled = false; 
            }
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
