using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameSlot : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject minigameGameObject;
    [SerializeField] private string minigameName;
    [SerializeField] private string requiredNPCID;
    [SerializeField] private TextMeshProUGUI minigameTitle;
    [SerializeField] private Button button;
    [SerializeField] private GameObject lockedIcon;
    [SerializeField] private GameObject unlockedBG;

    private bool isUnlocked = false;

    void Start()
    {
        isUnlocked = rewardManager.instance.IsRewardUnlocked(requiredNPCID);
    
        button.onClick.RemoveAllListeners();

        if (isUnlocked)
        {
            Debug.Log($"{minigameName} minigame is unlocked");
            MinigameUnlocked();
            button.onClick.AddListener(OnButtonClicked);
        }
        else
        {
            Debug.Log($"{minigameName} minigame is locked");
            MinigameLocked();
        }
    }
    
    void OnButtonClicked()
    {
        if (!isUnlocked) return;
        GameObject minigameInstance = Instantiate(minigameGameObject);
        minigameInstance.transform.SetParent(questManager.instance.transform, false);
    }

    void MinigameLocked()
    {
        minigameTitle.text = "???";
        button.interactable = false;
        unlockedBG.SetActive(false);
        lockedIcon.SetActive(true);   
    }

    void MinigameUnlocked()
    {
        minigameTitle.text = minigameName;
        button.interactable = true;
        unlockedBG.SetActive(true);
        lockedIcon.SetActive(false);
    }
}
