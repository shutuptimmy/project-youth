using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    private Vector2 newPlayerPos;
    public Animator transition { get; private set; }
    public float transitionTime = 1f;
    [SerializeField] private string currentBgScene = "";

    private List<AsyncOperation> loadScenes = new List<AsyncOperation>();

    private void Awake()
    {

        transition = GetComponentInChildren<Animator>();
        // Subscribe to the scene loaded event
        // SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += onSceneUnloaded;

        // **Important:** Initialize with the name of your default or first loaded background scene
        // Assuming the scene this manager is in (the persistent scene) is *not* the initial background.
        // If your initial background scene is loaded additively at the start, you may need to set this here.
        // For simplicity, let's assume the first call to changeScene sets this.

        Debug.Log(SceneManager.GetActiveScene().name);
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
        StartCoroutine(loadScene(scene));
        newPlayerPos = playerPos;
    }

    IEnumerator loadScene(SceneField scene)
    {
        // play fade
        transition.Play("FadeIn");

        // wait
        yield return new WaitForSeconds(transitionTime);
        // load scene
        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        // TODO: Get the scene name instead the persistentobject
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);


        // transition.Play("FadeOut");

        // --------------------------------
        // 2. Unload the previous background scene (if one was loaded)
        // if (!string.IsNullOrEmpty(currentBgScene))
        // {
        //     // Unload the old scene using the stored name
        //     yield return SceneManager.UnloadSceneAsync(currentBgScene);
        //     Debug.Log($"Unloaded scene: {currentBgScene}");
        // }

        // 3. Load the new scene additively
        // AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.SceneName, LoadSceneMode.Additive);

        // Wait until the new scene is fully loaded
        // while (!asyncLoad.isDone)
        // {
        //     yield return null;
        // }

        // 4. Update the tracker for the current background scene
        // currentBgScene = scene.SceneName;

        // 5. Optionally, set the new scene as the active scene if that is required for lighting/physics
        // If the persistent scene handles all core logic, this might not be strictly necessary.
        // SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene.SceneName));
        // --------------------------------

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

    // public static sceneManager GetInstance()
    // {
    //     return instance;
    // }

    // private void OnDestroy()
    // {
    //     // Unsubscribe to prevent memory leaks
    //     SceneManager.sceneLoaded -= OnSceneLoaded;
    // }

    // private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     // Find the newly spawned player and set its position
    //     GameObject newPlayer = GameObject.FindGameObjectWithTag("Player");
    //     if (newPlayer != null)
    //     {
    //         newPlayer.transform.position = newPlayerPos;
    //     }
    // }
}
