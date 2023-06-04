using UnityEngine;

namespace Shooter3D
{
    [RequireComponent(typeof(Rigidbody))]

    /// <summary>
    /// Парящий транспорт
    /// </summary>
    public class HoverVehicle : Vehicle
    {
        /// <summary>
        /// Сила, толкающая вперёд
        /// </summary>
        [SerializeField] private float thrustForward;
        /// <summary>
        /// Сила, отвечающая за поворот
        /// </summary>
        [SerializeField] private float thrustTorque;

        /// <summary>
        /// Сопротивление движению
        /// </summary>
        [SerializeField] private float dragLinear;
        /// <summary>
        /// Сопротивление вращению
        /// </summary>
        [SerializeField] private float dragAngular;

        /// <summary>
        /// Высота парения
        /// </summary>
        [SerializeField] private float hoverHeight;

        /// <summary>
        /// Сила двигателя
        /// </summary>
        [SerializeField] private float hoverForce;

        /// <summary>
        /// Максимальная линейная скорость
        /// </summary>
        [SerializeField] private float maxLinearSpeed;

        /// <summary>
        /// Положение двигателей
        /// </summary>
        [SerializeField] private Transform[] hoverJets;

        private new Rigidbody rigidbody;

        /// <summary>
        /// Находимся на земле
        /// </summary>
        private bool isGrounded;


        #region Unity Events

        protected override void Start()
        {
            base.Start();

            rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            ComputeForces();
        }

        #endregion


        /// <summary>
        /// Применить силу двигателя
        /// </summary>
        /// <param name="tr">Позиция двигателя</param>
        /// <returns>Применима ли сила</returns>
        public bool ApplyJetForce(Transform tr)
        {
            Ray ray = new Ray(tr.position, -tr.up);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, hoverHeight))
            {
                float s = (hoverHeight - hit.distance) / hoverHeight;
                Vector3 force = s * hoverForce * hit.normal;

                rigidbody.AddForceAtPosition(force, tr.position, ForceMode.Acceleration);

                return true;
            }

            return false;
        }


        /// <summary>
        /// Рассчёт действующих сил
        /// </summary>
        private void ComputeForces()
        {
            isGrounded = false;

            for (int i = 0; i < hoverJets.Length; i++)
            {
                if (ApplyJetForce(hoverJets[i]))
                {
                    isGrounded = true;
                }
            }

            if (isGrounded)
            {
                rigidbody.AddRelativeForce(Vector3.forward * thrustForward * targetInputControl.z);
                rigidbody.AddRelativeTorque(Vector3.up * thrustTorque * targetInputControl.x);
            }

            // Линейное сопротивление
            {
                float dragCoeff = thrustForward / maxLinearSpeed;
                Vector3 dragForce = rigidbody.velocity * -dragCoeff;

                if (isGrounded)
                {
                    rigidbody.AddForce(dragForce, ForceMode.Acceleration);
                }
            }

            // Вращательное сопротивление
            {
                Vector3 dragForce = rigidbody.angularVelocity * -dragAngular;
                rigidbody.AddTorque(dragForce, ForceMode.Acceleration);
            }
        }
    }
}