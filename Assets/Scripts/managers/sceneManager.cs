using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour, IDataPersistence
{
    [Header("Main Configuration")]
    private Vector2 targetedPlayerPos; // for standard location transitions
    public Animator transition { get; private set; }
    public float transitionTime = 1f;
    private string currentBgScene = "";
    private bool isInitialSceneLoaded = false;

    // for minigame variables
    private Vector2 returnPlayerPosition;
    private string returnSceneName;

    [Header("Audio Manager")]
    [SerializeField] private AudioClip mainAudio;
    [SerializeField] private AudioClip minigameAudio;

    private void Awake()
    {
        transition = GetComponentInChildren<Animator>();
        // SceneManager.sceneUnloaded += onSceneUnloaded;
    }

    void Start()
    {
        musicManager.instance.playMusicBG(mainAudio, transform, 1f);
    }

    private void OnEnable()
    {
        gameEventsManager.instance.sceneEvents.onChangeScene += changeScene;
        gameEventsManager.instance.sceneEvents.onPlayCrossFade += playCrossFade;
        gameEventsManager.instance.sceneEvents.onStartMinigame += SwitchToMinigame;
        gameEventsManager.instance.sceneEvents.onQuitMinigame += ReturnFromMinigame;
    }

    private void OnDisable()
    {
        gameEventsManager.instance.sceneEvents.onChangeScene -= changeScene;
        gameEventsManager.instance.sceneEvents.onPlayCrossFade -= playCrossFade;
        gameEventsManager.instance.sceneEvents.onStartMinigame -= SwitchToMinigame;
        gameEventsManager.instance.sceneEvents.onQuitMinigame -= ReturnFromMinigame;
    }

    // private void OnDestroy()
    // {
    //     SceneManager.sceneUnloaded -= onSceneUnloaded;
    // }

    // void onSceneUnloaded(Scene scene)
    // {
    //     // Find the newly spawned player and set its position
    //     GameObject newPlayer = GameObject.FindGameObjectWithTag("Player");
    //     if (newPlayer != null)
    //     {
    //         newPlayer.transform.position = targetedPlayerPos;
    //     }
    //     Debug.Log("unloaded event triggered");
    // }

    public void changeScene(SceneField scene, Vector2 playerPos)
    {
        targetedPlayerPos = playerPos;
        StartCoroutine(loadNewScenery(scene.SceneName));
    }

    void playCrossFade()
    {
        transition.Play("FadeIn");
        gameEventsManager.instance.playerEvents.DisablePlayerMovement();
    }

    // load a scene and position from saved gamedata
    IEnumerator InitializeSceneLoad(string sceneName)
    {
        yield return new WaitForEndOfFrame();

        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        currentBgScene = sceneName;

        // Set the flag to true to prevent future re-triggers
        isInitialSceneLoaded = true;
        dataPersistenceManager.instance.OnSceneLoaded(SceneManager.GetSceneByName(sceneName), LoadSceneMode.Additive);
    }

    IEnumerator loadNewScenery(string sceneName)
    {
        // play fade
        playCrossFade();

        // wait
        yield return new WaitForSeconds(transitionTime);

        // unload the current scene
        yield return SceneManager.UnloadSceneAsync(currentBgScene);

        // load and set the current scene
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        currentBgScene = sceneName;

        // CRITICAL: Wait for the end of the frame so the Player object is spawned and ready
        // yield return new WaitForEndOfFrame();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = targetedPlayerPos;
            Debug.Log($"SCENE MANAGER: Player moved to {targetedPlayerPos} in {sceneName}");
        }

    }

    void SwitchToMinigame()
    {
        StartCoroutine(InitializeMinigameScene());
    }

    void ReturnFromMinigame()
    {
        StartCoroutine(FinishMinigameScene());
    }

    IEnumerator InitializeMinigameScene()
    {
        // 1. Fade Out
        playCrossFade();
        yield return new WaitForSeconds(transitionTime);

        // 2. Save Player Position & Current Scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            returnPlayerPosition = player.transform.position;
        }
        returnSceneName = currentBgScene;

        // 3. Unload the Location Scene
        if (!string.IsNullOrEmpty(currentBgScene))
        {
            Debug.Log($"SCENE MANAGER: Unloading {currentBgScene} for Minigame.");
            yield return SceneManager.UnloadSceneAsync(currentBgScene);
        }

        dataPersistenceManager.instance.MinigameLoadData();
        gameEventsManager.instance.inputEvents.ChangeInputEventContext(inputEventContext.MINIGAME);
        musicManager.instance.playMusicBG(minigameAudio, transform, 1f);
    }

    IEnumerator FinishMinigameScene()
    {
        // 1. Fade Out (Minigame is ending)
        playCrossFade();
        yield return new WaitForSeconds(transitionTime);

        // 2. Load the previous Location Scene
        if (!string.IsNullOrEmpty(returnSceneName))
        {
            Debug.Log($"SCENE MANAGER: Restoring {returnSceneName}.");
            yield return SceneManager.LoadSceneAsync(returnSceneName, LoadSceneMode.Additive);
            currentBgScene = returnSceneName; // Restore tracker
        }

        // Restore Player Position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = returnPlayerPosition;
        }

        gameEventsManager.instance.inputEvents.ChangeInputEventContext(inputEventContext.DEFAULT);
        musicManager.instance.playMusicBG(mainAudio, transform, 1f);
    }

    public void loadData(gameData data)
    {
        currentBgScene = data.playerLocation;

        // load the scene once
        if (!isInitialSceneLoaded) StartCoroutine(InitializeSceneLoad(currentBgScene));
    }

    public void saveData(gameData data)
    {
        data.playerLocation = currentBgScene;
    }
}
