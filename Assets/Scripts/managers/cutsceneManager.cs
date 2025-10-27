using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class cutsceneManager : MonoBehaviour
{
    public PlayableDirector director { get; private set; }


    private static cutsceneManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one cutscene manager in the scene. Removing duplicate...");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

    }

    public static cutsceneManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        director = GetComponent<PlayableDirector>();

    }
}
