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
    public bool waitingForInspection = false;
    public Seed firstSeed;
    public bool firstHarvest = true;
    public GameObject nextButton;
    private bool wasActive = false;
    private bool firstTime = true;
    private HintManager hintManager;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
        hintManager = FindObjectOfType<HintManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<InspectorManager>().inspectorUI.activeSelf && dialogueBox.activeSelf)
        {
            dialogueBox.SetActive(false);
            wasActive = true;

            if (firstTime)
            {
                hintManager.ShowCloseInspectorHint();
            }
        }

        if (wasActive && !FindObjectOfType<InspectorManager>().inspectorUI.activeSelf)
        {
            wasActive = false;
            dialogueBox.SetActive(true);

            if (firstTime)
            {
                firstTime = false;
                waitingForInspection = false;
                hintManager.HideCloseInspectorHint();
            }
        }

        if (waitingForZoom || waitingForHarvest || waitingForSelection || waitingForInspection)
        {
            nextButton.SetActive(false);
        }
        else
        {
            nextButton.SetActive(true);
        }

        if (FindObjectOfType<CameraManager>().isZoomedOut && waitingForZoom)
        {
            waitingForZoom = false;
            hintManager.HideScrollHint();
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
            if (waitingForZoom || waitingForHarvest || waitingForSelection || waitingForInspection)
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
            hintManager.ShowScrollHint();
            return;
        }

        if (currentSentence.Contains("[PULLIVAN]"))
        {
            currentSentence = currentSentence.Replace("[PULLIVAN]", "");
            hintManager.ShowInteractHint();
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
            hintManager.ShowInteractHint();
            return;
        }

        if (currentSentence.Contains("[SELECT]") && !waitingForSelection)
        {
            waitingForSelection = true;
            currentSentence = currentSentence.Replace("[SELECT]", "");
            dialogueText.text = currentSentence;
            hintManager.ShowSelectHint();
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
        hintManager.ShowPlantHint();
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
