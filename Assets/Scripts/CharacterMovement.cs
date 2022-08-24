using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{
    public GameObject model;

    public void RotateRight()
    {
        model.transform.Rotate(0f, -25f, 0f);
    }


    public void RotateLeft()
    {
        model.transform.Rotate(0f, 25f, 0f);
    }
}
