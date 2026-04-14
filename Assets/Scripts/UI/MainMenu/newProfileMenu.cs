using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class newProfileMenu : menu
{
    [Header("Components")]
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private TextMeshProUGUI placeholderName;
    [SerializeField] private Button backButton;
    [SerializeField] private Animator playerPortrait;
    [SerializeField] private Animator playerCharacter;

    [SerializeField] private confirmationPopupMenu confirmationPopupMenu;
    [SerializeField] private saveSlotsMenu saveSlotsMenu;
    [SerializeField] private AudioClip btnSFX;

    // defualt variables for creating the profile
    private int playerGender; // 0 = boy, 1 = girl;


    public void onCreateProfileClicked()
    {
        soundFXManager.instance.playSoundClip(btnSFX, this.transform, 1f);
        if (playerNameInput.text == "")
        {
            confirmationPopupMenu.activateMenu(
                $"Your name is empty. You will be named \"{placeholderName.text}\" by default. Continue?",
                () => storeProfileToData(placeholderName.text, playerGender),
                () => { }
            );
        }
        else storeProfileToData(playerNameInput.text, playerGender);


    }

    void storeProfileToData(string playerName, int playerGender)
    {
        dataPersistenceManager.instance.newGame(playerName, playerGender);
        saveSlotsMenu.saveGameAndLoadScene();
    }

    public void onBackClicked()
    {
        soundFXManager.instance.playSoundClip(btnSFX, this.transform, 1f);
        this.deactivateMenu();
        saveSlotsMenu.activateMenu(false);
    }

    public void girlGender()
    {
        placeholderName.text = "Jane";
        playerGender = 1;
        playerPortrait.Play("mcGirl");
        playerCharacter.Play("playerGirlIdle");

    }

    public void boyGender()
    {
        placeholderName.text = "John";
        playerGender = 0;
        playerPortrait.Play("mcBoy");
        playerCharacter.Play("playerBoyIdle");
    }

    public void activateMenu()
    {
        soundFXManager.instance.playSoundClip(btnSFX, this.transform, 1f);
        this.gameObject.SetActive(true);
        saveSlotsMenu.deactivateMenu();

        // automatically selected as a default
        boyGender();
    }

    public void deactivateMenu()
    {
        soundFXManager.instance.playSoundClip(btnSFX, this.transform, 1f);
        this.gameObject.SetActive(false);

        // reset the value after closing
        playerNameInput.text = "";
    }

}

