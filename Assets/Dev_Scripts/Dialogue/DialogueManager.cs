using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

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

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
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
            if (waitingForZoom)
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
        typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
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
