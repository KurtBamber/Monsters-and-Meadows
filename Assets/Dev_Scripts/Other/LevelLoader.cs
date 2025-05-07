using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1;

    public Sprite buttonClicked;
    public Sprite buttonUnClicked;

    public GameObject startButton;

    public void Start()
    {
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void LoadNextLevel()
    {
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void ButtonClicked()
    {
        StartCoroutine(changeButton());
    }
    IEnumerator changeButton()
    {
        yield return new WaitForSeconds(0.1f);
        startButton.GetComponent<Image>().sprite = buttonClicked;
        yield return new WaitForSeconds(0.5f);
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
