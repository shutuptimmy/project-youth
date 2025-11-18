using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class questObjectivePanel : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject contentPanel;
    [SerializeField] private TextMeshProUGUI currentObjectiveText;

    [SerializeField] private questManager questManager;

    private void Awake()
    {
        // Find the single instance of the questManager in the persistent scene
        // questManager = FindObjectOfType<questManager>();
        if (questManager == null)
        {
            Debug.LogError("Quest Manager not found in scene. Quest system will not function.");
        }
    }

    private void OnEnable()
    {
        gameEventsManager.instance.questEvents.onQuestStateChange += updateObjectiveDisplay;
    }

    private void OnDisable()
    {
        gameEventsManager.instance.questEvents.onQuestStateChange -= updateObjectiveDisplay;
    }

    // This method is the event handler. We don't use the 'quest' argument directly,
    // but its firing tells us to check for the current active quest.
    private void updateObjectiveDisplay(quest quest)
    {
        UpdateDisplay();
    }

    // Core logic to check the quest state and update the UI
    private void UpdateDisplay()
    {
        if (questManager == null) return;

        // Use the new public method to retrieve the active quest
        quest activeQuest = questManager.getQuestInProgress();

        if (activeQuest != null)
        {
            // Set the panel active and display the quest name
            contentPanel.SetActive(true);

            // Display the quest's display name
            currentObjectiveText.text = activeQuest.info.displayName;

            // OPTIONAL: If you want to show the current step status instead of the name:
            // currentObjectiveText.text = activeQuest.getFullStatusText(); 
        }
        else
        {
            // Hide the panel if no quest is active
            contentPanel.SetActive(false);
        }
    }

    private void Start()
    {
        // Call once on startup to set the initial state (if a quest was loaded/started)
        UpdateDisplay();
    }
}
