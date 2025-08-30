using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject MenuDePause;
    public void Start()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause(); 
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
    public void Contiue()
    {
        MenuDePause.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void Resetando()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Lobyby");
    }
        public void Creditos()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Creditos");
    }
}
