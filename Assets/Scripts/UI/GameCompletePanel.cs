using UnityEngine;
using UnityEngine.UI;

public class GameCompletePanel : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject contentParent;
    [SerializeField] private Button continueBtn;
    [SerializeField] private string rewardID;
    [SerializeField] private Collider2D carCollider;

    private bool isGameComplete;
    private bool isHarryPresent;

    void Start()
    {
        if (isGameComplete && isHarryPresent) carCollider.enabled = false;
    }

    void setGameQuestComplete()
    {
        gameEventsManager.instance.questEvents.finishQuest("aftermath");
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
            contentParent.SetActive(true);
            continueBtn.onClick.AddListener(setGameQuestComplete);
        }

        isHarryPresent = data.unlockedRewardIds.Contains("forceChar");
    }

    public void saveData(gameData data) {}
}
