using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DoorInteraction : InteractableBase
{
    [Header("Door Target")]
    public SceneField sceneToLoad;
    public Vector2 newPlayerPos;

    [Header("Lock Mechanism")]
    [Tooltip("ID of the quest that must be completed to unlock the door. Leave empty if always unlocked.")]
    [SerializeField] private string requiredQuestId = "";

    private BoxCollider2D boxCollider;

    public override void Interact()
    {
        // 1. Check if a quest ID is required
        if (!string.IsNullOrEmpty(requiredQuestId))
        {
            // Access the running questManager instance
            questManager manager = FindObjectOfType<questManager>();

            // CRITICAL CHECK: Call the public method on the manager
            bool questCompleted = manager != null && manager.IsQuestCompleted(requiredQuestId);

            if (!questCompleted)
            {
                // Door is locked! Trigger dialogue and return.
                Debug.Log($"Door is locked. Quest required: {requiredQuestId}");

                // Assuming your dialogue system uses gameEventsManager for dialogue
                gameEventsManager.instance.dialogueEvents.enterDialogue("doorLocked");
                return; // Stop execution, do not load the scene
            }
        }

        // 2. If the door is unlocked, change the scene.
        gameEventsManager.instance.sceneEvents.changeScene(sceneToLoad, newPlayerPos);
    }

    private void Reset()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
    }
}
