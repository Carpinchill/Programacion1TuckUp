using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DeathMenu : MonoBehaviour
{
    public GameObject mainMenu;

    public void Retry()
    {
        SceneManager.LoadScene(Movement.currentSceneName);
    }

    public void GiveUp()
    {
        SceneManager.LoadScene("Menu Principal");
    }

}
