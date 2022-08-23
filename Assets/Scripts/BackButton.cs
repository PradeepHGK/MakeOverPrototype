using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackButton : MonoBehaviour
{
    public GameObject characterList;
    public GameObject categoryList;
    public GameObject AssetList;
    public GameObject AssetContent;
    public GameObject CategoryContent;
    private Button backButton;

    private void Start()
    {
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(CloseAssetList);
        backButton.onClick.AddListener(OpenCategoryList);
        backButton.onClick.AddListener(CloseCategoryList);

    }

    public void OpenCharacterList()
    {
        if (characterList != null)
        {
            characterList.SetActive(true);
            
        }
    }
    public void OpenCategoryList()
    {
        if (categoryList != null)
        {
            categoryList.SetActive(true);

           
        }
    }

    public void CloseCategoryList()
    {
        if (AssetList != null)
        {
            AssetList.SetActive(false);

            for (int i = 0; i < CategoryContent.transform.childCount; i++)
            {
                Destroy(CategoryContent.transform.GetChild(i).gameObject, 1);
            }

            OpenCharacterList();

        }
        
    }
    public void CloseAssetList()
    {
        if (AssetList != null)
        {
            AssetList.SetActive(false);

            for (int i = 0; i < AssetContent.transform.childCount; i++)
            {
                Destroy(AssetContent.transform.GetChild(i).gameObject, 1);
            }

            OpenCategoryList();
            
        }
        
    }
}