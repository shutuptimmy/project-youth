using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class glossaryLogUI : MonoBehaviour, IDataPersistence
{
    [Header("Data")]
    // Drag ALL your lessonInfoSO files here in the inspector
    [SerializeField] private lessonInfoSO[] allGameLessons;

    [Header("Components")]
    [SerializeField] private GameObject contentParent;
    [SerializeField] private glossaryLogScrollList scrollList;

    [Header("Main Panel")]
    [SerializeField] private ScrollRect mainPanelScrollRect;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private Image contentImage;
    [SerializeField] private GameObject imageContainer;

    private Button firstSelectButton;
    private List<string> unlockedLessonIDs = new List<string>();

    // Call this function from your Book UI Button's OnClick event
    public void ToggleGlossary()
    {
        if (contentParent.activeInHierarchy) HideUI();
        else ShowUI();
    }

    void ShowUI()
    {
        contentParent.SetActive(true);
        gameEventsManager.instance.playerEvents.DisablePlayerMovement();

        loadGlossary();

        // Select first button for controller support
        if (firstSelectButton != null)
        {
            firstSelectButton.Select();
        }
        else
        {
            // Clear text if no terms are unlocked
            titleText.text = "No Notes Collected";
            descText.text = "Find lesson papers to fill your notebook.";
            imageContainer.SetActive(false);
        }
    }

    void HideUI()
    {
        contentParent.SetActive(false);
        gameEventsManager.instance.playerEvents.EnablePlayerMovement();
        EventSystem.current.SetSelectedGameObject(null);
    }

    void loadGlossary()
    {

        // 2. Loop through ALL terms, checking if they are unlocked
        foreach (lessonInfoSO lesson in allGameLessons)
        {
            string readKey = lesson.lessonId + "_READ";

            if (unlockedLessonIDs.Contains(readKey))
            {
                glossaryLogButton btn = scrollList.CreateButtonIfNotExists(lesson, () =>
                {
                    setMainPanelInfo(lesson);
                });

                if (firstSelectButton == null)
                {
                    firstSelectButton = btn.button;
                    setMainPanelInfo(lesson); // Show first item immediately
                }
            }
        }
    }

    void setMainPanelInfo(lessonInfoSO lesson)
    {
        titleText.text = lesson.lessonTitle;
        descText.text = lesson.lessonDesc;

        if (lesson.lessonImage != null)
        {
            imageContainer.SetActive(true);
            contentImage.sprite = lesson.lessonImage;
            contentImage.preserveAspect = true;
        }
        else
        {
            // Hide the image area if this specific lesson is text-only
            imageContainer.SetActive(false);
        }

        // NEW: Force the scroll view to jump back to the top
        // We use a Coroutine or a slight delay because UI layout needs a frame to recalculate size
        StartCoroutine(resetScrollPos());
    }

    IEnumerator resetScrollPos()
    {
        yield return new WaitForEndOfFrame();
        mainPanelScrollRect.verticalNormalizedPosition = 1f;
    }

    public void loadData(gameData data)
    {
        // Cache the unlocked list so we can use it when opening the menu
        this.unlockedLessonIDs = data.unlockedRewardIds;
    }

    public void saveData(gameData data) { }

}