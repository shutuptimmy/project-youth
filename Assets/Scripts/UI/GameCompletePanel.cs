using UnityEngine;
using UnityEngine.UI;

public class GameCompletePanel : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject contentParent;
    [SerializeField] private Button continueBtn;
    [SerializeField] private string rewardID;

    void setGameQuestComplete()
    {
        gameEventsManager.instance.questEvents.finishQuest("aftermath");
    }

    public void loadData(gameData data)
    {
        Debug.Log("GameCP loaded");
        continueBtn.onClick.RemoveAllListeners();

        bool isGameComplete = data.unlockedRewardIds.Contains(rewardID);

        if (isGameComplete)
        {
            contentParent.SetActive(true);
            continueBtn.onClick.AddListener(setGameQuestComplete);
        }
        else
        {
            contentParent.SetActive(false);
        }
    }

    public void saveData(gameData data) {}
}
