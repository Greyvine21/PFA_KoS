using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public EventSystem m_evSys;
    //public GameObject m_optionButton;
    //public GameObject m_playButton;
    private GameObject m_lastSelected;
    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;

    Resolution[] resolutions;
    public int Carte = 0;
    public int Credits = 0;

    public void PlayCarte ()
    {
        SceneManager.LoadScene(Carte);
    }
    public void goCredits ()
    {
        SceneManager.LoadScene(Credits);
    }

    public void QuitGame ()
    {
        Application.Quit();
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void LockCursor(bool b){
        if(b)     
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;

        print(b);
        Cursor.visible = b;
    }

    public void selectButton(GameObject go){
        m_evSys.SetSelectedGameObject(go);
    }

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        LockCursor(true);
    }

     void Update ()
    {
        if (m_evSys.currentSelectedGameObject == null)
            m_evSys.SetSelectedGameObject(m_lastSelected);
        else
            m_lastSelected = m_evSys.currentSelectedGameObject;
	}
}
