using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IvanController : MonoBehaviour
{
    private bool isPulled = false;
    public GameObject ivan;
    public DialogueTrigger dialogueTrigger;

    // Update is called once per frame
    void Update()
    {
        if (!isPulled && Input.GetMouseButtonDown(0))
        {
            PullIvan();
        }
    }

    private void PullIvan()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("FullyGrown"))
            {
                isPulled = true;
                Instantiate(ivan, transform.position, Quaternion.identity);
                Destroy(gameObject);
                FindObjectOfType<HintManager>().PlayerInteracted();
                FindObjectOfType<DialogueManager>().SkipToNextSentence();
                dialogueTrigger.OnPulledOut();
            }
        }
    }
}
