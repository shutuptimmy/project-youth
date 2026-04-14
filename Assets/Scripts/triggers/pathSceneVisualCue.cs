using UnityEngine;

public class pathSceneVisualCue : MonoBehaviour
{
    [SerializeField] private GameObject interactableVisualCue;
    private BoxCollider2D boxCollider;

    void Start()
    {
        interactableVisualCue.SetActive(false);
    }

    private void Reset()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player") interactableVisualCue.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player") interactableVisualCue.SetActive(false);
    }
}
