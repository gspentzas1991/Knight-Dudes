using UnityEngine;

namespace Code.UserInput
{
    public class CameraMovement : MonoBehaviour
    {
        #pragma warning disable 0649
        [SerializeField] private float cameraMovementSpeed;
        [SerializeField] private float zoomSpeed;
        [SerializeField] private float edgeScrollingOffset;
        [SerializeField] private Vector3 maximumPosition;
        [SerializeField] private Vector3 minimumPosition;
        #pragma warning restore 0649

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
            if ((UnityEngine.Input.GetKey(KeyCode.W) || UnityEngine.Input.mousePosition.y>Screen.height+ edgeScrollingOffset ) && transform.position.y<maximumPosition.y)
            {
                newPositionOffset.y = cameraMovementSpeed * Time.deltaTime;
            }
            if ((UnityEngine.Input.GetKey(KeyCode.S) || UnityEngine.Input.mousePosition.y < edgeScrollingOffset) && transform.position.y > minimumPosition.y)
            {
                newPositionOffset.y = -cameraMovementSpeed * Time.deltaTime;
            }
            if ((UnityEngine.Input.GetKey(KeyCode.D) || UnityEngine.Input.mousePosition.x > Screen.width + edgeScrollingOffset) && transform.position.x < maximumPosition.x)
            {
                newPositionOffset.x = cameraMovementSpeed * Time.deltaTime;
            }
            if ((UnityEngine.Input.GetKey(KeyCode.A) || UnityEngine.Input.mousePosition.x < edgeScrollingOffset) && transform.position.x > minimumPosition.x)
            {
                newPositionOffset.x = -cameraMovementSpeed * Time.deltaTime;
            }
            if ((UnityEngine.Input.mouseScrollDelta.y >0 && transform.position.z < maximumPosition.z) 
                || (UnityEngine.Input.mouseScrollDelta.y < 0 && transform.position.z > minimumPosition.z))
            {
                newPositionOffset.z = UnityEngine.Input.mouseScrollDelta.y * Time.deltaTime * zoomSpeed;
            }
            transform.position += newPositionOffset;
        }
    }
}
