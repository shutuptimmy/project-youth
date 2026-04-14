using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class pathSceneTrigger : MonoBehaviour
{
    public SceneField sceneToLoad;
    public Vector2 newPlayerPos;
    private BoxCollider2D boxCollider;

    private void Reset()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.tag == "Player")
        {
            gameEventsManager.instance.sceneEvents.changeScene(sceneToLoad, newPlayerPos);
        }
    }

}
