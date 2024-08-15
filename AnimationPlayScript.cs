using UnityEngine;
using UnityEngine.UI;

public class AnimationPlayScript : MonoBehaviour
{
    public Slider slider;

    private bool IsCombo = false;

    private void Update()
    {
        if (Mathf.Approximately(slider.value, slider.maxValue))
        {
            SetTriggersForChildren(true);
        }
        else if (Mathf.Approximately(slider.value, 0f))
        {
            SetTriggersForChildren(false);
        }
    }

    private void SetTriggersForChildren(bool isCombo)
    {
        foreach (Transform child in transform)
        {
            Animator childAnimator = child.GetComponent<Animator>();

            if (childAnimator != null)
            {
                childAnimator.ResetTrigger("Combo");
                childAnimator.ResetTrigger("NoCombo"); 

                if (isCombo)
                {
                    childAnimator.SetTrigger("Combo");
                }
                else
                {
                    childAnimator.SetTrigger("NoCombo");
                }
            }
        }
    }
}