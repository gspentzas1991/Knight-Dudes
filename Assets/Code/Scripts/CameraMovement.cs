using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Scripts
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField]
        private float cameraMovementSpeed = 0f;
        [SerializeField]
        private float zoomSpeed = 0f;
        [SerializeField]
        private float edgeScrollingOffset = 0f;
        [SerializeField]
        private Vector3 maximumPosition = new Vector3();
        [SerializeField]
        private Vector3 minimumPosition = new Vector3();

        // Update is called once per frame
        private void LateUpdate()
        {
            MovementDetection();
        }

        /// <summary>
        /// Moves the transform of the camera according to user input
        /// </summary>
        private void MovementDetection()
        {
            var newPositionOffset = new Vector3();
            if ((Input.GetKey(KeyCode.W) || Input.mousePosition.y>Screen.height+ edgeScrollingOffset ) && transform.position.y<maximumPosition.y)
            {
                newPositionOffset.y = cameraMovementSpeed * Time.deltaTime;
            }
            if ((Input.GetKey(KeyCode.S) || Input.mousePosition.y < edgeScrollingOffset) && transform.position.y > minimumPosition.y)
            {
                newPositionOffset.y = -cameraMovementSpeed * Time.deltaTime;
            }
            if ((Input.GetKey(KeyCode.D) || Input.mousePosition.x > Screen.width + edgeScrollingOffset) && transform.position.x < maximumPosition.x)
            {
                newPositionOffset.x = cameraMovementSpeed * Time.deltaTime;
            }
            if ((Input.GetKey(KeyCode.A) || Input.mousePosition.x < edgeScrollingOffset) && transform.position.x > minimumPosition.x)
            {
                newPositionOffset.x = -cameraMovementSpeed * Time.deltaTime;
            }
            if ((Input.mouseScrollDelta.y >0 && transform.position.z < maximumPosition.z) 
                || (Input.mouseScrollDelta.y < 0 && transform.position.z > minimumPosition.z))
            {
                newPositionOffset.z = Input.mouseScrollDelta.y * Time.deltaTime * zoomSpeed;
            }
            transform.position += newPositionOffset;
        }
    }
}
