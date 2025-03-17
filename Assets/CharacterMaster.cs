using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterMaster : MonoBehaviour
{
    public Material characterMat;
    public TextMeshProUGUI nameText, inputName;

    public void Update()
    {
        nameText.text = inputName.text;
    }
}
