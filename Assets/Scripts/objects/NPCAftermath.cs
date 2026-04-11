using UnityEngine;

public class NPCAftermath : MonoBehaviour
{
    [SerializeField] private questInfoSO aftermathQuest;
    [SerializeField] private GameObject[] npcs;
    private bool isQuestInProgress;

    void Start()
    {
        // if (aftermathQuest == questState.IN_PROGRESS) isQuestInProgress = true;
        // else isQuestInProgress = false;

        foreach (GameObject npc in npcs)
        {
            npc.SetActive(isQuestInProgress);
        }
    }
}
