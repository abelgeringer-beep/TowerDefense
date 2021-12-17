using UnityEngine;

namespace Managers
{
    internal class CameraControl : MonoBehaviour
    {
        public float panSpeed = 30f;
        public float panBoarderThickness = 10f;
        public float scrollSpeed = 5f;
        public float minY = 10f;
        public float maxY = 80f;
        
        private void Update()
        {
            if (GameMaster.GameIsOver)
            {
                this.enabled = false;
                return;
            }

            if ((Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBoarderThickness) && transform.position.z <= 75)
                transform.Translate(Vector3.forward * (panSpeed * Time.deltaTime), Space.World); // Ignore the rotation of camera

            if ((Input.GetKey("a") || Input.mousePosition.x <= panBoarderThickness) && transform.position.x >= -50)
                transform.Translate(Vector3.left * (panSpeed * Time.deltaTime), Space.World); 

            if ((Input.GetKey("s") || Input.mousePosition.y <= panBoarderThickness) && transform.position.z >= -20)
                transform.Translate(Vector3.back * (panSpeed * Time.deltaTime), Space.World); 

            if ((Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBoarderThickness) && transform.position.x <= 50)
                transform.Translate(Vector3.right * (panSpeed * Time.deltaTime), Space.World);

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            Vector3 pos = transform.position; // own position
            pos.y -= scroll * 10 * scrollSpeed; //scrollweel speed is very low
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            transform.position = pos;
        }
    }
}
