using UnityEngine;

public class HUDUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup contentCanvasGroup;

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
        
        if (currentContext.Equals(inputEventContext.DEFAULT))
        {
            contentCanvasGroup.alpha = 0f;
            contentCanvasGroup.blocksRaycasts = false;
            contentCanvasGroup.interactable = false;
        }
        else 
        {
            contentCanvasGroup.alpha = 1f;
            contentCanvasGroup.blocksRaycasts = true;
            contentCanvasGroup.interactable = true;
        }
    }
}
