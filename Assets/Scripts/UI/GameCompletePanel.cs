using UnityEngine;
using UnityEngine.UI;

public class GameCompletePanel : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject contentParent;
    [SerializeField] private Button continueBtn;
    [SerializeField] private questInfoSO aftermathQuestInfo;
    [SerializeField] private string rewardID;
    [SerializeField] private Collider2D carCollider;
    [SerializeField] private Collider2D doorCollider;
    private string questId;
    private questState currentQuestState;

    private bool isGameComplete;
    private bool isHarryPresent;

    void Awake()
    {
        if (aftermathQuestInfo != null)
        {
            questId = aftermathQuestInfo.id;
        }
    }

    void OnEnable()
    {
        gameEventsManager.instance.questEvents.onQuestStateChange += questStateChange;
    }

    void OnDisable()
    {
        gameEventsManager.instance.questEvents.onQuestStateChange -= questStateChange;
    }

    void Start()
    {
        if (isGameComplete)
        {
            doorCollider.enabled = false;
            if (isHarryPresent) carCollider.enabled = false;
        }
    }

    void questStateChange(quest quest)
    {
        // only update the quest state if this point has the corresponding quest
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            Debug.Log("quest id: " + questId + " updated to state: " + currentQuestState);
        }
    }

    void setGameQuestComplete()
    {
        gameEventsManager.instance.questEvents.finishQuest(questId);
        gameEventsManager.instance.miscEvents.questReward(rewardID + "_READ");
    }

    public void loadData(gameData data)
    {
        Debug.Log("GameCP loaded");
        continueBtn.onClick.RemoveAllListeners();

        isGameComplete = data.unlockedRewardIds.Contains(rewardID);
        bool hasRead = data.unlockedRewardIds.Contains(rewardID + "_READ");

        if (!isGameComplete || hasRead) contentParent.SetActive(false);
        else
        {
            gameEventsManager.instance.miscEvents.questReward("afternoon");
            contentParent.SetActive(true);
            continueBtn.onClick.AddListener(setGameQuestComplete);
        }

        isHarryPresent = data.unlockedRewardIds.Contains("forceChar");
    }

    public void saveData(gameData data) {}
}
