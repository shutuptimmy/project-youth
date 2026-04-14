using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class saveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI locationText;
    // [SerializeField] private TextMeshProUGUI playTimeText;
    [SerializeField] private Animator playerPortrait;

    [Header("Clear Data Button")]
    [SerializeField] private Button clearButton;

    public bool hasData { get; private set; } = false;

    private Button saveSlotButton;

    private void Awake()
    {
        saveSlotButton = this.GetComponent<Button>();
    }

    public void setData(gameData data)
    {
        // there's no data for this profileId
        if (data == null)
        {
            hasData = false;
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
            clearButton.gameObject.SetActive(false);
        }
        // there's data for this profileId
        else
        {
            hasData = true;
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            clearButton.gameObject.SetActive(true);

            nameText.text = data.playerName.ToString();
            levelText.text = $"Level {(data.playerLvl >= globalConstants.maxLevel ? "MAX" : data.playerLvl.ToString())}";
            locationText.text = data.playerLocation;
            // playTimeText.text = playTimeFormat(data.playTime);

            switch (data.playerGender)
            {
                case 0:
                    playerPortrait.Play("mcBoy");
                    break;
                case 1:
                    playerPortrait.Play("mcGirl");
                    break;
                default:
                    playerPortrait.Play(null);
                    Debug.LogWarning("Player portrait cannot be defined by this value: " + data.playerGender);
                    break;
            }
        }
    }

    public string getProfileId()
    {
        return this.profileId;
    }


    public void setInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
        // clearButton.interactable = false;
    }

    public string getPlayerLocationText()
    {
        return locationText.text;
    }

    // private string playTimeFormat(float totalSeconds)
    // {
    //     int hours = Mathf.FloorToInt(totalSeconds / 3600);
    //     int minutes = Mathf.FloorToInt((totalSeconds % 3600) / 60);

    //     return string.Format("Play Time: {0:00}:{1:00}", hours, minutes);
    // }
}
