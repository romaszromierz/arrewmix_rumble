using System;
using System.IO;
using UnityEngine;

public class ChartCreator : MonoBehaviour
{
    public AudioSource audioSource;
    private float songTime;
    public string chartName;

    void Update()
    {
        songTime = audioSource.time;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RecordArrowPress("Up", songTime);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            RecordArrowPress("Down", songTime);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RecordArrowPress("Left", songTime);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RecordArrowPress("Right", songTime);
        }
    }

    void RecordArrowPress(string arrowType, float timing)
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Charts", chartName + ".csv");

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{arrowType},{timing}");
            }
    }

    public void InitializeChart(string chartName, AudioSource audioSource)
    {
        this.chartName = chartName;
        this.audioSource = audioSource;
        audioSource.Play();
    }
}