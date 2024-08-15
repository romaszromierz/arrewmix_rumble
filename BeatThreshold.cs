using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatThreshold : MonoBehaviour
{
    public float[] beatThresholds;

    // Start is called before the first frame update
    void Start()
    {
        BeatDetector beatDetector = GetComponent<BeatDetector>();
        int selectedSongIndex = MenuController.currentSongIndex;

        if (beatThresholds.Length > selectedSongIndex)
        {
            beatDetector.beatThreshold = beatThresholds[selectedSongIndex];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
