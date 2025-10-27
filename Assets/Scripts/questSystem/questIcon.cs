using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questIcon : MonoBehaviour
{
    [SerializeField] private GameObject reqNotMetToStartIcon;
    [SerializeField] private GameObject canStartIcon;
    [SerializeField] private GameObject reqNotMetToFinishIcon;
    [SerializeField] private GameObject canFinishIcon;

    public void setState(questState newState, bool startPoint, bool finishPoint)
    {
        // set all inactive
        reqNotMetToStartIcon.SetActive(false);
        canStartIcon.SetActive(false);
        reqNotMetToFinishIcon.SetActive(false);
        canFinishIcon.SetActive(false);

        // set appropriate one to active based on the new state
        switch (newState)
        {
            case questState.REQ_NOT_MET:
                if (startPoint)
                {
                    reqNotMetToStartIcon.SetActive(true);
                }
                break;

            case questState.CAN_START:
                if (startPoint)
                {
                    canStartIcon.SetActive(true);
                }
                break;

            case questState.IN_PROGRESS:
                if (finishPoint)
                {
                    reqNotMetToFinishIcon.SetActive(true);
                }

                break;

            case questState.CAN_FINISH:
                if (finishPoint)
                {
                    canFinishIcon.SetActive(true);
                }
                break;

            case questState.FINISHED:
                break;

            default:
                Debug.LogWarning("quest state not recognized by switch statement for quest icon: " + newState);
                break;
        }
    }
}
