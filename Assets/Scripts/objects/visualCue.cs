using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class visualCue : MonoBehaviour
{
    [SerializeField] private int cueNumber;
    [SerializeField] private Animator animator;

    void Update()
    {
        if (this.gameObject.activeInHierarchy)
        {
            animator.SetInteger("cue", cueNumber);
        }
    }
}
