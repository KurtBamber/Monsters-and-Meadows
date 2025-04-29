using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public enum TriggerType { OnClick, OnTriggerEnterOptional, OnTriggerEnterCompulsory, OnStart, OnPulledOut };
    public TriggerType triggerType;
    public bool hasTriggered;
    private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (triggerType == TriggerType.OnStart)
        {
            TriggerDialogue();
            hasTriggered = true;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && triggerType == TriggerType.OnTriggerEnterOptional)
        {
            if (!dialogueManager.dialogueBox.activeSelf)
            {
                TriggerDialogue();
                hasTriggered = true;
                Destroy(gameObject);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player") && triggerType == TriggerType.OnTriggerEnterCompulsory)
        {
            TriggerDialogue();
            hasTriggered = true;
            Destroy(gameObject);
        }
    }

    public void OnPulledOut()
    {
        TriggerDialogue();
        hasTriggered = true;
        Destroy(gameObject);
    }

    public void TriggerDialogue()
    {
        if (hasTriggered)
        {
            Destroy(gameObject);
            return;
        }
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
