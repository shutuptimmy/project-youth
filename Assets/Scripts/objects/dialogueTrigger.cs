using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogueTrigger : MonoBehaviour
{
    [SerializeField] private string dialogueKnotName;
    private bool hasTriggered;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !hasTriggered)
        {
            gameEventsManager.instance.dialogueEvents.enterDialogue(dialogueKnotName);
            hasTriggered = true;
        }
    }
}
