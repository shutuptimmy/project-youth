using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class lessonPaper : InteractableBase, IDataPersistence
{
    [Header("Components")]
    [SerializeField] private lessonInfoSO lessonInfo;
    private string paperId;
    private SpriteRenderer sprite;
    private BoxCollider2D boxCollider;
    private bool isCollectible = false;

    private void Reset()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
        boxCollider.offset = new Vector2(0f, -0.015f);
        boxCollider.size = new Vector2(0.16f, 0.11f);
    }

    private void Start()
    {
        paperId = lessonInfo.lessonId;
        if (!string.IsNullOrEmpty(paperId))
        {
            SetVisualsActive(false);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        gameEventsManager.instance.miscEvents.onQuestReward += rewardUnlocked;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        gameEventsManager.instance.miscEvents.onQuestReward -= rewardUnlocked;

    }

    void SetVisualsActive(bool isActive)
    {
        // Safety Check (Lazy Loading)
        if (sprite == null) sprite = GetComponent<SpriteRenderer>();
        if (boxCollider == null) boxCollider = GetComponent<BoxCollider2D>();


        sprite.enabled = isActive;
        boxCollider.enabled = isActive;
        isCollectible = isActive;
    }

    public override void Interact()
    {
        if (!isCollectible)
        {
            return;
        }
        gameEventsManager.instance.miscEvents.showLessonPanel(lessonInfo);
        gameEventsManager.instance.miscEvents.questReward(paperId + "_READ");
        Destroy(this.gameObject);
    }

    void rewardUnlocked(string id)
    {
        // show if id is present during gameplay
        if (id == paperId)
        {
            SetVisualsActive(true);
        }
    }

    public void loadData(gameData data)
    {
        paperId = lessonInfo.lessonId;

        if (!string.IsNullOrEmpty(paperId))
        {
            bool isUnlocked = data.unlockedRewardIds.Contains(paperId);
            bool isRead = data.unlockedRewardIds.Contains(paperId + "_READ");

            Debug.LogWarning("is unlocked?" + isUnlocked);
            Debug.LogWarning("has been read? " + isRead);

            if (isRead)
            {
                Debug.LogWarning("paper destroyed");
                Destroy(this.gameObject);
            }
            else if (isUnlocked)
            {
                Debug.LogWarning("paper seen");
                SetVisualsActive(true);
            }
            else
            {
                Debug.LogWarning("visuals off");
                SetVisualsActive(false);
            }
        }
    }
    public void saveData(gameData data) { }
}
