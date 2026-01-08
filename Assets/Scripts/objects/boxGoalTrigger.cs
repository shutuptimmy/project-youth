using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxGoalTrigger : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private boxPuzzleManager puzzleManager;
    [SerializeField] private pushableBox specificBox;
    [SerializeField] private GameObject visualSprite;
    private string requiredBoxId;

    private void Start()
    {
        gameEventsManager.instance.miscEvents.onBoxDraggingStateChanged += OnBoxDraggingStateChanged;
        visualSprite.SetActive(false);
    }

    private void OnDestroy()
    {
        gameEventsManager.instance.miscEvents.onBoxDraggingStateChanged -= OnBoxDraggingStateChanged;
    }

    private void OnBoxDraggingStateChanged(string boxId, bool isDragging)
    {
        // Check if the dragged box's ID matches the ID required by this goal
        if (boxId.Equals(requiredBoxId))
        {
            // Only show the outline if the correct box is being dragged
            visualSprite.SetActive(isDragging);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.Equals(specificBox.GetCollider2D()))
        {
            visualSprite.SetActive(false);
            // this.gameObject.SetActive(false);
            specificBox.Release();
            puzzleManager.puzzleComplete();
        }
    }

    public string setPuzzleId(string id)
    {
        return requiredBoxId = id;
    }
}
