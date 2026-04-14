using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class fallingApplesManager : MinigameManagerBase
{
    [Header("Player Script")]
    [SerializeField] private PlayerController minigamePlayer;

    [Header("Quest Step")]
    [SerializeField] private gravitationalResearchQuestStep gravitationalResearchQuestStep;

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

    // Hide the minigame before the crossfade
    public override void StartQuestStatus()
    {
        isQuestStepPresent = gravitationalResearchQuestStep != null;
    }

    private void OnEnable()
    {
        gameEventsManager.instance.playerEvents.onPlayerTookDamage += playerHit;
    }

    private void OnDisable()
    {
        gameEventsManager.instance.playerEvents.onPlayerTookDamage -= playerHit;
    }

    private void Update()
    {
        if (isGameActive)
        {
            timeElapsed += Time.deltaTime;
            timerText.text = Mathf.FloorToInt(timeElapsed).ToString() + "s";
        }
    }

    public override void ShowStartMenu()
    {
        minigameMenuPanelUI.activateMenu(
            "Dodge the Falling Apples",
            "Survive from falling apples for more than " + timeCompletion + " seconds to pass the game!"
            // TODO: if queststep isn't present, set this string instead
            // "Get the highest possible time record if you can!"
            ,
            "All time record: ", // + GetHighScore()
            () => StartMinigameBase(),
            "Start",
            () => QuitMinigameBtn(),
            "Exit Minigame"
        );
    }

    public override void ShowResultMenu(string title, string status)
    {
        minigameMenuPanelUI.activateMenu(
            title,
            status,
            "Time survived: " + timeElapsed,
            () => StartMenuBase(),
            "Retry",
            () => QuitMinigameBtn(),
            (playerHasWon && isQuestStepPresent)? "Complete Quest" : "Exit Minigame"
        );
    }

    public override void StartMinigame()
    {
        // reset game state when retry
        currentLives = maxLives;
        timeElapsed = 0f;
        minigamePlayer.transform.position = new Vector2(0, -0.35f);
        isGameActive = true;
        updateLives();
        StartCoroutine(SpawnerRoutine());
    }

    public override void QuitMinigame()
    {
        gravitationalResearchQuestStep?.playerWon(playerHasWon);
    }

    public override void MinigameComplete(bool resultCheck)
    {
        StopCoroutine(SpawnerRoutine());

        if (resultCheck) ResultMenuBase("Game completed!", "You won!");
        else ResultMenuBase("You Lost!", "Try again!");
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
        if (timeElapsed >= timeCompletion) MinigameCompleteBase(true);
        else if (timeElapsed < timeCompletion) MinigameCompleteBase(false);
    }

    IEnumerator SpawnerRoutine()
    {
        while (isGameActive)
        {
            // --- DIFFICULTY LOGIC ---
            // Calculate progress (0.0 to 1.0)
            float progress = Mathf.Clamp01(timeElapsed / 25f);

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