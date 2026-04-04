using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class questLogButton : MonoBehaviour, ISelectHandler
{
    public Button button { get; private set; }
    private TextMeshProUGUI buttonText;
    private UnityAction onSelectAction;

    public void Initialize(string displayName, UnityAction selectAction)
    {
        this.button = this.GetComponent<Button>();
        this.buttonText = this.GetComponentInChildren<TextMeshProUGUI>();
        this.buttonText.text = displayName;
        this.onSelectAction = selectAction;

    }

    public void OnSelect(BaseEventData eventData)
    {
        onSelectAction();
    }

    public void setState(questState state)
    {
        switch (state)
        {
            case questState.REQ_NOT_MET:
                buttonText.color = Color.red;
                break;
            case questState.CAN_START:
                buttonText.color = Color.black;
                break;
            case questState.IN_PROGRESS:
                buttonText.color = Color.yellow;
                break;
            case questState.CAN_FINISH:
                buttonText.color = Color.green;
                break;
            case questState.FINISHED:
                buttonText.color = Color.blue;
                break;
            default:
                Debug.LogWarning("Quest state not recognized by match: " + state);
                break;

        }
    }
}
