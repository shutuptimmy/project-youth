using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class pathSceneTrigger : MonoBehaviour
{
    public SceneField sceneToLoad;
    public Vector2 newPlayerPos;
    [SerializeField] private GameObject interactableVisualCue;


    private GameObject player;

    private BoxCollider2D boxCollider;
    private void Reset()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        interactableVisualCue.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.tag == "Player")
        {
            gameEventsManager.instance.sceneEvents.changeScene(sceneToLoad, newPlayerPos);
        }
    }

}
