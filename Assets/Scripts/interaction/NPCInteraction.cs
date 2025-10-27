using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class NPCInteraction : InteractableBase
{
    [Header("Ink JSON")]
    [SerializeField] private string knotName;

    private CircleCollider2D circleCollider;

    private void Reset()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;
        circleCollider.radius = .25f;
    }
    public override void Interact()
    {
        // if (!dialogueManager.GetInstance().isDialoguePlayin)
        // {
        //     // dialogueManager.GetInstance().enterDialogueMode(inkJSON);
        // }
    }


    void submitPressed(inputEventContext inputEventContext)
    {
        if (!inputEventContext.Equals(inputEventContext.DEFAULT))
        {
            return;
        }

        gameEventsManager.instance.dialogueEvents.enterDialogue(knotName);

    }

    private void OnEnable()
    {
        gameEventsManager.instance.inputEvents.onSubmitPressed += submitPressed;
    }

    private void OnDisable()
    {
        gameEventsManager.instance.inputEvents.onSubmitPressed -= submitPressed;

    }
}
