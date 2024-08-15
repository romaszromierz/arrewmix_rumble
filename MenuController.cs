using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuController : MonoBehaviour
{

    public GameObject MainMenuUI;
    public GameObject SongSelectUI;
    public GameObject PlayUI;
    public GameObject CharacterCustomizeUI;
    public GameObject SettingsUI;
    public GameObject ChartSelectUI;

    public Animator myAnimator;
    public Animator flash;
    public Animator songChangeAnimator;

    public List<Button> buttons;
    public List<Button> customizeButtons;
    public List<Color> colors;
    public List<Sprite> arts;
    public Image menuArt;
    public Image background;
    private int selectedIndex;
    public float[] previewStartTimes;
    public float[] previewDurations;
    public GameObject chartButtonPrefab;
    public Transform chartListContent;

    public string[] songTitles;
    public Sprite[] albumCovers;
    public AudioClip[] previewClips;
    public Image albumCoverImage;
    public TextMeshProUGUI songTitleText;
    public AudioSource songPreviewPlayer;
    public List<Button> chartButtons;


    public AudioSource menuTheme;
    public static int currentSongIndex;
    private int selectedCustomizeIndex;

    public static string selectedSong;
    public static string selectedChart;

    public List<float> beatThresholds;
    public BeatDetector beatDetector;
    void Start()
    {
        PlayUI.SetActive(false);
        CharacterCustomizeUI.SetActive(false);
        SongSelectUI.SetActive(false);
        SettingsUI.SetActive(false);
        ChartSelectUI.SetActive(false);

        selectedIndex = 0;
        SelectButton(selectedIndex);

        currentSongIndex = 0;
        UpdateSong();
        buttons[0].onClick.AddListener(Songs);
        buttons[1].onClick.AddListener(Customize);
        buttons[2].onClick.AddListener(Chart);
        buttons[3].onClick.AddListener(Exit);
        customizeButtons[0].onClick.AddListener(GoBack);
        selectedCustomizeIndex = 0;
        SelectCustomizeButton(selectedCustomizeIndex);
        songPreviewPlayer.volume = 0;
    }

    void Update()
    {

        if (CharacterCustomizeUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                customizeButtons[selectedCustomizeIndex].GetComponent<Animator>().SetBool("IsSelected", false);

                selectedCustomizeIndex--;
                if (selectedCustomizeIndex < 0)
                {
                    selectedCustomizeIndex = customizeButtons.Count - 1;
                }
                SelectCustomizeButton(selectedCustomizeIndex);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                customizeButtons[selectedCustomizeIndex].GetComponent<Animator>().SetBool("IsSelected", false);

                selectedCustomizeIndex = (selectedCustomizeIndex + 1) % customizeButtons.Count;
                SelectCustomizeButton(selectedCustomizeIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                customizeButtons[selectedCustomizeIndex].onClick.Invoke();
            }
        }
        else if (MainMenuUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                buttons[selectedIndex].GetComponent<Animator>().SetBool("IsSelected", false);

                selectedIndex--;
                if (selectedIndex < 0)
                {
                    selectedIndex = buttons.Count - 1;
                }
                SelectButton(selectedIndex);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                buttons[selectedIndex].GetComponent<Animator>().SetBool("IsSelected", false);

                selectedIndex = (selectedIndex + 1) % buttons.Count;
                SelectButton(selectedIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                buttons[selectedIndex].onClick.Invoke();
            }
        }
        else if(SongSelectUI.activeSelf)
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
                ChartSelect();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                GoBack2();
            }
        }
        else if (ChartSelectUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                chartButtons[selectedIndex].GetComponent<Animator>().SetBool("IsSelected", false);

                selectedIndex--;
                if (selectedIndex < 0)
                {
                    selectedIndex = chartButtons.Count - 1;
                }
                SelectChartButton(selectedIndex);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                chartButtons[selectedIndex].GetComponent<Animator>().SetBool("IsSelected", false);
                selectedIndex = (selectedIndex + 1) % chartButtons.Count;
                SelectChartButton(selectedIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                chartButtons[selectedIndex].onClick.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                GoBack3();
            }
        }
    }

    void SelectButton(int index)
    {
        buttons[selectedIndex].GetComponent<Animator>().SetBool("IsSelected", false);
        selectedIndex = index;
        buttons[selectedIndex].GetComponent<Animator>().SetBool("IsSelected", true);
        menuArt.sprite = arts[selectedIndex];
        StartCoroutine(FadeToColor(colors[selectedIndex]));
    }

    IEnumerator FadeToColor(Color targetColor)
    {
        float duration = 0.1f;
        float timer = 0f;
        Color initialColor = background.color;

        while (timer < duration)
        {
            background.color = Color.Lerp(initialColor, targetColor, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        background.color = targetColor;
    }

    void UpdateSong()
    {
        albumCoverImage.sprite = albumCovers[currentSongIndex];
        songTitleText.text = songTitles[currentSongIndex];
        songPreviewPlayer.clip = previewClips[currentSongIndex];
        songPreviewPlayer.time = previewStartTimes[currentSongIndex];
        songPreviewPlayer.Play();
        StopAllCoroutines();  //zapobiega tym dziwnym loopom
        StartCoroutine(StopPreviewAfterDuration(previewDurations[currentSongIndex]));

        selectedSong = songTitles[currentSongIndex];

        if (beatDetector != null && beatThresholds.Count > currentSongIndex)
        {
            beatDetector.beatThreshold = beatThresholds[currentSongIndex];
        }
    }

    IEnumerator StopPreviewAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        songPreviewPlayer.time = previewStartTimes[currentSongIndex];
        UpdateSong();
    }

    public void NextSong()
    {
        currentSongIndex = (currentSongIndex + 1) % songTitles.Length;
        UpdateSong();
        songChangeAnimator.Play("Select");
    }

    public void PreviousSong()
    {
        currentSongIndex--;
        if (currentSongIndex < 0)
        {
            currentSongIndex = songTitles.Length - 1;
        }
        UpdateSong();
        songChangeAnimator.Play("Select");
    }

    void Songs()
    {
        MainMenuUI.SetActive(false);
        SongSelectUI.SetActive(true);
        menuTheme.volume = 0;
        songPreviewPlayer.volume = 1;
    }

    void Customize()
    {
        myAnimator.SetTrigger("PlayAnimation");
        CharacterCustomizeUI.SetActive(true);
        MainMenuUI.SetActive(false);
    }

    void SelectCustomizeButton(int index)
    {
        customizeButtons[selectedCustomizeIndex].GetComponent<Animator>().SetBool("IsSelected", false);

        selectedCustomizeIndex = index;
        customizeButtons[selectedCustomizeIndex].GetComponent<Animator>().SetBool("IsSelected", true);
    }

    void Chart()
    {
        SceneManager.LoadScene(sceneName: "Creator");
    }

    public void ChartSelect()
    {
        SongSelectUI.SetActive(false);
        ChartSelectUI.SetActive(true);
        PopulateChartList();
        SelectChartButton(0);
    }

    void PopulateChartList()
    {
        string[] chartFiles = GetChartFiles();

        foreach (Transform child in chartListContent)
        {
            Destroy(child.gameObject);
        }

        chartButtons.Clear();

        foreach (string chartFile in chartFiles)
        {
            GameObject button = Instantiate(chartButtonPrefab, chartListContent);
            button.GetComponentInChildren<TextMeshProUGUI>().text = Path.GetFileNameWithoutExtension(chartFile);
            button.GetComponent<Button>().onClick.AddListener(() => LoadChart(chartFile));

            chartButtons.Add(button.GetComponent<Button>());
        }
    }

    public string[] GetChartFiles()
    {
        return Directory.GetFiles(ChartDirectory.Path, "*.csv");
    }

    public static class ChartDirectory
    {
        public static string Path { get; set; } = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Charts");
    }
    void LoadChart(string chartFile)
    {
        selectedChart = chartFile;

        SceneManager.LoadScene("SampleScene"); 
    }

    void SelectChartButton(int index)
    {
        chartButtons[selectedIndex].GetComponent<Animator>().SetBool("IsSelected", false);

        selectedIndex = index;
        chartButtons[selectedIndex].GetComponent<Animator>().SetBool("IsSelected", true);
    }

    void Exit()
    {
        Debug.Log("Exit game");
        Application.Quit();
        
    }


    void GoBack()
    {
        myAnimator.SetTrigger("RePlayAnimation");
        CharacterCustomizeUI.SetActive(false);
        MainMenuUI.SetActive(true);


    }


    void Play()
    {
        PlayUI.SetActive(true);
        SongSelectUI.SetActive(false);

    }

    void GoBack2()
    {
        MainMenuUI.SetActive(true);
        SongSelectUI.SetActive(false);
        menuTheme.volume = 1;
        songPreviewPlayer.volume = 0;

    }
    void GoBack3()
    {
        SongSelectUI.SetActive(true);
        ChartSelectUI.SetActive(false);
        selectedIndex = 0; 
    }
}