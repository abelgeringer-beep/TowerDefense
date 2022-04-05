using UnityEngine;

namespace Managers
{
    internal class CameraControl : MonoBehaviour
    {
        public Transform transform;

        public float panSpeed = 30f;
        public float panBorderThickness = 10f;

        public float scrollSpeed = 5f;
        public float minY = 20f;
        public float maxY = 80f;

        private void Update()
        {
            if (GameMaster.GameIsOver)
            {
                enabled = false;
                return;
            }

            if (Input.GetKey("w"))
            {
                transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey("s"))
            {
                transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey("d"))
            {
                transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey("a"))
            {
                transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");

            Vector3 pos = transform.position;

            pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            transform.position = pos;
                                                            
        }
    }
}