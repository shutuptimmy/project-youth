using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDraggable
{
    bool IsCurrentlyDragging();
}

public abstract class InteractableBase : MonoBehaviour
{
    [SerializeField] protected GameObject interactableVisualCue;
    protected bool isInteractable;
    protected GameObject player;
    private bool isGlobalDragging = false;

    protected virtual void Awake()
    {
        // required a defense check in case the global player transitions to minigame player or reverse
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        interactableVisualCue.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        gameEventsManager.instance.inputEvents.onInteractPressed += interactPressed;
        gameEventsManager.instance.miscEvents.onBoxDraggingStateChanged += onGlobalDragChanged;
    }

    protected virtual void OnDisable()
    {
        gameEventsManager.instance.inputEvents.onInteractPressed -= interactPressed;
        gameEventsManager.instance.miscEvents.onBoxDraggingStateChanged -= onGlobalDragChanged;

    }

    private void interactPressed(inputEventContext inputEventContext)
    {
        if (!isInteractable)
        {
            return;
        }

        if (isGlobalDragging)
        {
            IDraggable myDraggable = GetComponent<IDraggable>();

            // If I am NOT a draggable object, OR I am not the specific one being dragged...
            // ... I should ignore this input.
            if (!myDraggable.IsCurrentlyDragging())
            {
                return;
            }
        }

        Interact();
    }

    void onGlobalDragChanged(string id, bool isDragging)
    {
        isGlobalDragging = isDragging;

        if (isGlobalDragging)
        {
            // If dragging started, and I am NOT the one being dragged, hide my cue
            // We check this by seeing if I implement IDraggable AND if I'm the one active
            IDraggable myDraggable = GetComponent<IDraggable>();
            bool currentlyDraggingObject = myDraggable != null && myDraggable.IsCurrentlyDragging();

            if (!currentlyDraggingObject)
            {
                interactableVisualCue.SetActive(false);
            }
        }
        else
        {
            // If dragging stopped, and the player is still standing on top of me,
            // re-show the cue (unless I was the one just dropped, trigger logic handles that)
            if (isInteractable)
            {
                interactableVisualCue.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            isInteractable = true;

            // Only show the cue if the player isn't busy dragging something else
            if (!isGlobalDragging)
            {
                interactableVisualCue.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            // Look for the interface instead of a specific class
            IDraggable draggable = GetComponent<IDraggable>();

            bool currentlyDragging = draggable != null && draggable.IsCurrentlyDragging();

            if (!currentlyDragging)
            {
                interactableVisualCue.SetActive(false);
                isInteractable = false;
            }
        }

    }

    public void overridePlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }

    public abstract void Interact();
}
