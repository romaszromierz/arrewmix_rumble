using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterCustomizationManager : MonoBehaviour
{
    public static CharacterCustomizationManager Instance;

    public int currentHeadIndex = 0;
    public int currentFaceIndex = 0;
    public int currentAccIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveCustomizationData(int headIndex, int faceIndex, int accIndex)
    {
        currentHeadIndex = headIndex;
        currentFaceIndex = faceIndex;
        currentAccIndex = accIndex;
    }
}