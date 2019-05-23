using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public Image fondNoir;
    public Image mission;

    public GameObject play;
    public GameObject backCarte;
    public GameObject backMenu;

    public int game;
    public int menu;

    public void Start()
    {
        mission.enabled = false;
        fondNoir.enabled = false;
        play.SetActive(false);
        backCarte.SetActive(false);
        backMenu.SetActive(true);
    }

    public void Level1 ()
    {
        mission.enabled = true;
        fondNoir.enabled = true;
        play.SetActive(true);
        backCarte.SetActive(true);
        backMenu.SetActive(false);
    }

    public void DisabledUI ()
    {
        mission.enabled = false;
        fondNoir.enabled = false;
        play.SetActive(false);
        backCarte.SetActive(false);
        backMenu.SetActive(true);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(game);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(menu);
    }
}
