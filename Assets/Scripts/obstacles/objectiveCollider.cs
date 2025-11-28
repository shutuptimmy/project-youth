using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectiveCollider : MonoBehaviour
{
    [Header("Quest Config")]
    [SerializeField] private pushableBox specificBox;
    [SerializeField] private moveTheBoxQuestStep questStep;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the object entering is the specific box we are looking for
        if (collider.gameObject == specificBox.gameObject)
        {
            // 1. Convert the box to a static background object
            // specificBox.ConvertToBackgroundObject();

            // 2. Finish the quest step
            questStep?.isFinishedMoving();

            // 3. Optional: Disable this trigger so it doesn't fire again
            this.gameObject.SetActive(false);
        }
    }


}
