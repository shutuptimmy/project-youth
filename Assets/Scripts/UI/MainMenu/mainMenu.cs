using UnityEngine;
using UnityEngine.UI;

public class mainMenu : menu
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button optionsButton;

    [Header("Menu Navigations")]
    [SerializeField] private saveSlotsMenu saveSlotsMenu;
    [SerializeField] private soundsMenuUI soundsMenuUI;

    [Header("Music Background")]
    [SerializeField] private AudioClip musicBG;

    private void Start()
    {
        musicManager.instance.playMusicBG(musicBG, transform, 1f);

        newGameButton.onClick.RemoveAllListeners();
        loadGameButton.onClick.RemoveAllListeners();
        optionsButton.onClick.RemoveAllListeners();

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

        optionsButton.onClick.AddListener(() => soundsMenuUI.activateMenu());
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

    public void onQuitPressed()
    {
        Application.Quit();
    }
}
