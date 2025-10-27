using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class cutsceneTrigger : MonoBehaviour
{
    [SerializeField] private TimelineAsset cutscene;
    private bool hasTriggered = false;


    private void OnTriggerEnter2D(Collider2D collider)
    {
        // PlayableDirector director = FindObjectOfType<cutsceneManager>().GetComponent<PlayableDirector>();

        if (collider.CompareTag("Player"))
        {
            // Get the singleton instance of the cutscene manager
            cutsceneManager manager = cutsceneManager.GetInstance();

            if (manager != null && manager.director != null && cutscene != null && !hasTriggered)
            {
                manager.director.playableAsset = cutscene;
                manager.director.Play();
                hasTriggered = true;
                Debug.Log(hasTriggered);
            }
            else
            {
                Debug.LogWarning("Manager or director not found, or cutscene asset is not assigned.");
            }
        }
    }
}
