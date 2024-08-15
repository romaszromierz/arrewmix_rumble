using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 

public class InputChecker : MonoBehaviour
{
    public int hitCount = 0;
    public int missedCount = 0;
    public int totalMissedCount = 0;

    public AudioClip hitSound; 
    public AudioClip missSound; 

    public GameObject scoreBar; 

    public AudioSource audioSource;
    public Slider scoreSlider; 

    public List<GameObject> currentArrows = new List<GameObject>();
    public List<KeyCode> currentKeys = new List<KeyCode>();
    public List<bool> hitRegistered = new List<bool>();
    public ArrowSpawner arrowSpawner;   


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        scoreSlider = scoreBar.GetComponent<Slider>(); 
    }

    void Update()
    {
        for (int i = 0; i < currentArrows.Count; i++)
        {
            if (Input.GetKeyDown(currentKeys[i]) && !hitRegistered[i])
            {
                hitRegistered[i] = true;
                hitCount++;
                audioSource.PlayOneShot(hitSound); 
                scoreSlider.value++;
                if (scoreSlider.value >= scoreSlider.maxValue)
                {
                    arrowSpawner.comboState = true;
                    missedCount = 0;
                }
                StartCoroutine(DestroyArrowAfterDelay(currentArrows[i], "UpArrHit")); 
            }
        }
    }

    IEnumerator DestroyArrowAfterDelay(GameObject arrow, string animationName)
    {
        try
        {
            Animator animator = arrow.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play(animationName);
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            }
        }
        finally
        {
            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Destroy(rb); 
            }
            Destroy(arrow);
        }
    }

    public void RegisterArrow(GameObject arrow, KeyCode key)
    {
        currentArrows.Add(arrow);
        hitRegistered.Add(false);
        currentKeys.Add(key);
        Debug.Log("Arrow collided with end point");
    }

    public void UnregisterArrow(GameObject arrow)
    {
        int index = currentArrows.IndexOf(arrow);
        if (index != -1 && !hitRegistered[index])
        {
            missedCount++;
            totalMissedCount++;
            if (missedCount >= 3) 
            {
                scoreSlider.value = 0; 
                arrowSpawner.comboState = false; 
                missedCount = 0; 
            }

            audioSource.PlayOneShot(missSound); 
            StartCoroutine(DestroyArrowAfterDelay(currentArrows[index], "UpArrMiss")); 
            Rigidbody rb = currentArrows[index].GetComponent<Rigidbody>();

            if (rb != null)
            {
                Destroy(rb); 
            }

            currentArrows.RemoveAt(index);
            hitRegistered.RemoveAt(index);
            currentKeys.RemoveAt(index);
        }
    }
}