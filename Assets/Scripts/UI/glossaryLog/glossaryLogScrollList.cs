using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class glossaryLogScrollList : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject contentParent;

    [Header("Rect Transforms")]
    [SerializeField] private RectTransform scrollRectTransform;
    [SerializeField] private RectTransform contentRectTransform;

    [Header("Quest Log Button")]
    [SerializeField] private GameObject glossaryButtonPrefab;

    private Dictionary<string, glossaryLogButton> idToButtonMap = new Dictionary<string, glossaryLogButton>();

    public glossaryLogButton CreateButtonIfNotExists(lessonInfoSO lesson, UnityAction selectAction)
    {
        glossaryLogButton glossaryButton = null;

        if (!idToButtonMap.ContainsKey(lesson.lessonId))
        {
            glossaryButton = InstantiateButton(lesson, selectAction);
        }
        else
        {
            glossaryButton = idToButtonMap[lesson.lessonId];
        }

        return glossaryButton;
    }

    private glossaryLogButton InstantiateButton(lessonInfoSO lesson, UnityAction selectAction)
    {
        glossaryLogButton btn = Instantiate(glossaryButtonPrefab, contentParent.transform)
            .GetComponent<glossaryLogButton>();

        btn.gameObject.name = lesson.lessonId + "_button";
        RectTransform buttonRect = btn.GetComponent<RectTransform>();

        btn.Initialize(lesson.lessonTitle, () =>
        {
            selectAction();
            updateScrolling(buttonRect);
        });

        idToButtonMap[lesson.lessonId] = btn;
        return btn;
    }

    void updateScrolling(RectTransform buttonRectTransform)
    {
        float buttonYMin = Mathf.Abs(buttonRectTransform.anchoredPosition.y);
        float buttonYMax = buttonYMin + buttonRectTransform.rect.height;

        float contentYMin = contentRectTransform.anchoredPosition.y;
        float contentYMax = contentYMin + scrollRectTransform.rect.height;

        if (buttonYMax > contentYMax)
        {
            contentRectTransform.anchoredPosition = new Vector2(
                contentRectTransform.anchoredPosition.x,
                buttonYMax - scrollRectTransform.rect.height
            );
        }
        else if (buttonYMin < contentYMin)
        {
            contentRectTransform.anchoredPosition = new Vector2(
                contentRectTransform.anchoredPosition.x,
                buttonYMin
            );
        }
    }
}
