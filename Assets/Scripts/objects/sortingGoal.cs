using UnityEngine;
using TMPro;

public class sortingGoal : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private sortingBoxesManager manager;
    [SerializeField] private IBoxType goalType;
    [SerializeField] private TextMeshProUGUI goalText;
    [SerializeField] private GameObject visualSprite;



    private void Start()
    {
        gameEventsManager.instance.miscEvents.onBoxDraggingStateChanged += OnBoxDraggingStateChanged;
        visualSprite.SetActive(false);
        goalText.gameObject.SetActive(false);

    }

    private void OnDestroy()
    {
        gameEventsManager.instance.miscEvents.onBoxDraggingStateChanged -= OnBoxDraggingStateChanged;
    }

    private void OnBoxDraggingStateChanged(string boxId, bool isDragging)
    {
        // Only show the outline if the correct box is being dragged
        visualSprite.SetActive(isDragging);
        goalText.gameObject.SetActive(isDragging);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the object is a box
        sortingBox box = collider.GetComponent<sortingBox>();

        if (collider.Equals(box.GetCollider2D()))
        {
            // Verify if the type matches
            if (box.data.type == goalType) manager.correctGoal(box);
            else manager.wrongGoal(box);
        }
    }
}