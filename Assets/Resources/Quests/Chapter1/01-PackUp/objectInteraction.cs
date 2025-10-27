using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class objectInteraction : InteractableBase
{

    [Header("Dialogue")]
    [SerializeField] private string dialogueKnotName;
    public PackUpQuestStep questStep;

    public override void Interact()
    {
        gameEventsManager.instance.dialogueEvents.enterDialogue(dialogueKnotName);
        questStep.PackedUp();
        Destroy(this.gameObject);

    }

}
