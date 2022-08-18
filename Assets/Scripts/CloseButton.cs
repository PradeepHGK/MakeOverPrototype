using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CloseButton : MonoBehaviour
{
    public GameObject CategoriesList;
    public GameObject AssetList;
    public GameObject Content;
    private Button backButton;

    private void Start()
    {
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(CloseAssetList);
    }

    public void OpenCategoriesList()
    {
        if (CategoriesList != null)
        {
            CategoriesList.SetActive(true);
        }
    }

    public void CloseAssetList()
    {
        if (AssetList != null)
        {
            AssetList.SetActive(false);

            for (int i = 0; i < Content.transform.childCount; i++)
            {
                Destroy(Content.transform.GetChild(i).gameObject, 1);
            }
    
            OpenCategoriesList();
        }
        backButton.gameObject.SetActive(false);
    }
}
