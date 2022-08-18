using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class CharacterModelController : MonoBehaviour
{
    [Space(20)]

    [SerializeField] List<Character> _charactersData;
    [SerializeField] GameObject _listParent;
    [SerializeField] GameObject _characterBtnPrefab;

    [Space]
    [Header("Scroll List")]
    [SerializeField] GameObject _characterBtnList;
    [SerializeField] GameObject _categoriesBtnList;
    [SerializeField] GameObject _assetBtnList;

    [SerializeField] private Character _currentCharacterSelected;
    [SerializeField] private GameObject _currentCharacterModel;


    private void Start()
    {
        LoadCharacterData();
    }

    void LoadCharacterData()
    {
        foreach (var item in _charactersData)
        {

            Debug.Log(item.name);
            var characterBtn = Instantiate(_characterBtnPrefab, _listParent.transform).GetComponent<Button>();
            characterBtn.onClick.AddListener(() =>
            {
                _currentCharacterModel = Instantiate(item.GirlModel);

                _currentCharacterSelected = item;
                LoadCategoryData();
                _characterBtnList.SetActive(false);
            });
        }
    }


    void LoadCategoryData()
    {
        var typeList = Enum.GetValues(typeof(ModelPropertyType));
        foreach (var item in typeList)
        {
            _categoriesBtnList.SetActive(true);
            var catebtn = Instantiate(_characterBtnPrefab, _categoriesBtnList.transform.GetChild(0).transform).GetComponent<Button>();
            catebtn.gameObject.name = item.ToString();
            catebtn.onClick.AddListener(() =>
            {
                _categoriesBtnList.SetActive(false);
                LoadAssetData((ModelPropertyType)item);
            });
        }
    }

    void LoadAssetData(ModelPropertyType type)
    {
        _assetBtnList.SetActive(true);

        foreach (var item in _currentCharacterSelected.modelProperties.categories)
        {
            if (item.type == type)
            {
                var assetBtn = Instantiate(_characterBtnPrefab, _assetBtnList.transform.GetChild(0).transform).GetComponent<Button>();
                assetBtn.gameObject.name = item.modelProperties.modelName;
                assetBtn.onClick.AddListener(LoadAssetOnCharacter);
            }
        }
    }

    void LoadAssetOnCharacter()
    {

    }
}
