using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackUpQuestStep : questStep
{
    // private DoorInteraction doorInteraction;
    private int thingsInteracted = 0;
    private int thingsRequired = 3;

    // private void Awake()
    // {
    //     doorInteraction = GetComponent<DoorInteraction>();

    //     // Safety check to ensure the component was found
    //     if (doorInteraction == null)
    //     {
    //         Debug.LogError("DoorInteraction component not found on the same GameObject as PackUpQuestStep.");
    //     }
    // }

    public void PackedUp()
    {
        if (thingsInteracted < thingsRequired)
        {
            thingsInteracted++;
            Debug.Log(thingsInteracted);
            // updateState();
        }

        if (thingsInteracted >= thingsRequired)
        {
            finishQuestStep();
            // doorInteraction.questFinished();
        }
    }

    // private void updateState()
    // {
    //     string state = interactedToThings.ToString();
    //     changeState(state);
    // }
}
