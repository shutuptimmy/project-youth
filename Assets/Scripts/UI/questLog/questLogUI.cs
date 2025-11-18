using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class questLogUI : MonoBehaviour
{
    [Header("Components")]

    [SerializeField] private GameObject contentParent;
    [SerializeField] private questLogScrollList scrollList;
    [SerializeField] private TextMeshProUGUI questDisplayNameText;
    [SerializeField] private TextMeshProUGUI questStatusText;
    [SerializeField] private TextMeshProUGUI expRewardsText;
    [SerializeField] private TextMeshProUGUI lvlReqsText;
    [SerializeField] private TextMeshProUGUI questReqsText;


    private Button firstSelectButton;

    private void OnEnable()
    {
        gameEventsManager.instance.inputEvents.onQuestLogTogglePressed += questLogTogglePressed;
        gameEventsManager.instance.questEvents.onQuestStateChange += questStateChange;
    }

    private void OnDisable()
    {
        gameEventsManager.instance.questEvents.onQuestStateChange -= questStateChange;

    }

    void questLogTogglePressed()
    {
        if (contentParent.activeInHierarchy)
        {
            hideUI();
        }
        else
        {
            showUI();
        }
    }

    void showUI()
    {
        contentParent.SetActive(true);
        gameEventsManager.instance.playerEvents.DisablePlayerMovement();

        // this needs to happen after the content parent is set to active or it won't work
        if (firstSelectButton != null)
        {
            firstSelectButton.Select();

        }
    }
    void hideUI()
    {
        contentParent.SetActive(false);
        gameEventsManager.instance.playerEvents.EnablePlayerMovement();
        EventSystem.current.SetSelectedGameObject(null);
    }

    void questStateChange(quest quest)
    {
        questLogButton questLogButton = scrollList.CreateButtonIfNotExists(quest, () =>
       {
           setQuestLogInfo(quest);
       });

        // initialize the first selected button if not already so that it's always on top
        if (firstSelectButton == null)
        {
            firstSelectButton = questLogButton.button;
        }

        // set button color based on quest state
        questLogButton.setState(quest.state);
    }

    void setQuestLogInfo(quest quest)
    {
        // quest name
        questDisplayNameText.text = quest.info.displayName;

        // status
        questStatusText.text = quest.getFullStatusText();

        // req
        lvlReqsText.text = "Level " + quest.info.lvlReq;
        questReqsText.text = "";

        foreach (questInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
        {
            questReqsText.text += prerequisiteQuestInfo.displayName + "\n";
        }

        // rewards
        expRewardsText.text = quest.info.expReward + "XP";
    }
}
