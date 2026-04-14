using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questIcon : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite;

    public void setState(questState newState, bool startPoint, bool finishPoint)
    {
        // set appropriate one to active based on the new state
        switch (newState)
        {
            case questState.REQ_NOT_MET: break;

            case questState.CAN_START:
                if (startPoint)
                {
                    // canStartIcon.SetActive(true);
                    animator.Play("exclamation");
                    sprite.color = Color.yellow;
                    sprite.enabled = true;
                }
                break;

            case questState.IN_PROGRESS:
                if (finishPoint)
                {
                    // reqNotMetToFinishIcon.SetActive(true);
                    animator.Play("question");
                    sprite.color = Color.white;
                    sprite.enabled = true;
                }

                break;

            case questState.CAN_FINISH:
                if (finishPoint)
                {
                    // canFinishIcon.SetActive(true);
                    animator.Play("question");
                    sprite.color = Color.yellow;
                    sprite.enabled = true;
                }
                break;

            case questState.FINISHED:
                animator.Play("default");
                sprite.enabled = false;
                break;

            default:
                Debug.LogWarning("quest state not recognized by switch statement for quest icon: " + newState);
                break;
        }
    }
}
