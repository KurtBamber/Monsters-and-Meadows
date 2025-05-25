using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public enum TriggerType { OnClick, OnTriggerEnterOptional, OnTriggerEnterCompulsory, OnStart, OnPulledOut, OnHouseBuilt, OnRaidComplete };
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
        if (triggerType == TriggerType.OnPulledOut)
        {
            TriggerDialogue();
            hasTriggered = true;
            Destroy(gameObject);
        }
    }

    public void OnHouseBuilt()
    {
        if (triggerType == TriggerType.OnHouseBuilt)
        {
            FindObjectOfType<Enemy_Spawner>().spawnEnemies = true;
            TriggerDialogue();
            hasTriggered = true;
            Destroy(gameObject);
        }
    }

    public void OnRaidComplete()
    {
        if (triggerType == TriggerType.OnRaidComplete)
        {
            Enemy_Manager[] allEnemies = FindObjectsOfType<Enemy_Manager>();
            bool allScared = true;

            foreach (Enemy_Manager enemy in allEnemies)
            {
                if (!enemy.isScared)
                {
                    allScared = false;
                    break;
                }
            }

            if (allScared)
            {
                FindObjectOfType<Enemy_Spawner>().firstRaid = false;
                TriggerDialogue();
                hasTriggered = true;
                Destroy(gameObject);
            }
        }
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
