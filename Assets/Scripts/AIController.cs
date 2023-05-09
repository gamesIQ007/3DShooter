using UnityEngine;

namespace Shooter3D
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private CubeArea area;

        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotateSpeed;

        private Vector3 movePosition;

        private void Start()
        {
            if (area == null)
            {
                area = FindObjectOfType<CubeArea>();
            }

            movePosition = GetNewMovePosition();
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, movePosition, Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movePosition), Time.deltaTime * rotateSpeed);

            if (transform.position == movePosition)
            {
                movePosition = GetNewMovePosition();
            }
        }

        private Vector3 GetNewMovePosition()
        {
            return area.GetRandomInsideZone();
        }
    }
}