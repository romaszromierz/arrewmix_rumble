using UnityEngine;

public class BeatIndicator : MonoBehaviour
{
    public GameObject targetObject; // Assign your target GameObject in the Inspector

    private Animator animator;

    void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target GameObject is not assigned in the Inspector.");
            enabled = false; // Disable the script to prevent further errors
            return;
        }

        animator = targetObject.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the target GameObject.");
            enabled = false; // Disable the script to prevent further errors
            return;
        }
    }

    public void HighlightBeat()
    {
        if (animator == null)
        {
            Debug.LogError("Animator is null in HighlightBeat.");
            return;
        }

        animator.Play("Beat");
    }
}