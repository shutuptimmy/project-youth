using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private bool loadQuestState = true;

    private Dictionary<string, quest> questMap;

    private int currentPlayerLevel;

    private void Awake()
    {
        questMap = createQuestMap();
    }

    private void OnEnable()
    {
        gameEventsManager.instance.questEvents.onStartQuest += startQuest;
        gameEventsManager.instance.questEvents.onAdvanceQuest += advanceQuest;
        gameEventsManager.instance.questEvents.onFinishQuest += finishQuest;
        gameEventsManager.instance.questEvents.onQuestStepStateChange += questStepStateChange;

        gameEventsManager.instance.playerEvents.onPlayerLevelChange += playerLevelChanged;
    }

    private void OnDisable()
    {
        gameEventsManager.instance.questEvents.onStartQuest -= startQuest;
        gameEventsManager.instance.questEvents.onAdvanceQuest -= advanceQuest;
        gameEventsManager.instance.questEvents.onFinishQuest -= finishQuest;
        gameEventsManager.instance.questEvents.onQuestStepStateChange -= questStepStateChange;
        gameEventsManager.instance.playerEvents.onPlayerLevelChange -= playerLevelChanged;
    }

    private void Start()
    {
        foreach (quest quest in questMap.Values)
        {
            // initialize any loaded quest steps
            // if (quest.state == questState.IN_PROGRESS)
            // {
            //     quest.instantiateCurrentQuestStep(this.transform);
            // }

            // broadcast the initial state of all quests on startup
            gameEventsManager.instance.questEvents.questStateChange(quest);
        }
    }

    private void Update()
    {
        foreach (quest quest in questMap.Values)
        {
            if (quest.state == questState.REQ_NOT_MET && checkReqMet(quest))
            {
                changeQuestState(quest.info.id, questState.CAN_START);
            }
        }
    }

    private void changeQuestState(string id, questState state)
    {
        quest quest = getQuestById(id);
        quest.state = state;
        gameEventsManager.instance.questEvents.questStateChange(quest);
    }

    private void playerLevelChanged(int level)
    {
        currentPlayerLevel = level;
    }

    private bool checkReqMet(quest quest)
    {
        bool meetsReq = true;
        if (currentPlayerLevel < quest.info.lvlReq)
        {
            meetsReq = false;
        }

        foreach (questInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
        {
            if (getQuestById(prerequisiteQuestInfo.id).state != questState.FINISHED)
            {
                meetsReq = false;
            }
        }

        return meetsReq;
    }

    private void startQuest(string id)
    {
        quest quest = getQuestById(id);
        quest.instantiateCurrentQuestStep(this.transform);
        changeQuestState(quest.info.id, questState.IN_PROGRESS);
    }

    private void advanceQuest(string id)
    {
        quest quest = getQuestById(id);
        quest.moveToNextStep();

        if (quest.currentStepExists())
        {

            quest.instantiateCurrentQuestStep(this.transform);
        }
        else
        {
            changeQuestState(quest.info.id, questState.CAN_FINISH);
        }
    }
    private void finishQuest(string id)
    {
        Debug.Log("finish quest: " + id);
        quest quest = getQuestById(id);
        claimRewards(quest);
        changeQuestState(quest.info.id, questState.FINISHED);

    }

    private void claimRewards(quest quest)
    {
        gameEventsManager.instance.playerEvents.ExperienceGained(quest.info.expReward);
    }

    private void questStepStateChange(string id, int stepIndex, questStepState questStepState)
    {
        quest quest = getQuestById(id);
        quest.storeQuestStepState(questStepState, stepIndex);
        changeQuestState(id, quest.state);
    }

    private Dictionary<string, quest> createQuestMap()
    {
        // loads all questInfoSO under the Assets/Resources/Quests folder
        questInfoSO[] allQuests = Resources.LoadAll<questInfoSO>("Quests");
        // create quest map
        Dictionary<string, quest> idToQuestMap = new Dictionary<string, quest>();
        foreach (questInfoSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, new quest(questInfo));
            // idToQuestMap.Add(questInfo.id, loadQuest(questInfo));
        }
        return idToQuestMap;
    }

    private quest getQuestById(string id)
    {
        quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogError("ID not found in quest map tho:" + id);
        }
        return quest;
    }

    private void OnApplicationQuit()
    {
        foreach (quest quest in questMap.Values)
        {
            questData questData = quest.getQuestData();
            Debug.Log(quest.info.id);
            Debug.Log("state: " + questData.state);
            Debug.Log("index: " + questData.questStepIndex);
            foreach (questStepState stepState in questData.questStepStates)
            {
                Debug.Log("step state: " + stepState.state);

            }
            // saveQuest(quest);
        }

    }


    private void saveQuest(quest quest)
    {
        try
        {
            questData questData = quest.getQuestData();
            string serializedData = JsonUtility.ToJson(questData);
            PlayerPrefs.SetString(quest.info.id, serializedData);

            Debug.Log(serializedData);
        }
        catch (System.Exception e)
        {
            Debug.LogError("failed to save quest with id " + quest.info.id + ": " + e);
        }
    }

    private quest loadQuest(questInfoSO questInfo)
    {
        quest quest = null;
        try
        {
            // load quest from saved data 
            if (PlayerPrefs.HasKey(quest.info.id) && loadQuestState)
            {
                string serializedData = PlayerPrefs.GetString(questInfo.id);
                questData questData = JsonUtility.FromJson<questData>(serializedData);
                quest = new quest(questInfo, questData.state, questData.questStepIndex, questData.questStepStates);
            }
            // otherwise initialize a new quest
            else
            {
                quest = new quest(questInfo);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load quest with id " + quest.info.id + ": " + e);
        }
        return quest;
    }

    public quest getQuestInProgress()
    {
        foreach (quest quest in questMap.Values)
        {
            if (quest.state == questState.IN_PROGRESS)
            {
                return quest;
            }
        }
        return null; // Return null if no quest is currently in progress
    }
}

