using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class questLogScrollList : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject contentParent;

    [Header("Rect Transforms")]
    [SerializeField] private RectTransform scrollRectTransform;
    [SerializeField] private RectTransform contentRectTransform;

    [Header("Quest Log Button")]
    [SerializeField] private GameObject questLogButtonPrefab;

    private Dictionary<string, questLogButton> idToButtonMap = new Dictionary<string, questLogButton>();


    public questLogButton CreateButtonIfNotExists(quest quest, UnityAction selectAction)
    {
        questLogButton questLogButton = null;
        // only create the button if we havent set this id before
        if (!idToButtonMap.ContainsKey(quest.info.id))
        {
            questLogButton = InstantiateQuestLogButton(quest, selectAction);
        }
        else
        {
            questLogButton = idToButtonMap[quest.info.id];
        }

        return questLogButton;
    }

    private questLogButton InstantiateQuestLogButton(quest quest, UnityAction selectAction)
    {
        // create a button
        questLogButton questLogButton = Instantiate(
            questLogButtonPrefab, contentParent.transform
        ).GetComponent<questLogButton>();
        // gameobject name in the scene
        questLogButton.gameObject.name = quest.info.id + "_button";
        // initialize and setup function for when the button is selected
        RectTransform buttonRectTransform = questLogButton.GetComponent<RectTransform>();
        questLogButton.Initialize(quest.info.displayName, () =>
        {
            selectAction();
            updateScrolling(buttonRectTransform);
        });
        // add map to keep track of the new button
        idToButtonMap[quest.info.id] = questLogButton;
        return questLogButton;
    }

    void updateScrolling(RectTransform buttonRectTransform)
    {
        // calculate the min and max for the selected button
        float buttonYMin = Mathf.Abs(buttonRectTransform.anchoredPosition.y);
        float buttonYMax = buttonYMin + buttonRectTransform.rect.height;

        // calculate the min and max for the content area
        float contentYMin = contentRectTransform.anchoredPosition.y;
        float contentYMax = contentYMin + scrollRectTransform.rect.height;

        // handle scrolling down
        if (buttonYMax > contentYMax)
        {
            contentRectTransform.anchoredPosition = new Vector2(
                contentRectTransform.anchoredPosition.x,
                buttonYMax - scrollRectTransform.rect.height
            );
        }
        // handle scrolling up
        else if (buttonYMin < contentYMin)
        {
            contentRectTransform.anchoredPosition = new Vector2(
                contentRectTransform.anchoredPosition.x,
                buttonYMin
            );
        }
    }
}
