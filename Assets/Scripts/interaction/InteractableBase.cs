using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableBase : MonoBehaviour
{
    [SerializeField] protected GameObject interactableVisualCue;
    protected bool isInteractable;
    protected GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        interactableVisualCue.SetActive(false);
    }

    // private void Update()
    // {
    //     if (isInteractable && inputManager.GetInstance().GetInteractPressed())
    //     {
    //         Interact();
    //     }
    // }

    private void OnEnable()
    {
        gameEventsManager.instance.inputEvents.onSubmitPressed += submitPressed;
    }

    private void OnDisable()
    {
        gameEventsManager.instance.inputEvents.onSubmitPressed -= submitPressed;

    }

    private void submitPressed(inputEventContext inputEventContext)
    {
        if (!isInteractable)
        {
            return;
        }
        Interact();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            interactableVisualCue.SetActive(true);
            isInteractable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            interactableVisualCue.SetActive(false);
            isInteractable = false;
        }

    }

    public abstract void Interact();
}
