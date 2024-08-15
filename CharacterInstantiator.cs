using UnityEngine;

public class CharacterInstantiator : MonoBehaviour
{
    public GameObject[] headPrefabs;
    public GameObject[] facePrefabs;
    public GameObject[] accPrefabs;

    public GameObject instantiationTarget; 
    public Animator commonAnimator; 

    private void Start()
    {
        int currentHeadIndex = CharacterCustomizationManager.Instance.currentHeadIndex;
        int currentFaceIndex = CharacterCustomizationManager.Instance.currentFaceIndex;
        int currentAccIndex = CharacterCustomizationManager.Instance.currentAccIndex;

        GameObject newCharacter = InstantiateCharacter(currentHeadIndex, currentFaceIndex, currentAccIndex, instantiationTarget);
        newCharacter.transform.parent = instantiationTarget.transform;

        SetAnimatorParameter(newCharacter, "VerticalOffset", 0.5f);
    }

    GameObject InstantiateCharacter(int headIndex, int faceIndex, int accIndex, GameObject target)
    {
        GameObject newCharacter = new GameObject("Character");

        GameObject newHead = Instantiate(headPrefabs[headIndex]);
        newHead.transform.parent = newCharacter.transform;

        GameObject newFace = Instantiate(facePrefabs[faceIndex]);
        newFace.transform.parent = newCharacter.transform;

        GameObject newAcc = Instantiate(accPrefabs[accIndex]);
        newAcc.transform.parent = newCharacter.transform;

        newCharacter.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        newCharacter.transform.position = target.transform.position;
        newCharacter.transform.localScale = target.transform.localScale;

        Animator characterAnimator = newCharacter.AddComponent<Animator>();
        characterAnimator.runtimeAnimatorController = commonAnimator.runtimeAnimatorController;

        return newCharacter;
    }

    public void InstantiateRandomCharacter(GameObject target)
    {
        int randomHeadIndex = Random.Range(0, headPrefabs.Length);
        int randomFaceIndex = Random.Range(0, facePrefabs.Length);
        int randomAccIndex = Random.Range(0, accPrefabs.Length);

        GameObject newCharacter = InstantiateCharacter(randomHeadIndex, randomFaceIndex, randomAccIndex, target);

        SetAnimatorParameter(newCharacter, "VerticalOffset", 0.5f);
    }

    void SetAnimatorParameter(GameObject character, string parameterName, float value)
    {
        Animator animator = character.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetFloat(parameterName, value);
        }
    }
}