using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questIcon : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite;
    // [SerializeField] private GameObject canStartIcon;
    // [SerializeField] private GameObject reqNotMetToFinishIcon;
    // [SerializeField] private GameObject canFinishIcon;

    public void setState(questState newState, bool startPoint, bool finishPoint)
    {

        // set all inactive
        // canStartIcon.SetActive(false);
        // reqNotMetToFinishIcon.SetActive(false);
        // canFinishIcon.SetActive(false);

        // set appropriate one to active based on the new state
        switch (newState)
        {

            case questState.CAN_START:
                if (startPoint)
                {
                    // canStartIcon.SetActive(true);
                    animator.Play("exclamation");
                    sprite.color = Color.yellow;
                }
                break;

            case questState.IN_PROGRESS:
                if (finishPoint)
                {
                    // reqNotMetToFinishIcon.SetActive(true);
                    animator.Play("question");
                    sprite.color = Color.white;
                }

                break;

            case questState.CAN_FINISH:
                if (finishPoint)
                {
                    // canFinishIcon.SetActive(true);
                    animator.Play("question");
                    sprite.color = Color.yellow;
                }
                break;

            case questState.FINISHED:
                animator.Play("default");
                break;

            default:
                Debug.LogWarning("quest state not recognized by switch statement for quest icon: " + newState);
                break;
        }
    }
}
