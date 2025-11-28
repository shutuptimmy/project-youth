using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainMenu : menu
{
    [Header("Menu Buttons")]
    [SerializeField] private Button loadGameButton;

    [Header("Menu Navigations")]
    [SerializeField] private saveSlotsMenu saveSlotsMenu;

    private void Start()
    {
        disableButtonsDependingOnData();
    }

    public void startGame()
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

    public void onLoadGameClicked()
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
