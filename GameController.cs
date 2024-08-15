using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public List<AudioClip> audioClips;
    public AudioSource audioSource;

    void Start()
    {
        int activeHeadIndex = CharacterCustomizationManager.Instance.currentHeadIndex;
        int activeFaceIndex = CharacterCustomizationManager.Instance.currentFaceIndex;
        int activeAccIndex = CharacterCustomizationManager.Instance.currentAccIndex;

        string selectedSong = MenuController.selectedSong;
        int selectedIndex = audioClips.FindIndex(clip => clip.name == selectedSong);
        if (selectedIndex >= 0 && selectedIndex < audioClips.Count)
        {
            AudioClip selectedClip = audioClips[selectedIndex];
            audioSource.clip = selectedClip;
            audioSource.Play();
        }

        string selectedChart = MenuController.selectedChart;
    }

}