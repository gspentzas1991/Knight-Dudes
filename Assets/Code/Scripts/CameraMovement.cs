using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float CameraMovementSpeed = 0f;
    [SerializeField]
    private float ZoomSpeed = 0f;
    [SerializeField]
    private float EdgeScrollingOffset = 0f;
    [SerializeField]
    private Vector3 MaximumPosition = new Vector3();
    [SerializeField]
    private Vector3 MinimumPosition = new Vector3();

    // Update is called once per frame
    void Update()
    {
        Vector3 newPositionOffset = new Vector3();
        if ((Input.GetKey(KeyCode.W) || Input.mousePosition.y>Screen.height+ EdgeScrollingOffset ) && transform.position.y<MaximumPosition.y)
        {
            newPositionOffset.y = CameraMovementSpeed * Time.deltaTime;
        }
        if ((Input.GetKey(KeyCode.S) || Input.mousePosition.y < EdgeScrollingOffset) && transform.position.y > MinimumPosition.y)
        {
            newPositionOffset.y = -CameraMovementSpeed * Time.deltaTime;
        }
        if ((Input.GetKey(KeyCode.D) || Input.mousePosition.x > Screen.width + EdgeScrollingOffset) && transform.position.x < MaximumPosition.x)
        {
            newPositionOffset.x = CameraMovementSpeed * Time.deltaTime;
        }
        if ((Input.GetKey(KeyCode.A) || Input.mousePosition.x < EdgeScrollingOffset) && transform.position.x > MinimumPosition.x)
        {
            newPositionOffset.x = -CameraMovementSpeed * Time.deltaTime;
        }
        //Zoom in and out on mouse scroll
        if ((Input.mouseScrollDelta.y >0 && transform.position.z < MaximumPosition.z) 
            || (Input.mouseScrollDelta.y < 0 && transform.position.z > MinimumPosition.z))
        {
            newPositionOffset.z = Input.mouseScrollDelta.y * Time.deltaTime * ZoomSpeed;
        }

        transform.position = transform.position + newPositionOffset;


    }
}
