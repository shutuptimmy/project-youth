using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainMenu : menu
{
    [Header("Menu Navigations")]
    [SerializeField] private saveSlotsMenu saveSlotsMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;


    [Header("Components")]
    [SerializeField] private GameObject loadingBarObject;
    [SerializeField] private Image loadingBar;
    [SerializeField] private SceneField persistentObjects;
    [SerializeField] private SceneField sceneToLoad;
    private List<AsyncOperation> loadScenes = new List<AsyncOperation>();

    // [SerializeField] private Button createProfileButton;
    // public newProfile newProfileUI;
    // [SerializeField] private GameObject loadProfileUI;

    void Awake()
    {
        loadingBarObject.SetActive(false);
    }

    private void Start()
    {
        if (!dataPersistenceManager.instance.hasGameData())
        {
            loadGameButton.interactable = false;
        }
    }

    // public void createProfile()
    // {
    //     newProfileUI.newProfilePanel.SetActive(true);
    // }

    // public void loadProfiles()
    // {
    //     loadProfileUI.SetActive(true);
    // }

    public void startGame()
    {
        // disableMenuButtons();
        // dataPersistenceManager.instance.newGame();
        saveSlotsMenu.activateMenu(false);
        this.deactivateMenu();

        // loadingBarObject.SetActive(true);
        // loadScenes.Add(SceneManager.LoadSceneAsync(persistentObjects));
        // loadScenes.Add(SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive));

        // StartCoroutine(progressLoadingBar());

        // gameObject.SetActive(false);

        // // Find the GameManager and tell it to load the rest of the game scenes.
        // // Assumes your GameManager is already a persistent singleton.
        // SceneManager.LoadScene(persistentSceneName, LoadSceneMode.Additive);
    }

    public void onLoadGameClicked()
    {
        saveSlotsMenu.activateMenu(true);
        this.deactivateMenu();
    }

    public void loadGame()
    {
        disableMenuButtons();
        dataPersistenceManager.instance.loadGame();
        loadingBarObject.SetActive(true);

        loadScenes.Add(SceneManager.LoadSceneAsync(persistentObjects));
        loadScenes.Add(SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive));
        StartCoroutine(progressLoadingBar());
    }

    private IEnumerator progressLoadingBar()
    {
        float loadProgress = 0f;
        for (int i = 0; i < loadScenes.Count; i++)
        {
            while (!loadScenes[i].isDone)
            {
                loadProgress += loadScenes[i].progress;
                loadingBar.fillAmount = loadProgress / loadScenes.Count;
                yield return null;
            }
        }
    }

    void disableMenuButtons()
    {
        newGameButton.interactable = false;
        loadGameButton.interactable = false;
    }

    public void activateMenu()
    {
        this.gameObject.SetActive(true);
    }

    public void deactivateMenu()
    {
        this.gameObject.SetActive(false);

    }
}
