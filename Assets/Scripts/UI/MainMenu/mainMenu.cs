using UnityEngine;
using UnityEngine.UI;

public class mainMenu : menu
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private AudioClip btnSFX;

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
        soundFXManager.instance.playSoundClip(btnSFX, this.transform, 1f);
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
        soundFXManager.instance.playSoundClip(btnSFX, this.transform, 1f);
        saveSlotsMenu.activateMenu(true);
        this.deactivateMenu();
    }

    public void activateMenu()
    {
        soundFXManager.instance.playSoundClip(btnSFX, this.transform, 1f);
        this.gameObject.SetActive(true);
        disableButtonsDependingOnData();
    }

    public void deactivateMenu()
    {
        soundFXManager.instance.playSoundClip(btnSFX, this.transform, 1f);
        this.gameObject.SetActive(false);
    }

    public void onQuitPressed()
    {
        soundFXManager.instance.playSoundClip(btnSFX, this.transform, 1f);
        Application.Quit();
    }
}
