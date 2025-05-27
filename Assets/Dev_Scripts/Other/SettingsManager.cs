using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.WebCam;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsUI;
    public bool isSettingsOpen, isDevModeOn;
    public GameObject devCheck;

    public Camera mainCam;
    public Movement Movement;
    int oldMask;


    public void Start()
    {
        settingsUI = this.gameObject.transform.GetChild(0).gameObject;
        settingsUI.SetActive(false);
        devCheck.SetActive(false);
        oldMask = Camera.main.cullingMask;

        isSettingsOpen = false;
        isDevModeOn = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isSettingsOpen)
        {
            OpenPanel();
        }
    }

    public void OpenPanel()
    {
        isSettingsOpen = true;
        settingsUI.SetActive(true);
    }

    public void ClosePanel()
    {
        if (isSettingsOpen)
        {
            settingsUI.SetActive(false);
            isSettingsOpen = false;
        }
    }

    public void DevMode()
    {
        if (isDevModeOn)
        {
            devCheck.SetActive(false);
            Camera.main.cullingMask = oldMask;
            mainCam.gameObject.SetActive(true);

            isDevModeOn = false;
        }
        else if (!isDevModeOn)
        {
            devCheck.SetActive(true);
            Camera.main.cullingMask = -1;

            isDevModeOn = true;
        }
    }

    public void Quit()
    {
        SceneManager.LoadScene("Start Scene");
    }
}
