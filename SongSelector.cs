using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class SongSelector : MonoBehaviour
{
    public AudioClip[] songs;
    public GameObject chartCreatorPrefab;
    public AudioSource previewAudioSource;
    public Button nextButton;
    public Button confirmButton;
    public Button backButton;
    public Button previousButton;
    public Button menuButton;
    public Button checkButton;
    public Button continueButton;
    public GameObject songSelectionUI; 
    public GameObject chartCreationUI; 
    public GameObject recordUI;
    public GameObject fileSavedUI;
    public TMP_InputField chartNameInputField;
    public TextMeshProUGUI songTitleText; 
    public TMP_Text errorMessageText;
    private int currentSongIndex = 0;
    public GameObject leftArrow;
    public GameObject upArrow;
    public GameObject downArrow;
    public GameObject rightArrow;

    public TextMeshProUGUI leftArrowText;
    public TextMeshProUGUI upArrowText;
    public TextMeshProUGUI downArrowText;
    public TextMeshProUGUI rightArrowText;

    void Start()
    {
        chartCreationUI.SetActive(false);
        recordUI.SetActive(false);
        fileSavedUI.SetActive(false);

        menuButton.onClick.AddListener(Menu);
        nextButton.onClick.AddListener(NextSong);
        previousButton.onClick.AddListener(PreviousSong);
        confirmButton.onClick.AddListener(SelectCurrentSong);
        backButton.onClick.AddListener(Back);
        checkButton.onClick.AddListener(StartRecording);
        continueButton.onClick.AddListener(Continue);

        UpdateSongPreview();
    }

    void Update()
    {
        if (recordUI.activeInHierarchy)
        {
            float songTime = previewAudioSource.time;

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                leftArrow.GetComponent<Animator>().Play("BluePr");
                leftArrowText.text = $"{songTime}";
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                upArrow.GetComponent<Animator>().Play("BluePr");
                upArrowText.text = $"{songTime}";
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                downArrow.GetComponent<Animator>().Play("BluePr");
                downArrowText.text = $"{songTime}";
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                rightArrow.GetComponent<Animator>().Play("BluePr");
                rightArrowText.text = $"{songTime}";
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (previewAudioSource.isPlaying)
                {
                    previewAudioSource.Pause();
                }
                else
                {
                    previewAudioSource.Play();
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                string chartName = chartNameInputField.text;
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Charts");
                string filePath = Path.Combine(folderPath, chartName + ".csv");

                if (File.Exists(filePath))
                {
                    File.WriteAllText(filePath, string.Empty);
                }

                previewAudioSource.time = 0;
                previewAudioSource.Play();

                leftArrowText.text = "0";
                upArrowText.text = "0";
                downArrowText.text = "0";
                rightArrowText.text = "0";
            }
            if (!previewAudioSource.isPlaying && previewAudioSource.time >= previewAudioSource.clip.length)
            {
                recordUI.SetActive(false);
                fileSavedUI.SetActive(true);
            }
        }
        if (songSelectionUI.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PreviousSong();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                NextSong();
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                SelectCurrentSong();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(sceneName: "MainMenu");
            }
        }
        else if (chartCreationUI.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartRecording();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Back();
            }
        }
        else if (fileSavedUI.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnContinueButtonClicked();
            }
        }
    }
    public void NextSong()
    {
        currentSongIndex = (currentSongIndex + 1) % songs.Length;
        UpdateSongPreview();
    }

    public void PreviousSong()
    {
        currentSongIndex--;
        if (currentSongIndex < 0)
        {
            currentSongIndex = songs.Length - 1;
        }
        UpdateSongPreview();
    }
    private void UpdateSongPreview()
    {
        previewAudioSource.clip = songs[currentSongIndex];
        previewAudioSource.Play();

        songTitleText.text = songs[currentSongIndex].name;
    }

    public void SelectCurrentSong()
    {
        songSelectionUI.SetActive(false);
        chartCreationUI.SetActive(true);
    }

    public void Continue()
    {
        fileSavedUI.SetActive(false);
        songSelectionUI.SetActive(true);
    }

    public void Menu()
    {
        SceneManager.LoadScene(sceneName: "MainMenu");
    }
    private void CreateChartCreator(AudioClip song)
    {
        string chartName = chartNameInputField.text;
        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Charts");
        string filePath = Path.Combine(folderPath, chartName + ".csv");

        if (File.Exists(filePath))
        {
            errorMessageText.text = "A file with this name already exists.";
            return;
        }

        GameObject chartCreator = Instantiate(chartCreatorPrefab);
        ChartCreator chartCreatorScript = chartCreator.GetComponent<ChartCreator>();
        chartCreatorScript.audioSource.clip = song;
        chartCreatorScript.chartName = chartName;
    }

    public void StartRecording()
    {
        string chartName = chartNameInputField.text;
        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Charts");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string filePath = Path.Combine(folderPath, chartName + ".csv");
        if (File.Exists(filePath))
        {
            errorMessageText.text = "A chart with this name already exists.";
        }
        else if (string.IsNullOrWhiteSpace(chartName))
        {
            errorMessageText.text = "Please name your chart";
        }
        else
        {
            errorMessageText.text = "Chart name is available.";

            chartCreationUI.SetActive(false);
            recordUI.SetActive(true);

            GameObject chartCreator = Instantiate(chartCreatorPrefab);
            ChartCreator chartCreatorScript = chartCreator.GetComponent<ChartCreator>();
            chartCreatorScript.InitializeChart(chartName, previewAudioSource);
        }
    }

    public void Back()
    {
        songSelectionUI.SetActive(true);
        chartCreationUI.SetActive(false);
    }

    public void OnContinueButtonClicked()
    {
        fileSavedUI.SetActive(false);
        songSelectionUI.SetActive(true);
    }
}