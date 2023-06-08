using UnityEngine;

namespace Shooter3D
{
    /// <summary>
    /// Турель
    /// </summary>
    public class Turret : Weapon
    {
        /// <summary>
        /// Основание турели
        /// </summary>
        [SerializeField] private Transform turretBase;
        /// <summary>
        /// Орудие турели
        /// </summary>
        [SerializeField] private Transform turretGun;
        /// <summary>
        /// Точка прицеливания
        /// </summary>
        [SerializeField] private Transform aim;

        /// <summary>
        /// Скорость поворота
        /// </summary>
        [SerializeField] private float rotationLerpFactor;

        /// <summary>
        /// Цель поворота основания
        /// </summary>
        private Quaternion baseTargetRotation;
        /// <summary>
        /// Поворот основания
        /// </summary>
        private Quaternion baseRotation;
        /// <summary>
        /// Цель поворота орудия
        /// </summary>
        private Quaternion gunTargetRotation;
        /// <summary>
        /// Поворот орудия
        /// </summary>
        private Vector3 gunRotation;


        protected override void Update()
        {
            base.Update();

            LookOnAim();
        }


        /// <summary>
        /// Повернуться в сторону прицела
        /// </summary>
        private void LookOnAim()
        {
            baseTargetRotation = Quaternion.LookRotation(new Vector3(aim.position.x, turretGun.position.y, aim.position.z) - turretGun.position);
            baseRotation = Quaternion.RotateTowards(turretBase.localRotation, baseTargetRotation, Time.deltaTime * rotationLerpFactor);
            turretBase.localRotation = baseRotation;

            gunTargetRotation = Quaternion.LookRotation(aim.position - turretBase.position);
            gunRotation = Quaternion.RotateTowards(turretGun.rotation, gunTargetRotation, Time.deltaTime * rotationLerpFactor).eulerAngles;
            turretGun.rotation = baseRotation * Quaternion.Euler(gunRotation.x, 0, 0);
        }
    }
}