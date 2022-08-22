using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackButton : MonoBehaviour
{
    public GameObject characterList;
    public GameObject categoryList;
    public GameObject AssetList;
    public GameObject Content;
    private Button backButton;

    private void Start()
    {
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(CloseAssetList);
    }

    public void OpenCharacterList()
    {
        if (characterList != null)
        {
            characterList.SetActive(true);
            CloseAssetList();
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

            for (int i = 0; i < Content.transform.childCount; i++)
            {
                Destroy(Content.transform.GetChild(i).gameObject, 1);
            }

            OpenCharacterList();

        }
        
    }
    public void CloseAssetList()
    {
        if (AssetList != null)
        {
            AssetList.SetActive(false);

            for (int i = 0; i < Content.transform.childCount; i++)
            {
                Destroy(Content.transform.GetChild(i).gameObject, 2);
            }

            OpenCategoryList();
            
        }
        
    }
}