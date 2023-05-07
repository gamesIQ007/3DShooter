using UnityEngine;

namespace Shooter3D
{
    public class ConverterCameraRotation : MonoBehaviour
    {
        [SerializeField] private new Transform camera;
        [SerializeField] private Transform cameraLook;
        [SerializeField] private Vector3 lookOffset;

        [SerializeField] private float topAngleLimit;
        [SerializeField] private float bottomAngleLimit;

        private void Update()
        {
            Vector3 angles = new Vector3(0, 0, 0);

            angles.z = camera.eulerAngles.x;

            if (angles.z >= topAngleLimit || angles.z <= bottomAngleLimit)
            {
                transform.LookAt(cameraLook.position + lookOffset);

                angles.x = transform.eulerAngles.x;
                angles.y = transform.eulerAngles.y;

                transform.eulerAngles = angles;
            }
        }
    }
}