using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class dialogueChoiceButton : MonoBehaviour, ISelectHandler
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

    public void selectButton()
    {
        button.Select();
    }

    public void OnSelect(BaseEventData eventData)
    {
        gameEventsManager.instance.dialogueEvents.UpdateChoiceIndex(choiceIndex);
    }
}
