using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[RequireComponent(typeof(BoxCollider2D))]
public class SceneTriggerInteraction : InteractableBase
{
    [SerializeField] private TimelineAsset cutscene;
    private BoxCollider2D boxCollider;
    private bool hasTriggered = false;

    public override void Interact()
    {
        cutsceneManager manager = cutsceneManager.GetInstance();

        if (manager != null && manager.director != null && cutscene != null && !hasTriggered)
        {
            manager.director.playableAsset = cutscene;
            manager.director.Play();
            hasTriggered = true;
            Debug.Log(hasTriggered);
        }
        else
        {
            Debug.LogWarning("Manager or director not found, or cutscene asset is not assigned.");
        }

        if (hasTriggered)
        {
            isInteractable = false;
        }
    }

    private void Reset()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
    }
}
