using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class computer : InteractableBase
{

    [Header("Components")]
    private BoxCollider2D boxCollider;

    [Header("Dialogue")]
    [SerializeField] private string dialogueKnotName;

    public override void Interact()
    {
        gameEventsManager.instance.dialogueEvents.enterDialogue(dialogueKnotName);
    }
}
