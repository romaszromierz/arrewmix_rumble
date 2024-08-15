using UnityEngine;
using TMPro;

public class AIController : MonoBehaviour
{
    public TMP_Text scoreText;
    public int aiScore = 0;
    public Animator scoreAnimator;
    private int previousHitCount = 0;
    private int previousTotalMissedCount = 0;
    public InputChecker inputChecker;



    void Update()
    {
        scoreText.text = "" + aiScore;

        if (inputChecker.hitCount > previousHitCount)
        {
            float randomNumber = Random.Range(0f, 1f);
            if (randomNumber <= 0.8f)
            {
                UpdateAIScore(true);
            }
            else
            {
                UpdateAIScore(false);
            }
            previousHitCount = inputChecker.hitCount;
        }

        if (inputChecker.totalMissedCount > previousTotalMissedCount)
        {
            float randomNumber = Random.Range(0f, 1f);
            if (randomNumber <= 0.2f)
            {
                UpdateAIScore(false);
            }
            else
            {
                UpdateAIScore(true);
            }
            previousTotalMissedCount = inputChecker.totalMissedCount;
        }
    }

    public void UpdateAIScore(bool addPoints)
    {
        if (addPoints)
        {
            if (inputChecker.scoreSlider.value >= inputChecker.scoreSlider.maxValue)
            {
                aiScore += 450;
                scoreAnimator.Play("POINTS");
            }
            else
            {
                aiScore += 150;
                scoreAnimator.Play("POINTS");
            }
        }
        else
        {
            aiScore -= 250;
            scoreAnimator.Play("POINTS");
        }
    }
}