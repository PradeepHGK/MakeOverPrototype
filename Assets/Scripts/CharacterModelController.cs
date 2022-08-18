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
        foreach (ModelPropertyType item in typeList)
        {
            if (item != ModelPropertyType.Body)
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
                assetBtn.onClick.AddListener(()=> LoadAssetOnCharacter(item));
            }
        }
    }

    void LoadAssetOnCharacter(Category category)
    {
        foreach (Transform bodyParts in _currentCharacterModel.transform)
        {
            switch (category.type)
            {
                case ModelPropertyType.Jeans:
                    if (bodyParts.CompareTag("Jeans"))
                    {
                        UpdateSharedMaterialArray(bodyParts.gameObject, 0, category.modelProperties.ModelPartMaterial);
                    }
                    break;
                case ModelPropertyType.FaceStyle:
                    if (bodyParts.CompareTag("Jeans"))
                    {
                        UpdateSharedMaterialArray(bodyParts.gameObject, 3, category.modelProperties.ModelPartMaterial);
                    }
                    break;
                case ModelPropertyType.Hair:
                    if (bodyParts.CompareTag("Hair"))
                    {
                        bodyParts.GetComponent<SkinnedMeshRenderer>().material = category.modelProperties.ModelPartMaterial;
                    }
                    break;
                case ModelPropertyType.Shoes:
                    if (bodyParts.CompareTag("Shoes"))
                    {
                        bodyParts.GetComponent<SkinnedMeshRenderer>().material = category.modelProperties.ModelPartMaterial;
                    }
                    break;
                case ModelPropertyType.OffShoulder:
                    if (bodyParts.CompareTag("Jeans"))
                    {
                        UpdateSharedMaterialArray(bodyParts.gameObject, 1, category.modelProperties.ModelPartMaterial);
                    }
                    break;
                default:
                    break;
            }
        }

        void UpdateSharedMaterialArray(GameObject part, int index, Material materialToUpdate)
        {
            var rend = part.GetComponent<SkinnedMeshRenderer>();
            var matArray = rend.sharedMaterials;
            matArray[index] = materialToUpdate;
            part.GetComponent<SkinnedMeshRenderer>().sharedMaterials = matArray;
        }
    }
}
