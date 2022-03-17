using UnityEngine;

namespace Managers
{
    internal class CameraControl : MonoBehaviour
    {
        public Transform cameraTransform;

        public float movementSpeed;
        public float movementTime;
        public float rotationAmount;
        public Vector3 zoomAmount;
        public float boarderThickness;
        public float minY;
        public float maxY;

        private Quaternion _newRotation;
        private Quaternion _originalRotation;
        private Vector3 _newPosition;
        private Vector3 _newCameraZoom;

        private void Start()
        {
            var t = transform;
            _newPosition = t.position;
            _originalRotation = t.rotation;
            _newRotation = _originalRotation;
            _newCameraZoom = cameraTransform.position;
        }

        private void Update()
        {
            if (GameMaster.GameIsOver)
            {
                enabled = false;
                return;
            }

            HandleMovement();

            transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * movementTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, _newRotation, Time.deltaTime * movementTime);
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, _newCameraZoom,
                                                            Time.deltaTime * movementTime);
        }

        private void LateUpdate()
        {
            HandleZoom();
        }

        private void HandleMovement()
        {
            if ((Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - boarderThickness) &&
                cameraTransform.position.z <= 40)
                _newPosition += Vector3.forward * movementSpeed;

            if ((Input.GetKey(KeyCode.A) || Input.mousePosition.x <= boarderThickness) && _newPosition.x >= -50)
                _newPosition += Vector3.left * movementSpeed;

            if ((Input.GetKey(KeyCode.S) || Input.mousePosition.y <= boarderThickness)
                        && cameraTransform.position.z >= -10)
                _newPosition += Vector3.back * movementSpeed;

            if ((Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - boarderThickness) &&
                _newPosition.x <= 50)
                _newPosition += Vector3.right * movementSpeed;

            if (Input.GetKey(KeyCode.Q))
                _newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);

            if (Input.GetKey(KeyCode.E))
                _newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);

            if (Input.GetKey(KeyCode.Space))
                _newRotation = _originalRotation;
        }

        private void HandleZoom()
        {
            if (Input.GetKey(KeyCode.R) && cameraTransform.position.y >= minY)
                _newCameraZoom += zoomAmount;

            if (Input.GetKey(KeyCode.F) && cameraTransform.position.y <= maxY)
                _newCameraZoom -= zoomAmount;
        }
    }
}