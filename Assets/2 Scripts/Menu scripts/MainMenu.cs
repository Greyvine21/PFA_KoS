using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int game = 0;

    public void PlayCarte ()
    {
        SceneManager.LoadScene(game);
    }

    public void QuitGame ()
    {
        Application.Quit();
    }
}
