using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModelController : MonoBehaviour
{
    [SerializeField] List<Character> _charactersData;

    private void Start()
    {
        LoadCharacterData();
    }


    void LoadCharacterData()
    {
        foreach (var item in _charactersData)
        {
            Debug.Log(item.name);
            foreach (var data in item.modelProperties.categories)
            {
                Debug.Log(data.type);
            }
        }
    }
}
