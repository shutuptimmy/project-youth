using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainMenuManager : MonoBehaviour
{
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
        loadingBarObject.SetActive(true);

        loadScenes.Add(SceneManager.LoadSceneAsync(persistentObjects));
        loadScenes.Add(SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive));

        StartCoroutine(progressLoadingBar());

        // gameObject.SetActive(false);

        // // Find the GameManager and tell it to load the rest of the game scenes.
        // // Assumes your GameManager is already a persistent singleton.
        // SceneManager.LoadScene(persistentSceneName, LoadSceneMode.Additive);
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
}
