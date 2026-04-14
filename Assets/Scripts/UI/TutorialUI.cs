using UnityEngine;

public class TutorialUI : MonoBehaviour, IDataPersistence
{
    [SerializeField] GameObject contentParent;
    public void DeactivateMenu()
    {
        Destroy(this.gameObject);
    }

    public void loadData(gameData data)
    {
        bool isQuestComplete = data.unlockedRewardIds.Contains("prologue");
        if (isQuestComplete) Destroy(this.gameObject);
        else contentParent.SetActive(true);
    }

    public void saveData(gameData data)
    {
    }
}
