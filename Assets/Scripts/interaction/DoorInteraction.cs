using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DoorInteraction : InteractableBase
{
    [Header("Door Target")]
    public SceneField sceneToLoad;
    public Vector2 newPlayerPos;

    [SerializeField] private questInfoSO questInfoForPoint;
    private string questId;
    private questState currentQuestState;

    private BoxCollider2D boxCollider;

    protected override void Awake()
    {
        base.Awake();

        boxCollider = GetComponent<BoxCollider2D>();

        if (questInfoForPoint != null)
        {
            questId = questInfoForPoint.id;
        }
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        gameEventsManager.instance.questEvents.onQuestStateChange += questStateChange;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        gameEventsManager.instance.questEvents.onQuestStateChange -= questStateChange;
    }

    public override void Interact()
    {
        gameEventsManager.instance.sceneEvents.changeScene(sceneToLoad, newPlayerPos);
    }

    void questStateChange(quest quest)
    {
        // only update the quest state if this point has the corresponding quest
        if (questInfoForPoint != null && quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            UpdateDoorState();
        }
    }

    private void UpdateDoorState()
    {
        bool isUnlocked = currentQuestState == questState.FINISHED;
        boxCollider.enabled = isUnlocked;

        Debug.Log($"Door for {questId} is now {(isUnlocked ? "Unlocked" : "Locked")}");
    }

    private void Reset()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
    }
}
