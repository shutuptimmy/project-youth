using UnityEngine;

public class HUDUI : MonoBehaviour
{
    [SerializeField] private GameObject contentParent;

    void OnEnable()
    {
        gameEventsManager.instance.dialogueEvents.onDialogueStarted += DisplayHUD;
        gameEventsManager.instance.dialogueEvents.onDialogueFinished += DisplayHUD;
        gameEventsManager.instance.sceneEvents.onStartMinigame += DisplayHUD;
        gameEventsManager.instance.sceneEvents.onQuitMinigame += DisplayHUD;
    }

    void OnDisable()
    {
        gameEventsManager.instance.dialogueEvents.onDialogueStarted -= DisplayHUD;
        gameEventsManager.instance.dialogueEvents.onDialogueFinished -= DisplayHUD;
        gameEventsManager.instance.sceneEvents.onStartMinigame -= DisplayHUD;
        gameEventsManager.instance.sceneEvents.onQuitMinigame -= DisplayHUD;
        
    }

    void DisplayHUD()
    {
        inputEventContext currentContext = gameEventsManager.instance.inputEvents.inputEventContext;
        
        if (currentContext.Equals(inputEventContext.DEFAULT)) contentParent.SetActive(false);
        else contentParent.SetActive(true);
    }
}
