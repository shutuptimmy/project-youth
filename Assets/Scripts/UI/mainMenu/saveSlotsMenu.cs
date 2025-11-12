using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class saveSlotsMenu : menu
{
    [Header("Navigation Menu")]
    [SerializeField] private mainMenu mainMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;
    [Header("Confirmation Popup Menu")]
    [SerializeField]
    private confirmationPopupMenu confirmationPopupMenu;

    private saveSlot[] saveSlots;

    private bool isLoadingGame = false;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<saveSlot>();
    }

    public void onSaveSlotClicked(saveSlot slot)
    {

        // disable all buttons
        disableMenuButtons();

        // update the selected profile id to be used for data persistence
        dataPersistenceManager.instance.changeSelectedProfileId(slot.getProfileId());

        string sceneToLoad = ""; // check the default bg location when creating new game data
        // case - loading game
        if (isLoadingGame)
        {
            // create a new game - which will initialize our data to a clean state
            dataPersistenceManager.instance.changeSelectedProfileId(slot.getProfileId());
            saveGameAndLoadScene();
            sceneToLoad = new gameData().playerLocation;
            Debug.Log("Starting New Game. Scene to load: " + sceneToLoad);

        }
        // case - new game, but the save slot has data
        else if (slot.hasData)
        {
            confirmationPopupMenu.activateMenu(
                "Starting a new game with this slot will override the existing data. Are you sure?",
                // function to execute if clicked "Yes"
                () =>
                {
                    dataPersistenceManager.instance.changeSelectedProfileId(slot.getProfileId());
                    dataPersistenceManager.instance.newGame();
                    saveGameAndLoadScene();
                },
                // function to execute if clicked "No"
                () =>
                {
                    this.activateMenu(isLoadingGame);
                }
            );
        }
        // case - new game if slot has no data
        else
        {
            dataPersistenceManager.instance.changeSelectedProfileId(slot.getProfileId());
            dataPersistenceManager.instance.newGame();
            saveGameAndLoadScene();
        }
    }

    void saveGameAndLoadScene()
    {
        // save the game anytime before loading a new scene
        dataPersistenceManager.instance.saveGame();
        // load the scene
        SceneManager.LoadSceneAsync("persistentObjects", LoadSceneMode.Single);
    }

    public void onBackClicked()
    {
        mainMenu.activateMenu();
        this.deactivateMenu();
    }

    public void onClearClicked(saveSlot slot)
    {
        confirmationPopupMenu.activateMenu(
                "Are you sure you want to delete this saved data?",
                // function to execute if clicked "Yes"
                () =>
                {
                    dataPersistenceManager.instance.deleteProfileData(slot.getProfileId());
                    activateMenu(isLoadingGame);
                },
                // function to execute if clicked "No"
                () =>
                {
                    activateMenu(isLoadingGame);
                }
            );
    }

    public void activateMenu(bool isLoadingGame)
    {
        // set this menu to be active
        this.gameObject.SetActive(true);

        // set mode
        this.isLoadingGame = isLoadingGame;

        // load all of the profiles that exist
        Dictionary<string, gameData> profilesGameData = dataPersistenceManager.instance.getAllProfilesGameData();

        // ensure the back button is enabled when we activate the menu
        backButton.interactable = true;

        // loop through each save slot in the UI and set the content appropriately
        GameObject firstSelected = backButton.gameObject;

        foreach (saveSlot slot in saveSlots)
        {
            gameData profileData = null;
            profilesGameData.TryGetValue(slot.getProfileId(), out profileData);
            slot.setData(profileData);
            if (profileData == null && isLoadingGame)
            {
                slot.setInteractable(false);
            }
            else
            {

                slot.setInteractable(true);
                if (firstSelected.Equals(backButton.gameObject))
                {
                    firstSelected = slot.gameObject;
                }
            }
        }
        // set the first selected button
        Button firstSelectedButton = firstSelected.GetComponent<Button>();
        this.setFirstSelected(firstSelectedButton);
    }


    public void deactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    void disableMenuButtons()
    {
        foreach (saveSlot slot in saveSlots)
        {
            slot.setInteractable(false);
        }
        backButton.interactable = false;
    }
}
