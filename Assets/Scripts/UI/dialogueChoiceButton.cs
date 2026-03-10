using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class dialogueChoiceButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerClickHandler
{
    [Header("Components")]
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI choiceText;

    private int choiceIndex = -1;

    public void setChoiceText(string choiceTextString)
    {
        choiceText.text = choiceTextString;

    }

    public void setChoiceIndex(int choiceIndex)
    {
        this.choiceIndex = choiceIndex;
    }

    // for keyboard hover
    public void selectButton()
    {
        button.Select();
    }

    // for keyboard submit key
    public void OnSelect(BaseEventData eventData)
    {
        gameEventsManager.instance.dialogueEvents.UpdateChoiceIndex(choiceIndex);
    }

    // for mouse hover
    public void OnPointerEnter(PointerEventData eventData)
    {
        button.Select();
    }

    // for mouse click
    public void OnPointerClick(PointerEventData eventData)
    {
        gameEventsManager.instance.dialogueEvents.MakeChoice(choiceIndex);
    }
}
