using UnityEngine;

namespace Shooter3D
{
    [System.Serializable]
    /// <summary>
    /// Колёсная ось
    /// </summary>
    public class WheelAxle
    {
        /// <summary>
        /// Коллайдер левого колеса
        /// </summary>
        [SerializeField] private WheelCollider leftWheelCollider;
        /// <summary>
        /// Коллайдер правого колеса
        /// </summary>
        [SerializeField] private WheelCollider rightWheelCollider;

        /// <summary>
        /// Меш левого колеса
        /// </summary>
        [SerializeField] private Transform leftWheelMesh;
        /// <summary>
        /// Меш правого колеса
        /// </summary>
        [SerializeField] private Transform rightWheelMesh;

        /// <summary>
        /// Движущая ось
        /// </summary>
        [SerializeField] private bool motor;
        public bool Motor => motor;

        /// <summary>
        /// Поворотная ось
        /// </summary>
        [SerializeField] private bool steering;
        public bool Steering => steering;


        #region Public

        /// <summary>
        /// Задать крутящий момент
        /// </summary>
        /// <param name="torque">Крутящий момент</param>
        public void SetTorque(float torque)
        {
            if (motor == false) return;

            leftWheelCollider.motorTorque = torque;
            rightWheelCollider.motorTorque = torque;
        }

        /// <summary>
        /// Торможение
        /// </summary>
        /// <param name="breakTorque">Сила торможения</param>
        public void Brake(float brakeTorque)
        {
            leftWheelCollider.brakeTorque = brakeTorque;
            rightWheelCollider.brakeTorque = brakeTorque;
        }

        /// <summary>
        /// Задать поворот
        /// </summary>
        /// <param name="steerAngle">Угол поворота</param>
        public void SetSteerAngle(float steerAngle)
        {
            if (steering == false) return;

            leftWheelCollider.steerAngle = steerAngle;
            rightWheelCollider.steerAngle = steerAngle;
        }

        /// <summary>
        /// Синхронизация поворота колеса и его меша
        /// </summary>
        public void UpdateMeshTransform()
        {
            UpdateWheelTransform(leftWheelCollider, ref leftWheelMesh);
            UpdateWheelTransform(rightWheelCollider, ref rightWheelMesh);
        }

        #endregion


        /// <summary>
        /// Обновление трансформы колеса
        /// </summary>
        /// <param name="wheelCollider">Коллайдер колеса</param>
        /// <param name="wheelTransform">Трансформ колеса</param>
        private void UpdateWheelTransform(WheelCollider wheelCollider, ref Transform wheelTransform)
        {
            Vector3 position;
            Quaternion rotation;
            wheelCollider.GetWorldPose(out position, out rotation);
            wheelTransform.position = position;
            wheelTransform.rotation = rotation;
        }
    }

    [RequireComponent(typeof(Rigidbody))]

    /// <summary>
    /// Колёсный транспорт
    /// </summary>
    public class WheeledVehicle : Vehicle
    {
        /// <summary>
        /// Массив колёсных осей
        /// </summary>
        [SerializeField] private WheelAxle[] wheelAxles;

        /// <summary>
        /// Максимальный крутящий момент
        /// </summary>
        [SerializeField] private float maxMotorTorque;
        /// <summary>
        /// Максимальный угол поворота
        /// </summary>
        [SerializeField] private float maxSteerAngle;
        /// <summary>
        /// Максимальное торможение
        /// </summary>
        [SerializeField] private float maxBrakeTorque;

        public override float LinearVelocity => rigidbody.velocity.magnitude;

        /// <summary>
        /// Ригид
        /// </summary>
        private new Rigidbody rigidbody;


        #region Unity Events

        protected override void Start()
        {
            base.Start();

            rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            float targetMotor = maxMotorTorque * targetInputControl.z;
            float brake = maxBrakeTorque * targetInputControl.y;
            float steering = maxSteerAngle * targetInputControl.x;

            for (int i = 0; i < wheelAxles.Length; i++)
            {
                if (brake == 0 && LinearVelocity < maxLinearVelocity)
                {
                    wheelAxles[i].Brake(0);
                    wheelAxles[i].SetTorque(targetMotor);
                }

                if (LinearVelocity > maxLinearVelocity)
                {
                    wheelAxles[i].Brake(brake * 0.2f);
                }
                else
                {
                    wheelAxles[i].Brake(brake);
                }

                wheelAxles[i].SetSteerAngle(steering);

                wheelAxles[i].UpdateMeshTransform();
            }
        }

        #endregion
    }
}