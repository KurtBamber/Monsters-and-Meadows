using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMaster : MonoBehaviour
{
    public TextMeshProUGUI nameText, inputName;

    [Header ("Important Player Info")]
    public static string Name;
    public string shownName;
    public static Material characterMat;

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            nameText.text = inputName.text;
        }

        if(SceneManager.GetActiveScene().buildIndex == 2 && nameText.text != null)
        {
            nameText = GameObject.Find("PlayerName").GetComponent<TextMeshProUGUI>();
            nameText.text = shownName;
        }

        Name = nameText.text;
        shownName = Name;
    }
}
