using UnityEngine;
using System.Collections;

public class ArrowAnimatorController : MonoBehaviour
{
    public GameObject arrowClickHandlerObject;

    void Start()
    {
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return null;

        if (arrowClickHandlerObject != null)
        {
            ArrowClickHandler arrowClickHandler = arrowClickHandlerObject.GetComponent<ArrowClickHandler>();

            if (arrowClickHandler != null)
            {
                float timeDifference = arrowClickHandler.GetLastTimeDifference();

                SetTriggersOnChildren(transform, timeDifference);
            }
        }
    }

    void SetTriggersOnChildren(Transform parent, float timeDifference)
    {
        foreach (Transform child in parent)
        {
            Animator childAnimator = child.GetComponent<Animator>();
            if (childAnimator != null)
            {
                if (timeDifference >= 0 && timeDifference < 1.0f)
                {
                    childAnimator.SetTrigger("Hit");
                }
                else
                {
                    childAnimator.SetTrigger("Miss");
                }
            }
            SetTriggersOnChildren(child, timeDifference);
        }
    }
}
