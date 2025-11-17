using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class newProfileMenu : menu
{
    [Header("Components")]
    // todo: get input from playerNameInput
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private TextMeshProUGUI placeholderName;
    [SerializeField] private Button backButton;
    [SerializeField] private Animator playerPortrait;
    [SerializeField] private Animator playerCharacter;

    [SerializeField] private confirmationPopupMenu confirmationPopupMenu;
    [SerializeField] private saveSlotsMenu saveSlotsMenu;

    // defualt variables for creating the profile
    private int playerGender; // 0 = boy, 1 = girl;


    public void onCreateProfileClicked()
    {
        if (playerNameInput.text == "")
        {
            confirmationPopupMenu.activateMenu(
                "You name is empty. You will be named \"" + placeholderName.text + "\" by default. Continue?",
                () =>
                {
                    // create profile with inputs below
                    dataPersistenceManager.instance.newGame(placeholderName.text, playerGender);
                    dataPersistenceManager.instance.saveGame();
                    this.deactivateMenu();

                    // reload the saved slots from newly created slot
                    dataPersistenceManager.instance.loadGame();
                    saveSlotsMenu.activateMenu(true);
                },
                () => { saveSlotsMenu.deactivateMenu(); } // simply close the confirmation popup when clicking cancel.
            );
        }
        else
        {
            dataPersistenceManager.instance.newGame(playerNameInput.text, playerGender);
            dataPersistenceManager.instance.saveGame();
            this.deactivateMenu();
            saveSlotsMenu.activateMenu(true);
        }


    }

    public void onBackClicked()
    {
        this.deactivateMenu();
        saveSlotsMenu.activateMenu(false);
    }

    public void girlGender()
    {

        placeholderName.text = "Jane";
        playerGender = 1;
        playerPortrait.Play("playerGirlNormal");
        playerCharacter.Play("playerGirlIdle");

    }

    public void boyGender()
    {
        placeholderName.text = "John";
        playerGender = 0;
        playerPortrait.Play("playerBoyNormal");
        playerCharacter.Play("playerBoyIdle");
    }

    public void activateMenu()
    {
        this.gameObject.SetActive(true);
        saveSlotsMenu.deactivateMenu();

        // automatically selected as a default
        boyGender();
    }

    public void deactivateMenu()
    {
        this.gameObject.SetActive(false);

        // reset the value after closing
        playerNameInput.text = "";
    }

}

