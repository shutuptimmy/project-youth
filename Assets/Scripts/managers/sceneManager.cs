using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour, IDataPersistence
{
    private Vector2 newPlayerPos;
    public Animator transition { get; private set; }
    public float transitionTime = 1f;
    private string currentBgScene = "";
    private bool isInitialSceneLoaded = false;

    private void Awake()
    {

        transition = GetComponentInChildren<Animator>();

        SceneManager.sceneUnloaded += onSceneUnloaded;
    }

    private void OnEnable()
    {
        gameEventsManager.instance.sceneEvents.onChangeScene += changeScene;
    }

    private void OnDisable()
    {
        gameEventsManager.instance.sceneEvents.onChangeScene -= changeScene;
    }

    private void OnDestroy()
    {

        SceneManager.sceneUnloaded -= onSceneUnloaded;
    }

    public void changeScene(SceneField scene, Vector2 playerPos)
    {
        // IMPORTANT: We use the SceneField's string conversion implicitly
        StartCoroutine(loadNewScene(scene.SceneName));
        newPlayerPos = playerPos;
    }

    // NEW: Coroutine to handle the initial scene load after data is available.
    IEnumerator InitializeSceneLoad(string sceneName)
    {
        // Wait for the end of the frame to ensure all Awake/Start methods have run
        // after persistentObjects was loaded.
        yield return new WaitForEndOfFrame();

        if (!string.IsNullOrEmpty(sceneName))
        {
            Debug.Log($"SCENE MANAGER: Triggering initial load of scene: {sceneName}");
            // Load the first scene additively, no transition needed yet.
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            // Set the current tracked scene name.
            currentBgScene = sceneName;
            // Set the flag to true to prevent future re-triggers
            isInitialSceneLoaded = true;
        }
    }

    IEnumerator loadNewScene(string sceneName)
    {
        // play fade
        transition.Play("FadeIn");

        // wait
        yield return new WaitForSeconds(transitionTime);

        if (!string.IsNullOrEmpty(currentBgScene))
        {
            Debug.Log($"SCENE MANAGER: Unloading scene: {currentBgScene}");
            yield return SceneManager.UnloadSceneAsync(currentBgScene);
        }
        // SceneManager.UnloadSceneAsync(currentBgScene);

        // load scene and set the currentBgScene
        Debug.Log($"SCENE MANAGER: Loading new scene: {sceneName}");
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        // SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // update the tracker
        currentBgScene = sceneName;
        // currentBgScene = scene.SceneName;
        Debug.Log("updated scene: " + currentBgScene);

    }

    void onSceneUnloaded(Scene scene)
    {
        // Find the newly spawned player and set its position
        GameObject newPlayer = GameObject.FindGameObjectWithTag("Player");
        if (newPlayer != null)
        {
            newPlayer.transform.position = newPlayerPos;
        }
        Debug.Log("unloaded event triggered");
    }

    public void loadData(gameData data)
    {
        currentBgScene = data.playerLocation;

        if (!isInitialSceneLoaded)
        {
            StartCoroutine(InitializeSceneLoad(currentBgScene));
        }
    }

    public void saveData(gameData data)
    {
        if (!string.IsNullOrEmpty(currentBgScene))
        {
            data.playerLocation = currentBgScene;
        }
    }
}
