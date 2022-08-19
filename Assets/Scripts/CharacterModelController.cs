using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;
using MakeOver.Constant;

public class CharacterModelController : MonoBehaviour
{
    [Space(20)]

    [SerializeField] List<Character> _charactersData;
    [SerializeField] GameObject _listParent;
    [SerializeField] GameObject _characterBtnPrefab;
    [SerializeField] GameObject _Successpopup;


    [Space]
    [Header("Scroll List")]
    [SerializeField] GameObject _characterBtnList;
    [SerializeField] GameObject _categoriesBtnList;
    [SerializeField] GameObject _assetBtnList;

    [SerializeField] private Character _currentCharacterSelected;
    [SerializeField] private GameObject _currentCharacterModel;

    [SerializeField] Button backButton;
    [SerializeField] Button DoneButton;



    private void Start()
    {
        LoadCharacterData();
        DoneButton.onClick.AddListener(LoadTaleScene);
    }

    void LoadCharacterData()
    {
        foreach (var item in _charactersData)
        {
            var characterBtn = Instantiate(_characterBtnPrefab, _listParent.transform).GetComponent<Button>();
            characterBtn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.ToString();

            _currentCharacterModel = Instantiate(item.GirlModel);
            _currentCharacterSelected = item;
            //characterBtn.onClick.AddListener(() =>
            //{

            //    //LoadCategoryData();
            //    _characterBtnList.SetActive(false);
            //});
        }
        LoadAssetData();
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
                catebtn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.ToString();
                catebtn.onClick.AddListener(() =>
                {
                    _categoriesBtnList.SetActive(false);
                    //LoadAssetData((ModelPropertyType)item);
                    backButton.gameObject.SetActive(true);
                });
            }
        }
    }

    void LoadAssetData()
    {
        _assetBtnList.SetActive(true);

        var randomAsset = new List<Category>();
        var rand = new System.Random();

        for (int i = 0; i < 3; i++)
        {
            var index = rand.Next(_currentCharacterSelected.modelProperties.categories.Count);
            randomAsset.Add(_currentCharacterSelected.modelProperties.categories.ElementAt(index));
        }

        foreach (var item in randomAsset)
        {
            {
                var assetBtn = Instantiate(_characterBtnPrefab, _assetBtnList.transform.GetChild(0).transform).GetComponent<Button>();
                assetBtn.gameObject.name = item.modelProperties.modelName;
                assetBtn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.modelProperties.modelName;
                assetBtn.transform.GetChild(0).GetComponent<Image>().sprite = item.modelProperties.modelThumbnails;
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

            Invoke("ShowPopup", 0.5f);
        }

        //Local Function
        void UpdateSharedMaterialArray(GameObject part, int index, Material materialToUpdate)
        {
            var rend = part.GetComponent<SkinnedMeshRenderer>();
            var matArray = rend.sharedMaterials;
            matArray[index] = materialToUpdate;
            part.GetComponent<SkinnedMeshRenderer>().sharedMaterials = matArray;
        }
    }

    void LoadTaleScene()
    {
        SceneManager.LoadScene(Constants.CandySceneName);
    }

    void ShowPopup()
    {
        _Successpopup.SetActive(true);
    }
}
