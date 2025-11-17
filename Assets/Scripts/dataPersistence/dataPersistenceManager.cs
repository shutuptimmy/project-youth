using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool initializeDataIfNull = false;
    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";


    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    private gameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private fileDataHandler dataHandler;
    private string selectedProfileId = "";

    public static dataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Data Persistence manager in the scene. Destroying newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        if (disableDataPersistence)
        {
            Debug.LogWarning("data persistence is currently disabled.");
        }

        this.dataHandler = new fileDataHandler(Application.persistentDataPath, fileName);
        if (overrideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("overrode selected profile id with test id: " + testSelectedProfileId);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Onsceneloaded");
        this.dataPersistenceObjects = findAllDataPersistenceObjects();
        // Debug.LogWarning(gameData);
        loadGame();

    }

    public void changeSelectedProfileId(string newProfileId)
    {
        // update the profile id for saving and loading
        this.selectedProfileId = newProfileId;
        // load the game, which will use that profile, updating our gamedata accordingly
        loadGame();
    }

    public void deleteProfileData(string profileId)
    {
        // delete data for the profile id
        dataHandler.delete(profileId);
        // reload the game so that our data matches to newly selected profile Id
        loadGame();
    }

    public void newGame(string playerName, int playerGender)
    {
        this.gameData = new gameData(playerName, playerGender);
    }
    public void loadGame()
    {
        // return if disabled
        if (disableDataPersistence)
        {
            return;
        }

        // load any game data using data handler
        this.gameData = dataHandler.load(selectedProfileId);

        // start new game if data is null and we're configured to initialize data for debugging purposes
        if (this.gameData == null && initializeDataIfNull)
        {
            newGame("testWithDataNull", 0);
        }

        // if no data, don't continue
        if (this.gameData == null)
        {
            Debug.Log("No data was found. New game needs to be started before data can be loaded.");
            return;
        }

        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.loadData(gameData);
        }

    }
    public void saveGame()
    {
        // return if disabled
        if (disableDataPersistence)
        {
            return;
        }

        // if no data to save, log warning here
        if (this.gameData == null)
        {
            Debug.LogWarning("no data found. New game needs to be started before data can be saved.");
            return;
        }

        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.saveData(gameData);
        }

        // save that data to a file using data handler
        dataHandler.save(gameData, selectedProfileId);
    }

    private void OnApplicationQuit()
    {
        if (overrideSelectedProfileId)
        {
            saveGame();
        }
    }

    private List<IDataPersistence> findAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool hasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, gameData> getAllProfilesGameData()
    {
        return dataHandler.loadAllProfiles();
    }
}
