using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainMenu : menu
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;

    [Header("Menu Navigations")]
    [SerializeField] private saveSlotsMenu saveSlotsMenu;

    private void Start()
    {
        newGameButton.onClick.RemoveAllListeners();
        loadGameButton.onClick.RemoveAllListeners();
        if (dataPersistenceManager.instance.isDataATest())
        {
            newGameButton.onClick.AddListener(() =>
            {
                dataPersistenceManager.instance.newGame("test", 0);
                saveSlotsMenu.saveGameAndLoadScene();
            });

            loadGameButton.onClick.AddListener(() => saveSlotsMenu.saveGameAndLoadScene());
        }
        else
        {
            newGameButton.onClick.AddListener(() => startGame());
            loadGameButton.onClick.AddListener(() => onLoadGameClicked());
        }
        disableButtonsDependingOnData();
    }

    void startGame()
    {
        saveSlotsMenu.activateMenu(false);
        this.deactivateMenu();
    }

    void disableButtonsDependingOnData()
    {
        if (!dataPersistenceManager.instance.hasGameData())
        {
            loadGameButton.interactable = false;
        }
    }

    void onLoadGameClicked()
    {
        saveSlotsMenu.activateMenu(true);
        this.deactivateMenu();
    }

    public void activateMenu()
    {
        this.gameObject.SetActive(true);
        disableButtonsDependingOnData();
    }

    public void deactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
