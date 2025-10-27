using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class playerManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private int startingLevel = 1;
    [SerializeField] private int startingExperience = 0;

    private int currentLevel;
    private int currentExperience;

    private void Awake()
    {
        currentLevel = startingLevel;
        currentExperience = startingExperience;
    }

    private void OnEnable()
    {
        gameEventsManager.instance.playerEvents.onExperienceGained += ExperienceGained;
    }

    private void OnDisable()
    {
        gameEventsManager.instance.playerEvents.onExperienceGained -= ExperienceGained;
    }

    private void Start()
    {
        gameEventsManager.instance.playerEvents.PlayerLevelChange(currentLevel);
        gameEventsManager.instance.playerEvents.PlayerExperienceChange(currentExperience);
    }

    private void ExperienceGained(int experience)
    {
        currentExperience += experience;
        // check if we're ready to level up
        while (currentExperience >= 100)
        {
            currentExperience -= 100;
            currentLevel++;
            gameEventsManager.instance.playerEvents.PlayerLevelChange(currentLevel);
        }
        gameEventsManager.instance.playerEvents.PlayerExperienceChange(currentExperience);
    }

    // public class playerManager : MonoBehaviour
    // {
    //     // public PlayerController2D playerController { get; private set; }

    //     // [Header("Rapport Status")]
    //     // Christopher
    //     // public int npc1Level;
    //     // public int npc1Exp;

    //     // April
    //     // public int npc2Level;
    //     // public int npc2Exp;

    //     // Harris
    //     // public int npc3Level;
    //     // public int npc3Exp;

    //     // Weasel
    //     // public int npc4Level;
    //     // public int npc4Exp;


    //     public static playerManager instance;
    //     public static playerManager GetInstance()
    //     {
    //         return instance;
    //     }

    //     private void Awake()
    //     {
    //         if (instance != null)
    //         {
    //             Destroy(gameObject);
    //             return;
    //         }
    //         instance = this;
    //     }


    //     private void Start()
    //     {
    //         // playerController = FindObjectOfType<PlayerController2D>();
    //         // if (playerController == null)
    //         // {
    //         //     Debug.LogError("Player object with PlayerController2D script is missin' bro.");
    //         // }
    //     }
    // }

}