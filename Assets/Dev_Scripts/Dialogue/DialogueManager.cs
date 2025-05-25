using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Assertions.Must;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image portraitImage;
    public float typingSpeed;
    private Queue<string> sentences = new Queue<string>();
    private bool isTyping = false;
    private string currentSentence;
    private Coroutine typingCoroutine;
    public Button button;
    private bool waitingForZoom = false;
    public bool waitingForHarvest = false;
    public bool waitingForSelection = false;
    public Seed firstSeed;
    public GameObject nextButton;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingForZoom || waitingForHarvest || waitingForSelection)
        {
            nextButton.SetActive(false);
        }
        else
        {
            nextButton.SetActive(true);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f && waitingForZoom)
        {
            waitingForZoom = false;
            FindObjectOfType<HintManager>().HideScrollHint();
            DisplayNextSentence();
        }
        
        if (dialogueBox.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            OnButtonClick();
        }
    }

    public void OnButtonClick()
    {
        if (dialogueBox.activeSelf)
        {
            if (waitingForZoom || waitingForHarvest || waitingForSelection)
            {
                return;
            }

            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueBox.SetActive(true);
        nameText.text = dialogue.speakerName;
        portraitImage.sprite = dialogue.speakerImage;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void SkipToNextSentence()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentSentence;
            isTyping = false;
        }
    }


    public void DisplayNextSentence()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentSentence;
            isTyping = false;
            return;
        }

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        currentSentence = sentences.Dequeue();

        if (currentSentence.Contains("[ZOOM]") && !waitingForZoom)
        {
            waitingForZoom = true;
            currentSentence = currentSentence.Replace("[ZOOM]", "");
            dialogueText.text = currentSentence;
            FindObjectOfType<HintManager>().ShowScrollHint();
            return;
        }

        if (currentSentence.Contains("[PULLIVAN]"))
        {
            currentSentence = currentSentence.Replace("[PULLIVAN]", "");
            FindObjectOfType<HintManager>().ShowInteractHint();
        }

        if (currentSentence.Contains("[PAN]"))
        {
            currentSentence = currentSentence.Replace("[PAN]", "");
            StartCoroutine(PanCamera());
        }

        if (currentSentence.Contains("[HARVEST]") && !waitingForHarvest)
        {
            waitingForHarvest = true;
            currentSentence = currentSentence.Replace("[HARVEST]", "");
            dialogueText.text = currentSentence;
            FindObjectOfType<HintManager>().ShowInteractHint();
            return;
        }

        if (currentSentence.Contains("[SELECT]") && !waitingForSelection)
        {
            waitingForSelection = true;
            currentSentence = currentSentence.Replace("[SELECT]", "");
            dialogueText.text = currentSentence;
            FindObjectOfType<HintManager>().ShowSelectHint();
            return;
        }

        if (currentSentence.Contains("[END]"))
        {
            currentSentence = currentSentence.Replace("[END]", "");
            FindObjectOfType<Enemy_Spawner>().isTutorial = false;
        }

        typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
    }

    IEnumerator PanCamera()
    {
        Camera.main.GetComponent<CameraManager>().cameraControlEnabled = false;
        Camera.main.GetComponent<CameraManager>().isZoomedOut = false;
        FindObjectOfType<FollowManager>().inGarden = true;
        yield return new WaitForSeconds(4f);
        FindObjectOfType<FollowManager>().inGarden = false;
        Camera.main.GetComponent<CameraManager>().cameraControlEnabled = true;
        FindObjectOfType<Hotbar>().AddSeed(firstSeed);
        FindObjectOfType<HintManager>().ShowPlantHint();
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        dialogueBox.SetActive(false);
    }
}
