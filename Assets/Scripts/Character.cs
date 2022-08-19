using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MakeOver_SO", menuName = "ScriptableObject/MakeOver/Character_SO")]
public class Character : ScriptableObject
{
    public GameObject GirlModel;
    //public List<MaterialsProperties> materialsProperties;
    //public List<TextureProperties> textureProperties;
    public CharacterSOData modelProperties;

}

[System.Serializable]
public class MaterialsProperties
{
    public Material ModelPartMaterial;
    public Texture2D Thumbnail;
    public ModelPropertyType type;
}

[System.Serializable]
public class TextureProperties
{
    public Texture2D Texture;
    public Texture2D Thumbnail;
    public ModelPropertyType type;
}

[System.Serializable]
public class ModelProperties
{
    public GameObject modelObject;
    public Texture2D ModelThumbnails;
    public Material ModelPartMaterial;
    public string modelName;
}

[System.Serializable]
public class Category
{
    public ModelPropertyType type;
    public ModelProperties modelProperties;
    public Texture2D ModelThumbnails;
}

[System.Serializable]
public class CharacterSOData
{
    public string CharacterName;
    public Transform modelPositionTransform;
    public Texture2D CharacterThumbnail;
    public List<Category> categories;
}


public enum ModelPropertyType
{
    Hair, Body, FaceStyle, Shoes, Jeans, OffShoulder, 
}
