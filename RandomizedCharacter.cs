using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedCharacter : MonoBehaviour
{
    public GameObject randomCharacterTarget;
    public CharacterInstantiator characterInstantiator; 

    void Start()
    {
        characterInstantiator.InstantiateRandomCharacter(randomCharacterTarget);
    }
}
