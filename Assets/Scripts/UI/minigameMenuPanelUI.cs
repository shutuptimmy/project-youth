using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class minigameMenuPanelUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private TextMeshProUGUI playerHighscoreText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private AudioClip btnSFX;

    public void activateMenu(string titleText, string displayText, string playerHighscoreText, UnityAction StartAction, string startText, UnityAction QuitAction, string quitText)
    {
        this.gameObject.SetActive(true);

        this.titleText.text = titleText;
        this.displayText.text = displayText;
        this.playerHighscoreText.text = playerHighscoreText;

        startButton.GetComponentInChildren<TextMeshProUGUI>().text = startText;
        quitButton.GetComponentInChildren<TextMeshProUGUI>().text = quitText;

        startButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();

        startButton.onClick.AddListener(() =>
        {
            deactivateMenu();
            StartAction();
        });
        quitButton.onClick.AddListener(() =>
        {
            deactivateMenu();
            QuitAction();
        });
    }

    void deactivateMenu()
    {
        soundFXManager.instance.playSoundClip(btnSFX, this.transform, 1f);
        this.gameObject.SetActive(false);
    }
}
