using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;
using MakeOver.Constant;

public class CharacterModelController_duplicate : MonoBehaviour
{
    public static CharacterModelController_duplicate Instance;

    [Space(20)]

    [SerializeField] List<Character> _charactersData;
    [SerializeField] GameObject _listParent;
    [SerializeField] GameObject _characterBtnPrefab;
    [SerializeField] GameObject _Successpopup;
    [SerializeField] GameObject _BGpanel;


    [Space]
    [Header("Scroll List")]
    [SerializeField] GameObject _characterBtnList;
    [SerializeField] GameObject _categoriesBtnList;
    [SerializeField] GameObject _assetBtnList;

    [SerializeField] private Character _currentCharacterSelected;
    [SerializeField] private GameObject _currentCharacterModel;

    [SerializeField] Button backButton;
    [SerializeField] Button DoneButton;

    [SerializeField] private Camera CharacterCamera;
    [SerializeField] private Stack<GameObject> _backStack = new Stack<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        LoadCharacterData();
        DoneButton.onClick.AddListener(OnClickDoneBtn);
        backButton.onClick.AddListener(OnClickBackBtn);

        _backStack.Push(_characterBtnList);
    }

    public void LoadCharacterData()
    {
        foreach (var item in _charactersData)
        {
            var characterBtn = Instantiate(_characterBtnPrefab, _listParent.transform).GetComponent<Button>();
            characterBtn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.ToString();



            characterBtn.onClick.AddListener(() =>
            {
                _currentCharacterSelected = item;
                _currentCharacterModel = Instantiate(item.GirlModel);
                _currentCharacterModel.transform.localEulerAngles = new Vector3(0, 180, 0);

                LoadCategoryData();
                _characterBtnList.SetActive(false);
                _backStack.Push(_categoriesBtnList);
                backButton.gameObject.SetActive(true);
            }
            );

            _characterBtnList.SetActive(true);
        }
    }

    void LoadCategoryData()
    {
        var typeList = Enum.GetValues(typeof(ModelPropertyType));

        foreach (ModelPropertyType item in typeList)
        {
            var canadd = _currentCharacterSelected.modelProperties.excludeTypes.Exists((t)=> {

                if(t.ToString() == item.ToString())
                {
                    return true;
                }

                return false;

            });

            if (!canadd)
            {
                _categoriesBtnList.SetActive(true);
                var catebtn = Instantiate(_characterBtnPrefab, _categoriesBtnList.transform.GetChild(0).transform).GetComponent<Button>();
                catebtn.gameObject.name = item.ToString();
                catebtn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.ToString();
                catebtn.onClick.AddListener(() =>
                {
                    _categoriesBtnList.SetActive(false);
                    LoadAssetData(item);
                    backButton.gameObject.SetActive(true);
                    _backStack.Push(_assetBtnList);
                });
            }
        }
    }


    //int i = 2;
    void LoadAssetData(ModelPropertyType modelPropertyType)
    {
       
        _assetBtnList.SetActive(true);

        foreach (var item in _currentCharacterSelected.modelProperties.categories)
        {
            if(item.type == modelPropertyType)
            {
                var assetBtn = Instantiate(_characterBtnPrefab, _assetBtnList.transform.GetChild(0).transform).GetComponent<Button>();
                assetBtn.gameObject.name = item.modelProperties.modelName;
                assetBtn.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.modelProperties.modelName;
                assetBtn.transform.GetChild(0).GetComponent<Image>().sprite = item.modelProperties.modelThumbnails;

                assetBtn.onClick.AddListener(() => LoadAssetOnCharacter(item));
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
                case ModelPropertyType.Body:
                    if (bodyParts.CompareTag("Shoes"))
                    {
                        bodyParts.GetComponent<SkinnedMeshRenderer>().material = category.modelProperties.ModelPartMaterial;
                    }
                    break;
                case ModelPropertyType.Shorts:
                    if (bodyParts.CompareTag("Shoes"))
                    {
                        bodyParts.GetComponent<SkinnedMeshRenderer>().material = category.modelProperties.ModelPartMaterial;
                    }
                    break;

                default:
                    break;
            }
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

    void OnClickBackBtn()
    {
        var popObject = _backStack.Pop();
        popObject.SetActive(false);

        var peekObject = _backStack.Peek();
        peekObject.SetActive(true);


        if (!popObject.name.Contains("Character"))
        {
            DestroyChildren(popObject.transform.GetChild(0));
        }

        if (peekObject.name.Contains("Character"))
        {
            Destroy(_currentCharacterModel.gameObject, 2);
        }
    }


    void DestroyChildren(Transform parent)
    {
        foreach (Transform item in parent)
        {
            Destroy(item.gameObject, 2);
        }
    }

    void OnClickDoneBtn()
    {
        //_BGpanel.SetActive(false);
        _Successpopup.SetActive(false);
        //CharacterCamera.gameObject.SetActive(false);
    }

    void ShowPopup()
    {
        _Successpopup.SetActive(true);
    }

    GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
}
