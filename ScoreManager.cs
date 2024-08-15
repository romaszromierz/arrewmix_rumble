using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public InputChecker inputChecker;
    public int playerScore = 0;
    public TextMeshProUGUI scoreText;
    private int previousHitCount = 0;
    private int previousTotalMissedCount = 0;
    public Animator scoreAnimator;

    void Update()
    {
        scoreText.text = "" + playerScore;

        if (inputChecker.hitCount > previousHitCount)
        {
            UpdatePlayerScore(true);
            previousHitCount = inputChecker.hitCount;
        }

        if (inputChecker.totalMissedCount > previousTotalMissedCount)
        {
            UpdatePlayerScore(false);
            previousTotalMissedCount = inputChecker.totalMissedCount;
        }
    }

    public void UpdatePlayerScore(bool hitArrow)
    {
        if (hitArrow)
        {
            if (inputChecker.scoreSlider.value >= inputChecker.scoreSlider.maxValue)
            {
                playerScore += 450;
                scoreAnimator.Play("POINTS");
            }
            else
            {
                playerScore += 150;
                scoreAnimator.Play("POINTS");
            }
        }
        else
        {
            playerScore -= 250;
            scoreAnimator.Play("POINTS");
        }
    }
}