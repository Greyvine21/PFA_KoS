using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LevelSelector : MonoBehaviour
{
    public EventSystem m_evSys;
    private GameObject m_lastSelected;
    public GameObject m_lvl1Button;
    public GameObject m_PlayButton;
    public GameObject mission;
    public GameObject backMenu;
    public int game;
    public int menu;

    public void Start()
    {
        m_evSys.SetSelectedGameObject(m_lvl1Button);
        mission.SetActive(false);
        backMenu.SetActive(true);
    }

    void Update ()
    {
        if (m_evSys.currentSelectedGameObject == null)
            m_evSys.SetSelectedGameObject(m_lastSelected);
        else
            m_lastSelected = m_evSys.currentSelectedGameObject;
	}

    public void Level1 ()
    {
        mission.SetActive(true);
        backMenu.SetActive(false);
        m_evSys.SetSelectedGameObject(m_PlayButton);
    }

    public void DisabledUI ()
    {
        mission.SetActive(false);
        backMenu.SetActive(true);
        m_evSys.SetSelectedGameObject(m_lvl1Button);
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
