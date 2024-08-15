using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterCustomization : MonoBehaviour
{
    public GameObject[] headPrefabs;
    public GameObject[] facePrefabs;
    public GameObject[] accPrefabs;

    public Button headButton;
    public Button faceButton;
    public Button accButton;
    public Button startButton;

    public GameObject characterPreview;
    public Animator flash;

    private int currentHeadIndex = 0;
    private int currentFaceIndex = 0;
    private int currentAccIndex = 0;

    private GameObject currentCharacter;

    void Start()
    {
        headButton.onClick.AddListener(ApplyNextHead);
        faceButton.onClick.AddListener(ApplyNextFace);
        accButton.onClick.AddListener(ApplyNextAccessory);
        startButton.onClick.AddListener(ApplyCustomization);

        LoadCustomization();
        UpdateCharacterPreview();
    }

    void UpdateCharacterPreview()
    {
        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }
        currentCharacter = InstantiateCharacter();
    }

    GameObject InstantiateCharacter()
    {
        GameObject newCharacter = new GameObject("Character");

        GameObject newHead = Instantiate(headPrefabs[currentHeadIndex]);
        newHead.transform.parent = newCharacter.transform;

        GameObject newFace = Instantiate(facePrefabs[currentFaceIndex]);
        newFace.transform.parent = newCharacter.transform;

        GameObject newAcc = Instantiate(accPrefabs[currentAccIndex]);
        newAcc.transform.parent = newCharacter.transform;

        newCharacter.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        return newCharacter;
    }

    void ApplyNextHead()
    {
        currentHeadIndex = (currentHeadIndex + 1) % headPrefabs.Length;
        flash.SetTrigger("Flash");

        UpdateCharacterPreview();
    }

    void ApplyNextFace()
    {
        currentFaceIndex = (currentFaceIndex + 1) % facePrefabs.Length;
        flash.SetTrigger("Flash");

        UpdateCharacterPreview();
    }

    void ApplyNextAccessory()
    {
        currentAccIndex = (currentAccIndex + 1) % accPrefabs.Length;
        flash.SetTrigger("Flash");

        UpdateCharacterPreview();
    }

    public void ApplyCustomization()
    {
        CharacterCustomizationManager.Instance.SaveCustomizationData(currentHeadIndex, currentFaceIndex, currentAccIndex);
    }

    void LoadCustomization()
    {
        currentHeadIndex = CharacterCustomizationManager.Instance.currentHeadIndex;
        currentFaceIndex = CharacterCustomizationManager.Instance.currentFaceIndex;
        currentAccIndex = CharacterCustomizationManager.Instance.currentAccIndex;
    }
}