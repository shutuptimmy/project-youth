using UnityEngine;

public class visualCue : MonoBehaviour
{
    [SerializeField] private bool isNpc;
    [SerializeField] private Animator animator;

    void Update()
    {
        if (this.gameObject.activeInHierarchy)
        {
            animator.SetBool("isNpc", isNpc);
        }
    }

}
