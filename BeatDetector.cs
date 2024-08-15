using UnityEngine;
using System.Collections;

public class BeatDetector : MonoBehaviour
{
    public float beatThreshold;
    public BeatIndicator beatIndicator;

    public AudioSource audioSource;
    private float[] samples = new float[1024];


    void Update()
    {
      
        audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);

        if (samples[2] > beatThreshold)
        {
            Debug.Log("Beat detected!");
            // Trigger the beat indicator's HighlightBeat method
            beatIndicator.HighlightBeat();
        }
        
    }
}