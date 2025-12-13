using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class lessonPanelUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject contentParent;
    [SerializeField] private TextMeshProUGUI lessonTitle;
    [SerializeField] private TextMeshProUGUI lessonDesc;

    void Start()
    {
        contentParent.SetActive(false);
    }

    private void OnEnable()
    {
        gameEventsManager.instance.miscEvents.onShowLessonPanel += activatePanel;
    }

    private void OnDisable()
    {

        gameEventsManager.instance.miscEvents.onShowLessonPanel -= activatePanel;
    }

    public void activatePanel(lessonInfoSO info)
    {
        lessonTitle.text = info.lessonTitle;
        lessonDesc.text = info.lessonDesc;

        Time.timeScale = 0f;

        gameEventsManager.instance.playerEvents.DisablePlayerMovement();
        contentParent.SetActive(true);
    }

    public void closeBtn()
    {
        Time.timeScale = 1f;

        gameEventsManager.instance.playerEvents.EnablePlayerMovement();
        contentParent.SetActive(false);
    }
}
