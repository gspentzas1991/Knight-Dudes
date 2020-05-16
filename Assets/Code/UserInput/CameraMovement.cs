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
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform gameCursorTransform;
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
            var cursorPosition = mainCamera.WorldToViewportPoint(gameCursorTransform.position);
            var newPositionOffset = new Vector3();
            if (Input.GetKey(KeyCode.W) || cursorPosition.y>1-edgeScrollingOffset )
            {
                newPositionOffset.y = cameraMovementSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.S) || cursorPosition.y < edgeScrollingOffset )
            {
                newPositionOffset.y = -cameraMovementSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D) || cursorPosition.x > 1 - edgeScrollingOffset)
            {
                newPositionOffset.x = cameraMovementSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.A) || cursorPosition.x < edgeScrollingOffset)
            {
                newPositionOffset.x = -cameraMovementSpeed * Time.deltaTime;
            }
            if (Input.mouseScrollDelta.y >0  
                || Input.mouseScrollDelta.y < 0)
            {
                newPositionOffset.z = Input.mouseScrollDelta.y * Time.deltaTime * zoomSpeed;
            }

            transform.position = ClampPosition(transform.position + newPositionOffset);
        }

        /// <summary>
        /// Keeps the vector3 position between the minimum and maximum values
        /// </summary>
        private Vector3 ClampPosition(Vector3 position)
        {
            position.x = Mathf.Clamp(position.x, minimumPosition.x, maximumPosition.x);
            position.y = Mathf.Clamp(position.y, minimumPosition.y, maximumPosition.y);
            position.z = Mathf.Clamp(position.z, minimumPosition.z, maximumPosition.z);
            return position;
        }
    }
}
