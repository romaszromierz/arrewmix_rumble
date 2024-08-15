using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrowdScript : MonoBehaviour
{
    public Slider slider;
    public AudioSource cheer;
    public AudioSource boo;

    private bool hasCheered = false;
    private bool hasBooed = false;

    private void Update()
    {
        if (Mathf.Approximately(slider.value, slider.maxValue))
        {
            if (!cheer.isPlaying && !hasCheered)
            {
                cheer.Play();
                hasCheered = true;
            }
        }
        else if (Mathf.Approximately(slider.value, 0f))
        {
            if (!boo.isPlaying && !hasBooed)
            {
                boo.Play();
                hasBooed = true;
            }
        }
        else
        {
            hasCheered = false;
            hasBooed = false;
        }
    }
}
