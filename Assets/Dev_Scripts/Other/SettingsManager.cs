using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsUI;
    public bool isSettingsOpen, isDevModeOn, isWASD;
    public GameObject devCheck, WASD;

    public Camera mainCam, devCam;
    public Movement Movement;
   

    public void Start()
    {
        settingsUI = this.gameObject.transform.GetChild(0).gameObject;
        settingsUI.SetActive(false);
        devCheck.SetActive(false);
        devCam.gameObject.SetActive(false);
        WASD.gameObject.SetActive(false);

        isSettingsOpen = false;
        isDevModeOn = false;
        isWASD = false;

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

    public void MovementMode()
    {
        if (isWASD)
        {
            WASD.SetActive(false);
            Movement.useWASD = false;
            isWASD = false;
        }
        else if (!isWASD)
        {
            WASD.SetActive(true);
            Movement.useWASD = true;
            isWASD = true;
        }
    }
}
