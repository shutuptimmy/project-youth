using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameEventsManager : MonoBehaviour
{
    public inputEvents inputEvents;
    public questEvents questEvents;
    public playerEvents playerEvents;
    public dialogueEvents dialogueEvents;
    public sceneEvents sceneEvents;
    public miscEvents miscEvents;
    // public cutsceneEvents cutsceneEvents;

    public static gameEventsManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Game Events Manager in the scene. Removing duplicate..");
            Destroy(gameObject);
            return;
        }
        instance = this;

        // initialize the events
        inputEvents = new inputEvents();
        questEvents = new questEvents();
        playerEvents = new playerEvents();
        dialogueEvents = new dialogueEvents();
        sceneEvents = new sceneEvents();
        miscEvents = new miscEvents();
        // cutsceneEvents = new cutsceneEvents();
    }


}
