using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class fallingApplesManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject mainContentParent;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameplayGameObject;
    [SerializeField] private PlayerController minigamePlayer;

    [Header("Quest Step")]
    [SerializeField] private gravitationalResearchQuestStep gravitationalResearchQuestStep;
    private bool isQuestStepPresent;

    [Header("Menu Panel")]
    [SerializeField] private minigameMenuPanelUI minigameMenuPanelUI;

    [Header("Lives UI")]
    [SerializeField] private Image[] heartImages; // 3 Heart Image gameobjects based on maxLives
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;

    [Header("Apple Spawn Config")]
    [SerializeField] private objectPooler objectPooler;
    [SerializeField] private Transform poolObjectTransform; // Parent object at top of screen
    [SerializeField] private float spawnXRange;

    [Header("Game Config")]
    [SerializeField] private float timeCompletion = 30f;
    [SerializeField] private float initialSpawnRate = 0.8f;
    [SerializeField] private float maxSpawnRate = 0.08f; // the lower the value, the faster it spawns over time
    [SerializeField] private float initialFallSpeed = 0.1f;
    [SerializeField] private float maxFallSpeed = 1f; // falls faster over time

    private int maxLives = 3;
    private int currentLives;
    private float timeElapsed;
    private bool isGameActive = false;
    private bool playerHasWon = false;

    // Hide the minigame before the crossfade
    IEnumerator Start()
    {
        mainContentParent.SetActive(false);
        gameplayGameObject.SetActive(false);

        gameEventsManager.instance.sceneEvents.startMinigame();
        yield return new WaitForSeconds(1f);

        objectPooler = objectPooler.instance;

        isQuestStepPresent = gravitationalResearchQuestStep == null;
        Debug.Log("Quest Step Status: " + gravitationalResearchQuestStep);

        showStartMenu();
    }

    private void OnEnable()
    {
        gameEventsManager.instance.miscEvents.onPlayerTookDamage += playerHit;
    }

    private void OnDisable()
    {
        gameEventsManager.instance.miscEvents.onPlayerTookDamage -= playerHit;
    }

    void showStartMenu()
    {
        gameplayGameObject.SetActive(true);
        mainContentParent.SetActive(false);

        minigameMenuPanelUI.activateMenu(
            "Dodge the Falling Apples",
            "Survive from falling apples for more than " + timeCompletion + " seconds to pass the game!"
            // TODO: if queststep isn't present, set this string instead
            // "Get the highest possible time record if you can!"
            ,
            "All time record: ", // + GetHighScore()
            () => startMinigame(),
            "Start",
            () => quitMinigameButton(),
            isQuestStepPresent
        );
    }

    void ShowResultMenu(string title, string status)
    {
        bool showQuit = isQuestStepPresent || playerHasWon;
        mainContentParent.SetActive(false);

        minigameMenuPanelUI.activateMenu(
            title,
            status,
            "Time survived: " + timeElapsed,
            () => startMinigame(),
            "Retry",
            () => quitMinigameButton(),
            showQuit
        );
    }

    private void Update()
    {
        if (isGameActive)
        {
            timeElapsed += Time.deltaTime;
            timerText.text = Mathf.FloorToInt(timeElapsed).ToString() + "s";
        }
    }

    public void startMinigame()
    {
        mainContentParent.SetActive(true);
        // reset game state when retry
        currentLives = maxLives;
        timeElapsed = 0f;
        minigamePlayer.transform.position = new Vector2(0, -0.35f);
        isGameActive = true;
        updateLives();
        StartCoroutine(SpawnerRoutine());

        gameEventsManager.instance.playerEvents.EnablePlayerMovement();
    }

    IEnumerator quitMinigame()
    {
        Debug.Log("suceess");
        gameEventsManager.instance.sceneEvents.quitMinigame();
        yield return new WaitForSeconds(1f);

        gravitationalResearchQuestStep?.playerWon();
        gameEventsManager.instance.playerEvents.EnablePlayerMovement();

        Destroy(gameObject);
    }

    public void quitMinigameButton()
    {
        StartCoroutine(quitMinigame());
    }

    void minigameComplete(bool playerWon)
    {
        gameEventsManager.instance.playerEvents.DisablePlayerMovement();
        StopCoroutine(SpawnerRoutine());
        isGameActive = false;

        mainContentParent.SetActive(false);

        if (playerWon)
        {
            playerHasWon = true;
            ShowResultMenu("Game completed!", "You won!");
        }
        else ShowResultMenu("You Lost!", "Try again!");
    }

    void spawnApple(float speed)
    {
        float randX = Random.Range(-spawnXRange, spawnXRange);
        Vector2 spawnPos = new Vector2(randX, poolObjectTransform.position.y);

        GameObject appleObj = objectPooler.spawnFromPool("apple", spawnPos, Quaternion.identity);

        appleObj.GetComponent<apple>().setup(speed);
    }

    void playerHit()
    {
        if (!isGameActive) return;

        currentLives--;
        updateLives();
    }

    void updateLives()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentLives) heartImages[i].sprite = fullHeartSprite;
            else heartImages[i].sprite = emptyHeartSprite;
        }

        if (currentLives <= 0) checkGameState();
    }

    private void checkGameState()
    {
        if (timeElapsed >= timeCompletion) minigameComplete(true);
        else if (timeElapsed < timeCompletion) minigameComplete(false);
    }

    IEnumerator SpawnerRoutine()
    {
        while (isGameActive)
        {
            // --- DIFFICULTY LOGIC ---
            // Calculate progress (0.0 to 1.0)
            float progress = Mathf.Clamp01(timeElapsed / 60f);

            // Ease In curve
            float curvedProgress = progress * progress;

            // Calculate current difficulty values using Linear Interpolation (Lerp)
            float currentSpawnRate = Mathf.Lerp(initialSpawnRate, maxSpawnRate, curvedProgress);
            float currentFallSpeed = Mathf.Lerp(initialFallSpeed, maxFallSpeed, curvedProgress);

            spawnApple(currentFallSpeed);
            yield return new WaitForSeconds(currentSpawnRate);
        }
    }
}