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

        string sceneName = slot.getPlayerLocationText();
        string sceneToLoad = "";
        if (!isLoadingGame)
        {
            // create a new game - which will initialize our data to a clean state
            dataPersistenceManager.instance.newGame();
            sceneToLoad = new gameData().playerLocation;
            Debug.Log("Starting New Game. Scene to load: " + sceneToLoad);

        }

        // save the game anytime before loading a new scene
        dataPersistenceManager.instance.saveGame();
        // load the scene - which will in turn save the game because of onSceneUnloaded() in the dataPersistenceManager
        SceneManager.LoadSceneAsync("persistentObjects", LoadSceneMode.Single);
        // SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public void onBackClicked()
    {
        mainMenu.activateMenu();
        this.deactivateMenu();
    }

    public void onClearClicked(saveSlot slot)
    {
        dataPersistenceManager.instance.deleteProfileData(slot.getProfileId());
        activateMenu(isLoadingGame);
    }

    public void activateMenu(bool isLoadingGame)
    {
        // set this menu to be active
        this.gameObject.SetActive(true);

        // set mode
        this.isLoadingGame = isLoadingGame;

        // load all of the profiles that exist
        Dictionary<string, gameData> profilesGameData = dataPersistenceManager.instance.getAllProfilesGameData();

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
