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
    private float MinimumZoom = 0f;
    [SerializeField]
    private float MaximumZoom = 0f;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPositionOffset = new Vector3();
        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y>Screen.height+ EdgeScrollingOffset)
        {
            newPositionOffset.y = CameraMovementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y < EdgeScrollingOffset)
        {
            newPositionOffset.y = -CameraMovementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x < EdgeScrollingOffset)
        {
            newPositionOffset.x = -CameraMovementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x > Screen.width + EdgeScrollingOffset)
        {
            newPositionOffset.x = CameraMovementSpeed * Time.deltaTime;
        }
        //Zoom in and out on mouse scroll
        if (Input.mouseScrollDelta.y != 0)
        {
            if ((transform.position.z <= MinimumZoom && Input.mouseScrollDelta.y>0)
                || (transform.position.z >= MaximumZoom && Input.mouseScrollDelta.y < 0))
            {
                newPositionOffset.z = Input.mouseScrollDelta.y * Time.deltaTime * ZoomSpeed;
            }
        }
        transform.position = transform.position + newPositionOffset;
    }
}
