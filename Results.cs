using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Results : MonoBehaviour
{
    public Animator arrowKo; 
    public GameObject ui;
    public TMP_Text aiScoreText;
    public TMP_Text playerScoreText;
    public TMP_Text resultText;
    public Button mainMenuButton;

    public AudioSource audioSource; 
    public ScoreManager scoreManager; 
    public AIController aiController; 

    private int aiScore;
    private int playerScore;

    private void Start()
    {
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(mainMenuButton.gameObject);
        ui.SetActive(false);
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            arrowKo.SetTrigger("Result");
            ui.SetActive(true);

            aiScoreText.text = "" + aiController.aiScore.ToString();
            playerScoreText.text = "" + scoreManager.playerScore.ToString();

            if (scoreManager.playerScore > aiController.aiScore)
            {
                resultText.text = "You Won!";
            }
            else
            {
                resultText.text = "You Lost!";
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                GoToMainMenu();
            }
        }
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}