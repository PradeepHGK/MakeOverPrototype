using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CloseButton : MonoBehaviour
{
    public GameObject CategoriesList;
    public GameObject AssetList;
    public GameObject Content;
    public void OpenCategoriesList()
    {
        if (CategoriesList != null)
        {
            CategoriesList.SetActive(true);
        }
    }

    public void CloseAssetList()
    {
        if(AssetList != null)
        {
            AssetList.SetActive(false);

        for (var i = AssetList.transform.childCount - 1; i >= 0; i--)
         {
            Destroy(AssetList.transform.GetChild(i));
         }

        }
    }
}
