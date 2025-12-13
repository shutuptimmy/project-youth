using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class minigameMenuPanelUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject contentParent;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private TextMeshProUGUI playerHighscoreText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    public void activateMenu(string titleText, string displayText, string playerHighscoreText, UnityAction startAction, string startText, UnityAction quitAction, bool showQuit)
    {
        this.gameObject.SetActive(true);

        this.titleText.text = titleText;
        this.displayText.text = displayText;
        this.playerHighscoreText.text = playerHighscoreText;

        startButton.GetComponentInChildren<TextMeshProUGUI>().text = startText;

        quitButton.gameObject.SetActive(showQuit);

        startButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();

        startButton.onClick.AddListener(() =>
        {
            deactivateMenu();
            startAction();
        });
        quitButton.onClick.AddListener(() =>
        {
            deactivateMenu();
            quitAction();
        });
    }

    void deactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
