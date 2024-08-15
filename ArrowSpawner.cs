using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ArrowSpawner : MonoBehaviour
{
    public List<GameObject> preComboArrowsUp; 
    public List<GameObject> comboArrowsUp; 
    public List<GameObject> preComboArrowsDown;
    public List<GameObject> comboArrowsDown; 
    public List<GameObject> preComboArrowsLeft;
    public List<GameObject> comboArrowsLeft;
    public List<GameObject> preComboArrowsRight;
    public List<GameObject> comboArrowsRight;

    public GameObject upEndPoint;
    public GameObject downEndPoint; 
    public GameObject leftEndPoint; 
    public GameObject rightEndPoint; 
    public GameObject upSpawnPoint;
    public GameObject downSpawnPoint;
    public GameObject leftSpawnPoint;
    public GameObject rightSpawnPoint;

    public string chartDirectoryPath;
    public string csvFilePath;
    public float arrowSpeed = 1.0f; 

    private float startTime;
    public bool comboState = false;
    public AudioSource audioSource;

    void Start()
    {

        string projectDirectory = Directory.GetParent(Application.dataPath).FullName;
        chartDirectoryPath = Path.Combine(projectDirectory, "Charts");
        string chartNameWithoutExtension = Path.GetFileNameWithoutExtension(MenuController.selectedChart);
        csvFilePath = Path.Combine(chartDirectoryPath, chartNameWithoutExtension + ".csv");


        startTime = Time.time;

        string[] lines = File.ReadAllLines(csvFilePath);
        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            GameObject startObject = null;
            GameObject endObject = null;
            float time = float.Parse(parts[1]) - 0.86f; 
            if (time < 0) time = 0;

            switch (parts[0])
            {
                case "Up":
                    startObject = upSpawnPoint;
                    endObject = upEndPoint;
                    StartCoroutine(SpawnArrow(time, startObject, endObject, "Up"));
                    break;
                case "Down":
                    startObject = downSpawnPoint;
                    endObject = downEndPoint;
                    StartCoroutine(SpawnArrow(time, startObject, endObject, "Down"));
                    break;
                case "Left":
                    startObject = leftSpawnPoint;
                    endObject = leftEndPoint;
                    StartCoroutine(SpawnArrow(time, startObject, endObject, "Left"));
                    break;
                case "Right":
                    startObject = rightSpawnPoint;
                    endObject = rightEndPoint;
                    StartCoroutine(SpawnArrow(time, startObject, endObject, "Right"));
                    break;
            }
        }
        string songFilePath = Path.Combine(chartDirectoryPath, MenuController.selectedSong + ".mp3");
    }

    IEnumerator SpawnArrow(float time, GameObject startObject, GameObject endObject, string direction)
    {
        float earliestSpawnTime = 1.0f;

        if (time < earliestSpawnTime)
        {
            yield break;
        }
        yield return new WaitForSeconds(time);

        GameObject arrowPrefab;
        List<GameObject> preComboArrows;
        List<GameObject> comboArrows;

        switch (direction)
        {
            case "Up":
                preComboArrows = preComboArrowsUp;
                comboArrows = comboArrowsUp;
                break;
            case "Down":
                preComboArrows = preComboArrowsDown;
                comboArrows = comboArrowsDown;
                break;
            case "Left":
                preComboArrows = preComboArrowsLeft;
                comboArrows = comboArrowsLeft;
                break;
            case "Right":
                preComboArrows = preComboArrowsRight;
                comboArrows = comboArrowsRight;
                break;
            default:
                yield break; 
        }

        if (comboState)
        {
            int randomIndex = Random.Range(0, comboArrows.Count);
            arrowPrefab = comboArrows[randomIndex];
        }
        else
        {
            int randomIndex = Random.Range(0, preComboArrows.Count);
            arrowPrefab = preComboArrows[randomIndex];
        }
        Vector3 spawnPosition = startObject.transform.position;
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
        StartCoroutine(MoveArrow(arrow, time, startObject, endObject));
    }

    IEnumerator MoveArrow(GameObject arrow, float time, GameObject startObject, GameObject endObject)
    {
        float elapsedTime = 0;
        time *= arrowSpeed; 

        while (elapsedTime < time)
        {
            if (arrow == null) yield break;
            float step = arrowSpeed * Time.deltaTime;
            float newY = Mathf.MoveTowards(arrow.transform.position.y, endObject.transform.position.y, step);
            arrow.transform.position = new Vector3(startObject.transform.position.x, newY, startObject.transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (arrow != null) 
        {
            arrow.transform.position = new Vector3(startObject.transform.position.x, endObject.transform.position.y, startObject.transform.position.z); 
            float timeSinceStart = Time.time - startTime; 
        }
    }
}
