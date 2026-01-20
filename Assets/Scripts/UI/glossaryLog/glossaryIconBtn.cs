using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glossaryIconBtn : MonoBehaviour
{
    private void OnEnable()
    {
        gameEventsManager.instance.inputEvents.onPausePressed += btnStatus;
    }

    private void OnDisable()
    {
        gameEventsManager.instance.inputEvents.onPausePressed -= btnStatus;
    }

    void btnStatus()
    {
        inputEventContext currentContext = gameEventsManager.instance.inputEvents.inputEventContext;

        if (currentContext.Equals(inputEventContext.DEFAULT)) this.gameObject.SetActive(true);
        else this.gameObject.SetActive(false);
    }
}
