using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DoorInteraction : InteractableBase
{
    public SceneField sceneToLoad;
    public Vector2 newPlayerPos;

    private BoxCollider2D boxCollider;

    public override void Interact()
    {
        gameEventsManager.instance.sceneEvents.changeScene(sceneToLoad, newPlayerPos);
    }

    private void Reset()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
    }
}
