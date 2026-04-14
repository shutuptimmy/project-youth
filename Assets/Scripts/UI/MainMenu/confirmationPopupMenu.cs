using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class confirmationPopupMenu : menu
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private AudioClip btnSFX;

    public void activateMenu(string displayText, UnityAction confirmAction, UnityAction cancelAction)
    {
        this.gameObject.SetActive(true);

        // set display text
        this.displayText.text = displayText;

        // remove any existing listeners
        // note - this only removes listeners added through code
        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        // assign onClick listeners
        confirmButton.onClick.AddListener(() =>
        {
            deactivateMenu();
            confirmAction();
        });
        cancelButton.onClick.AddListener(() =>
        {
            deactivateMenu();
            cancelAction();
        });
    }

    void deactivateMenu()
    {
        soundFXManager.instance.playSoundClip(btnSFX, this.transform, 1f);
        this.gameObject.SetActive(false);
    }
}
