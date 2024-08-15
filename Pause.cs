using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    public List<Button> pauseMenuButtons;
    public Animator pauseMenuAnimator;
    private int selectedButtonIndex = 0;
    private bool isPaused = false;

    private void Start()
    {
        pauseMenuButtons[0].onClick.AddListener(Resume);
        pauseMenuButtons[1].onClick.AddListener(Menu);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                PauseGame();
            }
        }

        if (isPaused)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // Deselect the currently selected button
                pauseMenuButtons[selectedButtonIndex].GetComponent<Animator>().SetBool("IsSelected", false);

                // Move to the previous button
                selectedButtonIndex--;
                if (selectedButtonIndex < 0)
                {
                    selectedButtonIndex = pauseMenuButtons.Count - 1;
                }
                SelectButton(selectedButtonIndex);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // Deselect the currently selected button
                pauseMenuButtons[selectedButtonIndex].GetComponent<Animator>().SetBool("IsSelected", false);

                // Move to the next button
                selectedButtonIndex = (selectedButtonIndex + 1) % pauseMenuButtons.Count;
                SelectButton(selectedButtonIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {

                Debug.Log("Return key pressed");
                pauseMenuButtons[selectedButtonIndex].onClick.Invoke();
            }
        }
    }

    void PauseGame()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        pauseMenuAnimator.Play("Pause");
        SelectButton(selectedButtonIndex);
    }

    void Resume()
    {
        StartCoroutine(ResumeAfterAnimation());
    }

    void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    IEnumerator ResumeAfterAnimation()
    {
        pauseMenuAnimator.Play("Unpause");
        yield return new WaitForSeconds(pauseMenuAnimator.GetCurrentAnimatorStateInfo(0).length);
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    void SelectButton(int index)
    {
        pauseMenuButtons[index].GetComponent<Animator>().SetBool("IsSelected", true);
    }
}