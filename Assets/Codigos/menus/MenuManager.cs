using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
   
    public GameObject controlsText; 
    public GameObject mainMenu;     

    void Start()
    {
        controlsText.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void ShowControls()
    {
        mainMenu.SetActive(false);
        controlsText.SetActive(true);
    }

    public void LoadTestLevel()
    {
        SceneManager.LoadScene("Level Test");
    }
   
    public void BackToMenu()
    {
        controlsText.SetActive(false);
        mainMenu.SetActive(true);
    }
}
