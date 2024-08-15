using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarAnimationPlayScript : MonoBehaviour
{
    public Slider slider;

    private float previousSliderValue;
    private void Start()
    {
        previousSliderValue = slider.value;
    }
    void Update()
    {
        if (!Mathf.Approximately(slider.value, previousSliderValue))
        {
            if (slider.value > previousSliderValue)
            {
                SetTriggersForChildren("Happy");
                Debug.Log("Happy");
            }
            else
            {
                SetTriggersForChildren("Hurt");
                Debug.Log("Hurt");
            }
            previousSliderValue = slider.value;
        }
    }

    private void SetTriggersForChildren(string triggerName)
    {
        foreach (Transform child in transform)
        {
            Animator childAnimator = child.GetComponent<Animator>();

            if (childAnimator != null)
            {
                childAnimator.ResetTrigger("Hurt");
                childAnimator.ResetTrigger("Happy");
                childAnimator.SetTrigger(triggerName);
            }
        }
    }
}
