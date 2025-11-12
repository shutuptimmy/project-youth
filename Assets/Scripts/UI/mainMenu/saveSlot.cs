using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private TextMeshProUGUI statusText; // player name & level
    [SerializeField] private TextMeshProUGUI locationText;
    [SerializeField] private TextMeshProUGUI chapterText;
    [SerializeField] private TextMeshProUGUI playTimeText;

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

            statusText.text = data.playerName + " | Level " + data.playerLvl.ToString();
            locationText.text = data.playerLocation;
            Debug.Log(locationText.text);
            chapterText.text = "Chapter " + data.playerChapter.ToString();

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
}
