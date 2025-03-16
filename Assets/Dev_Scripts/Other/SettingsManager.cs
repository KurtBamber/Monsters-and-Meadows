using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsUI;
    public bool isSettingsOpen, isDevModeOn;
    public GameObject devCheck;

    public Camera mainCam, devCam;
   

    public void Start()
    {
        settingsUI = this.gameObject.transform.GetChild(0).gameObject;
        settingsUI.SetActive(false);
        devCheck.SetActive(false);
        devCam.gameObject.SetActive(false);

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
            devCam.gameObject.SetActive(false);
            mainCam.gameObject.SetActive(true);

            isDevModeOn = false;
        }
        else if (!isDevModeOn)
        {
            devCheck.SetActive(true);
            devCam.gameObject.SetActive(true);
            mainCam.gameObject.SetActive(false);

            isDevModeOn = true;
        }
    }
}
