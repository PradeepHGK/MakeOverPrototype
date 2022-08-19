using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{

    public float speed = 200f;
    private Touch touch;

    private Vector2 touchPosition;

    private Quaternion rotationY;






    private void OnMouseDrag()
    {

        float rotX = Input.GetAxis("Mouse X") * speed * Mathf.Deg2Rad;


        transform.Rotate(Vector3.up, rotX);


    }
    public void Update()
    {
        if (Input.touchCount > 0)

                {
                    touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Moved)

                    {
                        rotationY = Quaternion.Euler(0f, -touch.deltaPosition.x * speed, 0f);

                        transform.rotation = rotationY * transform.rotation;

                    }

                }

            }


        }



    

